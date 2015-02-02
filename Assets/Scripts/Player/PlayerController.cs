using UnityEngine;
using System.Collections;
using Assets.Scripts;

/*
 * Controls te player's movement
 */
namespace Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        //reference to the jumper
        private Jumper _jumper;
        //jump button pressed
        private static bool _jump = false;
        //how much control you have in the air
        private float _airControl = 0.3f;

        //reference to the attack
        public PlayerAttack _attack;
        private static PlayerAttack attack;

        //firection the player is facing
        [HideInInspector]
        public bool _facingRight = true;

        //how quickly player accelerates
        public float _moveForce = 100f;
        //capping the speed of the player
        public float _maxSpeed = 4f;

        //for seeing if we pressed the down button once during the double tap
        private float _downTimer = 0f;
        private float _downDelay = 0.5f;
        private bool _downFirstPress = false;

        //for ghosting through platforms
        private float _ghostTimer = 0f;
        private float _ghostDelay = 0.5f;
        private bool _ghost = false;

        //cheat for tricking Unity when dealing with collision magic
        private BoxCollider2D _playerCollider;
        private bool _colSwitch = false;

        private bool animDone = false;
        private PlayerStateMachine machine;
        private delegate void state();
        private state[] doState;
        private bool hit;
        private Player.PlayerStateMachine.State prevState = 0;

        void Awake()
        {
            //find jumper
            _jumper = this.GetComponentInChildren<Jumper>();
            //find collider
            _playerCollider = this.GetComponent<BoxCollider2D>();
            attack = _attack;
            machine = new PlayerStateMachine();
            doState = new state[] { Idle, Move, Jump, InAirNow, Attack1, Attack2, Attack3, MovingAttack, InAirAttack, Parry, Block, Crouch, Hit, Dead };
        }

        void Update()
        {
            Player.PlayerStateMachine.State currState = machine.update(!_jumper.CanJump(), false, false, !CustomInput.Jump);
            Debug.Log(currState);
            doState[(int)currState]();
            if(prevState!=currState)
            {
                attack.Hide();
                _jump = false;
                //change anim
            }

            //if our cheat is on, out collider is off, so we need to tun it back on
            if (_colSwitch)
            {
                _colSwitch = false;
                _playerCollider.enabled = true;
            }

            //if the button has been pressed once
            if (_downFirstPress) _downTimer += Time.deltaTime;

            //if down is pressed
            if (CustomInput.DownFreshPress)
            {
                //and it is the first time
                if (!_downFirstPress)
                {
                    //set down as being pressed
                    _downFirstPress = true;
                    _downTimer = 0f;
                }
                //second press
                else
                {
                    //took too long to double tap
                    if (_downTimer > _downDelay)
                    {
                        //reset we have pushed down once
                        _downFirstPress = true;
                        _downTimer = 0f;
                    }
                    //double tapped in time
                    else
                    {
                        //we should be ghosting
                        _ghost = true;
                        _ghostTimer = 0f;
                        _downFirstPress = false;
                        //turn on the cheat for collision stuff
                        _colSwitch = true;
                        _playerCollider.enabled = false;
                    }
                }
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
                if (_jumper.CanJump()) rigidbody2D.AddForce(Vector2.right * _h * _moveForce);
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

            //if jump was pressed
            if (_jump)
            {
                //and player can jump
                if (_jumper.CanJump())
                {
                    //add upward force
                    rigidbody2D.AddForce(new Vector2(0f, _jumper.Force), ForceMode2D.Impulse);
                }
                _jump = false;
            }

        }

        //flipping so player faces the correct position
        void Flip()
        {
            // Switch the way the player is labelled as facing.
            _facingRight = !_facingRight;
            this.transform.localScale=new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);

            /* this should be unnecessary as child objects are relative to the parent so changing the parent scale changes theirs too
             * -Jonathan
            //sprites should be separate from the collider to get rid of weird collision stuff (that I understand)
            Transform _sprites = transform.FindChild("Sprites");
            // Multiply the player's x local scale by -1.
            Vector3 _scale = _sprites.localScale;
            _scale.x *= -1;
            _sprites.localScale = _scale;
             */
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
            attack.gameObject.SetActive(true);
        }
        private static void Attack2()
        {
            attack.gameObject.SetActive(true);
        }
        private static void Attack3()
        {
            attack.gameObject.SetActive(true);
        }

        private static void MovingAttack()
        {
            attack.gameObject.SetActive(true);
        }

        private static void InAirAttack()
        {
            attack.gameObject.SetActive(true);
        }
        private static void Move()
        {
        }
        private static void Parry()
        {
        }
        private static void Block()
        {
        }
        private static void Crouch()
        {
        }
        private static void Jump()
        {
            _jump = true;
        }
        private static void InAirNow()
        {
        }

        private static void Hit()
        {
        }

        private static void Dead()
        {
        }
    }
}
