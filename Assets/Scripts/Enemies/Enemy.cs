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

		//health
		public int _health = 10;

        //time before enemy can be hit again
        public float _hitTime = 1f;
        private float _timer;
        public bool _hit = false;

        //layers for ghosting/reverting
        protected int ENEMY;
        protected int GHOSTING_ENEMY;

        //for ghosting through platforms
        protected float _ghostTimer = 0f;
        protected float _ghostDelay = 0.5f;

        //orbs to spawn when hit
        [HideInInspector]
        public GameObject _orb;

        //reference to the player
        public GameObject _player;

		//enemy parts that should be colored
		public Renderer[] _coloredPieces;

        //knockback force
        public float _force = 75f;

		private EnemySpawner _spawner;

        //Hack
        bool ExitCatcher = true;

        void Start()
        {
            Init();
            StartUp();
        }

        void Update()
        {
            if (!Data.GameManager.Paused)
            {
                UpdateHit();
                if (this.gameObject.layer.Equals(GHOSTING_ENEMY)) UpdateGhost();
                Run();
            }
        }

        //subclasses should implement these instead of Start() and Update() 
        //so we can control enemy globals easily from this class
        protected abstract void StartUp();
        protected abstract void Run();

        // Produces the orbs
        public void ProduceOrbs(int _num)
        {
            for (int i = 0; i < _num; i++)
            {
                //create a new orb
                GameObject _newOrb = (GameObject)GameObject.Instantiate(_orb, transform.position, Quaternion.identity);
                //set the orb color
                _newOrb.GetComponent<ColorPickup>().ColorType = this.ResolveMultiColor(_color);
                //throw it in the air
                _newOrb.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(Random.Range(-1f, 1f), Random.Range(5, 10)), ForceMode2D.Impulse);
            }
        }

        //Initialize the enemy
        public void Init()
        {
            //load the orb
            _orb = (GameObject)Resources.Load("Prefabs/orb");

            //find the player
            _player = GameObject.Find("player");

            ENEMY = LayerMask.NameToLayer("enemy");
            GHOSTING_ENEMY = LayerMask.NameToLayer("ghosting_enemy");

			//color parts that are supposed to be toned
			for(int i = 0; i < _coloredPieces.Length; i++)
			{
				_coloredPieces[i].material.color = CustomColor.GetColor(_color);
			}
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
				//subtract health
				_health -= _damage;
				if(_health <= 0)
				{
					this.Die();
				}
                //apply knockback force based on position
                if (_player.GetComponent<Player.PlayerController>()._facingRight)
                    GetComponent<Rigidbody2D>().AddRelativeForce((new Vector2(1, 0)) * _force * _player.GetComponent<Player.PlayerColorData>().Attack);
                else
                {
                    GetComponent<Rigidbody2D>().AddRelativeForce((new Vector2(-1, 0)) * _force * _player.GetComponent<Player.PlayerColorData>().Attack);
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
            if (ExitCatcher)
                ExitGhost();
            _ghostTimer += Time.deltaTime;
            //small delay to let you fall
            if (_ghostTimer > _ghostDelay)
            {
                //Makes up for trigger exit not being called
                ExitCatcher = true;
            }
        }

        //Enemy ghost init by platform trigger
        public void EnterGhost()
        {
            if (this.gameObject.layer != GHOSTING_ENEMY)
                this.gameObject.layer = GHOSTING_ENEMY;
            ExitCatcher = false;
            _ghostTimer = 0;
        }

        //Enemy ghost exited by leaving platform trigger
        public virtual void ExitGhost()
        {
            if (this.gameObject.layer != ENEMY)
                this.gameObject.layer = ENEMY;
            _ghostTimer = _ghostDelay+1;
        }

        //Gets or sets the color
        public ColorElement Color
        {
            get { return _color; }
            set
			{ 
				_color = value;
				//color parts that are supposed to be toned
				for(int i = 0; i < _coloredPieces.Length; i++)
				{
					_coloredPieces[i].material.color = CustomColor.GetColor(_color);
				}
			}
        }

        public void OnTriggerEnter2D(Collider2D col)
        {
            if (Data.GameManager.State != GameState.Pause)
            {
                if (col.tag == "Player Attack")
                {
                    Hit((int)_player.GetComponent<Player.PlayerColorData>().Attack, this.transform.position);
                }
            }
        }

		public virtual void Die()
		{
			_spawner.EnemyDestroyed();
			Destroy(gameObject);
		}

		public void SaveSpawnerReference(EnemySpawner _spawner)
		{
			this._spawner = _spawner;
		}

		public ColorElement ResolveMultiColor(ColorElement _color)
		{
			return CustomColor.Weights(_color);
		}
    }
}
