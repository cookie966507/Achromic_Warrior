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
		private int _maxHealth;

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
        private float _force = 2f;

		private EnemySpawner _spawner;

		protected int _dir = 1;

		public GameObject _healthBar;

		protected float _primary = 0.8f;
		protected float _secondary = 0.4f;
		protected float _upperTertiary = 0.6f;
		protected float _lowerTertiary = 0.2f;

		//base stats
		public float _baseAtk = 2f;
		public float _baseDef = 0f;
		public float _baseSpd = 4f;
		
		//changing stats (used as ratio with _primary, _secondary, etc)
		private float _atk = 0f;
		private float _def = 0f;
		private float _spd = 0f;
		
		//max a stat can be
		public float _maxAtk = 8f;
		public float _maxSpd = 11f;

        //Hack
        bool ExitCatcher = true;

        void Start()
        {
            Init();
            StartUp();
        }

        void Update()
        {
            if (!Data.GameManager.SuspendedState)
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
			_maxHealth = _health;
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

			this.UpdateStats();
			_baseAtk = this.GetComponent<CustomDamage>().damage;
        }

        //what happens when hit
        public virtual void Hit(int _damage, Vector3 _hitPos)
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
				float calcDamage = _damage * (1-(this.DefenseRatio));
				_damage = Mathf.CeilToInt(calcDamage);
                //show the damage taken
                UI.DamageDisplay.instance.ShowDamage(_damage, _hitPos, _color);
				//subtract health
				_health -= _damage;
				_healthBar.transform.localScale = new Vector3((float)_health/_maxHealth, 1, 1);

				if(_health <= 0)
				{
					this.Die();
				}
                //apply knockback force based on position
				float _finalForce =  _force * _player.GetComponent<Player.PlayerColorData>().Attack;
				_finalForce = Mathf.Clamp(_finalForce, 5f, 20f);
                if (_player.GetComponent<Player.PlayerController>()._facingRight)
                    GetComponent<Rigidbody2D>().AddRelativeForce((new Vector2(1f, 0.25f)) * _finalForce, ForceMode2D.Impulse);
                else
                {
                    GetComponent<Rigidbody2D>().AddRelativeForce((new Vector2(-1f, 0.25f)) * _finalForce, ForceMode2D.Impulse);
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
            if(_spawner!=null)
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

		public void Flip()
		{
			//turn sprites around, not actual gameobject
			Transform _sprites = transform.Find("Sprites");
			//change dir
			_dir *= -1;
			_sprites.localScale = new Vector2(_dir, _sprites.localScale.y);
		}

		public void UpdateStats()
		{
			switch (_color)
			{
			case ColorElement.Red:
				_atk = _primary;
				_spd = 0f;
				_def = 0f;
				break;
				
			case ColorElement.Green:
				_atk = 0f;
				_spd = _primary;
				_def = 0f;
				break;
				
			case ColorElement.Blue:
				_atk = 0f;
				_spd = 0f;
				_def = _primary;
				break;
				
			case ColorElement.Yellow:
				_atk = _secondary;
				_spd = _secondary;
				_def = 0f;
				break;
				
			case ColorElement.Cyan:
				_atk = 0f;
				_spd = _secondary;
				_def = _secondary;
				break;
				
			case ColorElement.Magenta:
				_atk = _secondary;
				_spd = 0f;
				_def = _secondary;
				break;
				
			case ColorElement.Orange:
				_atk = _upperTertiary;
				_spd = _lowerTertiary;
				_def = 0f;
				break;
				
			case ColorElement.Chartreuse:
				_atk = _lowerTertiary;
				_spd = _upperTertiary;
				_def = 0f;
				break;
				
			case ColorElement.Spring:
				_atk = 0f;
				_spd = _upperTertiary;
				_def = _lowerTertiary;
				break;
				
			case ColorElement.Azure:
				_atk = 0f;
				_spd = _lowerTertiary;
				_def = _upperTertiary;
				break;
				
			case ColorElement.Rose:
				_atk = _upperTertiary;
				_spd = 0f;
				_def = _lowerTertiary;
				break;
				
			case ColorElement.Violet:
				_atk = _lowerTertiary;
				_spd = 0f;
				_def = _upperTertiary;
				break;
				
			case ColorElement.Black:
				_atk = _primary;
				_spd = _primary;
				_def = _primary;
				break;
			}
		}

		//stats that return modified values based on color
		public float Attack
		{
			get { return _baseAtk + _atk * _maxAtk; }
		}

		public float DefenseRatio
		{
			get { return _def; }
		}
		
		public float Speed
		{
			get { return _baseSpd + _spd * _maxSpd; }
		}
    }
}
