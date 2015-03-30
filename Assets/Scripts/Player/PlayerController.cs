using UnityEngine;
using System.Collections;
using Assets.Scripts.Data;
using Assets.Scripts.Enums;
using Assets.Scripts.UI;
using Assets.Scripts.Util;

/*
 * Controls te player's movement
 */
namespace Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        public Transform foot;
        public float JumpForce = 13.5f;
        //reference to the attack
        public ObjectHider _attack;
        public ObjectHider _block;
        public Animator anim;


        //firection the player is facing
        [HideInInspector]
        public bool _facingRight = true;

        //how quickly player accelerates
        public float _moveForce = 200f;
        //capping the speed of the player
        public float _maxSpeed = 4f;


        //reference to the attack
        private static ObjectHider attack;
        private static ObjectHider block;
        //for ghosting through platforms
        private static float _ghostTimer = 0f;
        private static bool _ghost = false;
        //cheat for tricking Unity when dealing with collision magic
        private static BoxCollider2D _playerCollider;
        private static bool _colSwitch = false;
        private static bool doOnce = false;
        //jump button pressed
        private static bool _jump = false;


        //how much control you have in the air
        private float _airControl = 0.3f;
        //for ghosting through platforms
        private float _ghostDelay = 1f;

        private int temp;//delete when anim event is done

        //state machine vars

        private float parryTime = .2f;
        private float renderTime = .002f;
        private float renderTimer = 0;
        private static bool invun = false;
        private static float invunTime = .5f;
        private static float invunTimer = 0;
        private PlayerColorData colorData;
        private static int damage = 0;
        private float parryTimer = 0;
        private bool animDone = false;
        private bool inAir = false;
        private bool blockSucessful = false;
        private bool hit = false;
        private bool render = false;
        private bool enemyOnRight=false;
        private static bool blocking = false;
        public static bool achromic = false;
        private PlayerStateMachine machine;
        private delegate void state();
        private state[] doState;
        private PlayerState prevState = 0;
        private PlayerState currState = 0;
        private PlayerArmor[] _armor;

        void Awake()
        {
            //find collider
            _playerCollider = this.GetComponent<BoxCollider2D>();
            colorData = this.gameObject.GetComponent<PlayerColorData>();
            _armor = GameObject.FindObjectsOfType<PlayerArmor>();
            //state machine init
            attack = _attack;
            block = _block;
            machine = new PlayerStateMachine();
            doState = new state[] { Idle, Move, Jump, Jump2, InAirNow, Attack1, Attack2, Attack3, MovingAttack, InAirAttack, Parry, Block, Crouch, Hit, Dead };
        }

        void Update()
        {
            if (!GameManager.Paused)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                    colorData.AddColor(Color.white, 500f);

                //going Achromic
                if (CustomInput.SuperFreshPress && colorData.isfull())
                {
                    achromic = true;
                }

                if (temp++ > 3)
                    animDone = true;

                TouchingSomething();
                if (_ghost)
                    inAir = true;
                //get next state
                currState = machine.update(inAir, blockSucessful, hit, animDone);
                if (invunTimer > 0)
                {
                    if (renderTimer > renderTime)
                    {
                        render = !render;
                        renderTimer = 0;
                        foreach (PlayerArmor pa in _armor)
                            pa.GetComponent<Renderer>().enabled = render;
                    }
                    hit = false;
                    renderTimer += Time.deltaTime;
                    invunTimer -= Time.deltaTime;
                }
                else
                {
                    foreach (PlayerArmor pa in _armor)
                        pa.GetComponent<Renderer>().enabled = true;
                    invun = false;
                }

                //run state
                doState[(int)currState]();
                //state clean up
                if (blockSucessful)
                {
                    parryTimer += Time.deltaTime;
                    if (parryTimer > parryTime)
                    {
                        blockSucessful = false;
                        parryTimer = 0;
                    }
                }
                if (prevState != currState)
                {
                    doOnce = false;
                    animDone = false;
                    temp = 0;
                    attack.Hide();
                    block.Hide();
                    _jump = false;
                    blocking = false;
                    blockSucessful = false;
                    parryTimer = 0;
                    hit = false;
                    anim.SetInteger("state", (int)currState);
                }
                prevState = currState;

                Ghosting();
            }
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (GameManager.State != GameState.Pause)
            {
                if (col.gameObject.tag == "enemy" && !invun)
                {
                    if (blocking)
                        blockSucessful = true;
                    else
					{
                        hit = true;
						CustomDamage potentialDamage = col.gameObject.GetComponent<CustomDamage>();                       
						if (potentialDamage != null)
						{
							damage = potentialDamage.damage;
							damage -= (int)colorData.Defense;
							if (blockSucessful)
								damage -= (int)(colorData.Defense * .5f);
							if (damage < 0)
								damage = 0;
							DamageDisplay.instance.ShowDamage(damage, transform.position, ColorElement.White);
						}
						else
							damage = 0;
					}
					}
                    if (col.gameObject.transform.position.x < this.gameObject.transform.position.x)
                        enemyOnRight = false;
                    else
						enemyOnRight = true;
            }
        }

        public void AnimDetector()
        {
            animDone = true;
        }

        //detects if you are in the air
        private void TouchingSomething()
        {
            int _layerMask = (1 << LayerMask.NameToLayer("player")) | (1 << LayerMask.NameToLayer("orb_collector") | (1 << LayerMask.NameToLayer("Default")));
            //compliment to collide with all EXCEPT these layers
            _layerMask = ~_layerMask;
            RaycastHit2D temp = Physics2D.Raycast(foot.position, -Vector2.up, 0.05f, _layerMask);
            //RaycastHit2D temp2 = Physics2D.Raycast(frontFoot.position, -Vector2.up, 0.05f);
            if (temp.collider != null)
            {
                //allow falling through untagged triggers
                inAir = temp.collider.tag == "Untagged";
                //consider removing may be unneeded
                //if (temp.collider.tag == "enemy")
                //{
                //    hit = true;
                //    if (temp.collider.gameObject.transform.position.x < this.gameObject.transform.position.x)
                //        enemyOnRight = false;
                //    else
                //        enemyOnRight = true;
                //    CustomDamage potentialDamage = temp.collider.gameObject.GetComponent<CustomDamage>();
                //    if (potentialDamage != null)
                //    {
                //        damage = potentialDamage.damage;
                //        DamageDisplay.instance.ShowDamage(damage, transform.position, ColorElement.White);
                //    }
                //    else
                //        damage = 0;
                //}
            }
            else
                inAir = true;
        }

        private void Ghosting()
        {
            //if our cheat is on, out collider is off, so we need to tun it back on
            if (_colSwitch)
            {
                _colSwitch = false;
                _playerCollider.enabled = true;
            }
            //if ghosting
            if (_ghost) _ghostTimer += Time.deltaTime;

            //if ghosting is over
            if (_ghostTimer > _ghostDelay)
            {
                _ghost = false;
                _ghostTimer = 0;
            }
            //ignore collision between the player and platforms
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("player"), LayerMask.NameToLayer("platform"), _ghost);
        }

        //fixed update runs on a timed cycle (for physics stuff)
        void FixedUpdate()
        {
            if (GameManager.State != GameState.Pause)
            {
                if (currState == PlayerState.move ||
                    currState == PlayerState.movingAttack ||
                    currState == PlayerState.inAir ||
                    currState == PlayerState.inAirAttack)
                {
                    //caching the horizontal input
                    float _h;

                    //assign value based on left or right input
                    if (CustomInput.Left) _h = -1f;
                    else if (CustomInput.Right) _h = 1f;
                    else _h = 0f;


                    // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
                    if (_h * GetComponent<Rigidbody2D>().velocity.x < _maxSpeed)
                    {
                        //account for air control
                        if (!inAir) GetComponent<Rigidbody2D>().AddForce(Vector2.right * _h * _moveForce);
                        else GetComponent<Rigidbody2D>().AddForce(Vector2.right * _h * _moveForce * _airControl);
                    }

                    // If the player's horizontal velocity is greater than the maxSpeed...
                    if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > _maxSpeed)
                    {
                        // ... set the player's velocity to the maxSpeed in the x axis.
                        GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * _maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
                    }

                    //flippng based on direction and movement
                    if (_h > 0 && !_facingRight)
                        Flip();
                    else if (_h < 0 && _facingRight)
                        Flip();
                }
                //STATE MACHINE SAY JUMP NOW!!!
                if (_jump)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0f);
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
					_ghost = true;
                    _jump = false;
                }
                if (currState == PlayerState.hit)
                {

                    float _h;
                    if (enemyOnRight) _h = -1f;
                    else _h = 1f;


                    // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
                    if (_h * GetComponent<Rigidbody2D>().velocity.x < _maxSpeed)
                    {
                        //account for air control
                        if (!inAir) GetComponent<Rigidbody2D>().AddForce(Vector2.right * _h * _moveForce);
                        else GetComponent<Rigidbody2D>().AddForce(Vector2.right * _h * _moveForce * _airControl);
                    }

                    // If the player's horizontal velocity is greater than the maxSpeed...
                    if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > _maxSpeed)
                    {
                        // ... set the player's velocity to the maxSpeed in the x axis.
                        GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * _maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
                    }
                }
            }
        }

        //flipping so player faces the correct position
        void Flip()
        {
            // Switch the way the player is labelled as facing.
            _facingRight = !_facingRight;
            //this.transform.localScale=new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);

            //sprites should be separate from the collider to get rid of weird collision stuff (that I understand)
            Transform _sprites = transform.FindChild("Sprites");
            // Multiply the player's x local scale by -1.
            Vector3 _scale = _sprites.localScale;
            _scale.x *= -1;
            _sprites.localScale = _scale;

        }

        //updating the max speed; this will control how fast the player moves
        public void UpdateSpeed(float _maxSpeed)
        {
            this._maxSpeed = _maxSpeed;
        }

        //getter-setter for ghosting
        public bool Ghost
        {
            get { return _ghost; }
            set
            {
                _ghost = value;
                _ghostTimer = 0f;
            }
        }

        private static void Idle()
        {
        }

        private static void Attack1()
        {
            if (!doOnce)
            {
                doOnce = true;
                attack.Show();
            }
        }
        private static void Attack2()
        {
            if (!doOnce)
            {
                doOnce = true;
                attack.Show();
            }
        }
        private static void Attack3()
        {
            if (!doOnce)
            {
                doOnce = true;
                attack.Show();
            }
        }

        private static void MovingAttack()
        {
            if (!doOnce)
            {
                doOnce = true;
                attack.Show();
            }
        }

        private static void InAirAttack()
        {
            if (!doOnce)
            {
                doOnce = true;
                attack.Show();
            }
        }
        private static void Move()
        {
        }
        private static void Parry()
        {

        }
        private static void Block()
        {
            if (!doOnce)
            {
                doOnce = true;
                blocking = true;
                block.GetComponent<Renderer>().material.color = CustomColor.GetColor(FindObjectOfType<PlayerColorData>().Color);
                block.Show();
            }
        }
        private static void Crouch()
        {
            if (CustomInput.JumpFreshPress)
            {
                //we should be ghosting
                _ghost = true;
                _ghostTimer = 0f;

				_colSwitch = true;
				_playerCollider.enabled = false;
            }
        }
        private static void Jump()
        {
            if (!doOnce)
            {
                doOnce = true;
                _jump = true;
            }
        }
        private static void Jump2()
        {
            if (!doOnce)
            {
                doOnce = true;
                _jump = true;
            }
        }
        private static void InAirNow()
        {
            if (CustomInput.Down && CustomInput.JumpFreshPress)
            {
                //we should be ghosting
                _ghost = true;
                _ghostTimer = 0f;

                _colSwitch = true;
                _playerCollider.enabled = false;
            }
        }

        private static void Hit()
        {
            if (!doOnce)
            {
                PlayerLifeData.damageHealth(damage);
                damage = 0;
                doOnce = true;
                invunTimer = invunTime;
                invun = true;
            }
        }

        private static void Dead()
        {
        }
    }
}