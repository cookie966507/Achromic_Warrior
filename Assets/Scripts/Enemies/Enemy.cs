using UnityEngine;
using System.Collections;
using Assets.Scripts.Enums;

/*
 * Base enemy class
 */
namespace Assets.Scripts.Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        //color element associated with the enemy
        public ColorElement _color;

        //time before enemy can be hit again
        public float _hitTime = 1f;
        private float _timer;
        public bool _hit = false;

        //layers for ghosting/reverting
        private int ENEMY;
        private int GHOSTING_ENEMY;

        //for ghosting through platforms
        private float _ghostTimer = 0f;
        private float _ghostDelay = 0.5f;

        //orbs to spawn when hit
        [HideInInspector]
        public GameObject _orb;

        //reference to the player
        public GameObject _player;

        //knockback force
        public float _force = 75f;

        void Start()
        {
            Init();
            StartUp();
        }

        void Update()
        {
            UpdateHit();
            if (this.gameObject.layer.Equals(GHOSTING_ENEMY)) UpdateGhost();
            Run();
        }

        //subclasses should implement these instead of Start() and Update() 
        //so we can control enemy globals easily from this class
        protected abstract void Run();
        protected abstract void StartUp();

        // Produces the orbs
        public void ProduceOrbs(int _num)
        {
            for (int i = 0; i < _num; i++)
            {
                //create a new orb
                GameObject _newOrb = (GameObject)GameObject.Instantiate(_orb, transform.position, Quaternion.identity);
                //set the orb color
                _newOrb.GetComponent<ColorPickup>().ColorType = _color;
                //throw it in the air
                _newOrb.rigidbody2D.AddRelativeForce(new Vector2(Random.Range(-1f, 1f), Random.Range(5, 10)), ForceMode2D.Impulse);
            }
        }

        //Initialize the enemy
        public void Init()
        {
            //set the enemy color
            renderer.material.color = CustomColor.GetColor(_color);

            //load the orb
            _orb = (GameObject)Resources.Load("Prefabs/orb");

            //find the player
            _player = GameObject.Find("player");

            ENEMY = LayerMask.NameToLayer("enemy");
            GHOSTING_ENEMY = LayerMask.NameToLayer("ghosting_enemy");
        }

        //what happens when hit
        public void Hit(int _damage, Vector3 _hitPos)
        {
            //if not already hit
            if (!_hit)
            {
                //reset hit
                _hit = true;
                _timer = 0f;
                //if player is not achromic produce more orbs ------- CHANGE TO PLAYER STATE LATER
                if (!_player.GetComponent<Player.PlayerColorData>().Color.Equals(ColorElement.Black))
                    ProduceOrbs(1);
                //show the damage taken
                UI.DamageDisplay.instance.ShowDamage(_damage, _hitPos, _color);
                //apply knockback force based on position
                if (_player.GetComponent<Player.PlayerController>()._facingRight)
                    rigidbody2D.AddRelativeForce((new Vector2(1, 0)) * _force * _player.GetComponent<Player.PlayerColorData>().Attack);
                else
                {
                    rigidbody2D.AddRelativeForce((new Vector2(-1, 0)) * _force * _player.GetComponent<Player.PlayerColorData>().Attack);
                }
            }
        }

        //Updates the hit
        public void UpdateHit()
        {
            if (_hit)
            {
                _timer += Time.deltaTime;
                if (_timer > _hitTime)
                {
                    _hit = false;
                }
            }
        }

        //Ghosting
        public void UpdateGhost()
        {
            _ghostTimer += Time.deltaTime;
            if (_ghostTimer > _ghostDelay)
            {
                _ghostTimer = 0f;
                this.gameObject.layer = ENEMY;
            }
        }

        //Enemy ghost init by platform trigger
        public void InitGhost()
        {
            this.gameObject.layer = GHOSTING_ENEMY;
            _ghostTimer = 0;
        }

        //Enemt ghost exited by leaving platform trigger
        public void ExitGhost()
        {
            this.gameObject.layer = ENEMY;
            _ghostTimer = 0;
        }

        //Gets or sets the color
        public ColorElement Color
        {
            get { return _color; }
            set { _color = value; }
        }
    }
}
