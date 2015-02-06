using UnityEngine;
using System.Collections;
using Assets.Scripts.Data;
using Assets.Scripts.Enums;
using Assets.Scripts.Util;

/*
 * Controls te player's movement
 */
namespace Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        public Transform foot;
        public float JumpForce = 10;
        //reference to the attack
        public ObjectHider _attack;
        public ObjectHider _block;

        //firection the player is facing
        [HideInInspector]
        public bool _facingRight = true;

        //how quickly player accelerates
        public float _moveForce = 100f;
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
        private float _ghostDelay = 0.5f;
        private int temp;
        private bool animDone = false;
        private bool inAir = false;
        private bool hit=false;
        private static bool blocking = false;
        private bool blockSucessful = false;
        private PlayerStateMachine machine;
        private delegate void state();
        private state[] doState;
        private PlayerState prevState = 0;
        private PlayerState currState = 0;

        void Awake()
        {
            //find collider
            _playerCollider = this.GetComponent<BoxCollider2D>();
            attack = _attack;
            block = _block;
            machine = new PlayerStateMachine();
            doState = new state[] { Idle, Move, Jump, Jump2, InAirNow, Attack1, Attack2, Attack3, MovingAttack, InAirAttack, Parry, Block, Crouch, Hit, Dead };
        }

        void Update()
        {
            if (GameManager.State != GameState.Pause)
            {
                if (temp++ > 3)
                    animDone = true;
                TouchingSomething();
                //get next state
                currState = machine.update(inAir, false, false, animDone);
                //run state
                doState[(int)currState]();
                //state clean up
                if(blockSucessful)
                    blockSucessful = false;
                if (prevState != currState)
                {
                    doOnce = false;
                    animDone = false;
                    temp = 0;
                    attack.Hide();
                    block.Hide();
                    _jump = false;
                    blocking = false;
                    //TODO: change anim
                }
                prevState = currState;

                Ghosting();
            }
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (GameManager.State != GameState.Pause)
            {
                if(col.gameObject.tag=="enemy")
                {
                    if (blocking)
                        blockSucessful = true;
                    else
                        hit = true;
                }
            }
        }

        //detects if you are in the air
        //support for two feet is commented out
        private void TouchingSomething()
        {
            int _layerMask = (1 << LayerMask.NameToLayer("player")) | (1 << LayerMask.NameToLayer("orb_collector") | (1 << LayerMask.NameToLayer("Default")));
            //compliment to collide with all EXCEPT these layers
            _layerMask = ~_layerMask;
            RaycastHit2D temp = Physics2D.Raycast(foot.position, -Vector2.up, 0.05f, _layerMask);
            //RaycastHit2D temp2 = Physics2D.Raycast(frontFoot.position, -Vector2.up, 0.05f);
            if (temp != null && temp.collider != null)
            {
                //allow falling through untagged triggers
                inAir = temp.collider.tag == "Untagged";
                if (temp.collider.tag == "enemy")
                {
                    hit = true;
                }
            }
            /*lse if (temp2 != null && temp2.collider != null)
            {
                inAir = temp2.collider.tag == "Untagged";
                if (temp2.collider.tag == "Pit")
                {
                    hit = true;
                    health = 0;
                }
                if (temp2.collider.tag == "Enemy")
                {
                    hit = true;
                }
            }*/
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
                    if (_h * rigidbody2D.velocity.x < _maxSpeed)
                    {
                        //account for air control
                        if (!inAir) rigidbody2D.AddForce(Vector2.right * _h * _moveForce);
                        else rigidbody2D.AddForce(Vector2.right * _h * _moveForce * _airControl);
                    }

                    // If the player's horizontal velocity is greater than the maxSpeed...
                    if (Mathf.Abs(rigidbody2D.velocity.x) > _maxSpeed)
                        // ... set the player's velocity to the maxSpeed in the x axis.
                        rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * _maxSpeed, rigidbody2D.velocity.y);

                    //flippng based on direction and movement
                    if (_h > 0 && !_facingRight)
                        Flip();
                    else if (_h < 0 && _facingRight)
                        Flip();
                }
                //STATE MACHINE SAY JUMP NOW!!!
                if (_jump)
                {
                    rigidbody2D.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
                    _jump = false;
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
                block.Show();
                block.renderer.material.color = CustomColor.GetColor(FindObjectOfType<PlayerColorData>().Color);
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
            Debug.Log("I'M INVINCIBLE HAHAHAHAHAHAHAHAHAHAH!!!!!!!");
        }

        private static void Dead()
        {
        }
    }
}
