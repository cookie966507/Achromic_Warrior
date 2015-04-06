using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Data;
using Assets.Scripts.Enums;
using Assets.Scripts.UI;

namespace Assets.Scripts.Enemies
{
	public class EnemySpawner : MonoBehaviour
	{
		public string _spawnName = "Spawn";
		public int _numSpawns = 0;

		public EnemiesRemaining _remaining;

		private List<Transform> _spawnNodes;

		public int _maxEnemiesOnScreen = 5;
		public int _numInLevel;
		private int _numEnemies = 0;

		public GameObject[] _enemyTypes;

		private float _spawnTimer = 0f;
		public float _spawnDelay = 5f;

		public bool _endless;

		public ColorElement[] _allowedColors;

		void Awake()
		{
			_spawnNodes = new List<Transform>();

			for(int i = 0; i < _numSpawns; i++)
			{
				Transform _node = GameObject.Find(_spawnName + i.ToString()).transform;
				_spawnNodes.Add(_node);
			}
		}

		void Start()
		{
			if(!_endless) _remaining.UpdateEnemiesRemaining(_numInLevel + _numEnemies);
		}
		
		void Update ()
		{
			if(!GameManager.SuspendedState)
			{
				if(_numInLevel > 0)
				{
					_spawnTimer += Time.deltaTime;
					if(_spawnTimer > _spawnDelay)
					{
						_spawnTimer = 0f;

						if(_numEnemies < _maxEnemiesOnScreen)
						{
							SpawnEnemy();
							_numEnemies++;
							if(!_endless)
							{
								_numInLevel--;
							}
						}
					}
				}
				else
				{
					if(_numEnemies == 0)
					{
						//end level here
						GameManager.State = GameState.Win;
					}
				}
			}
		}

		private void SpawnEnemy()
		{
			int _randomEnemy = Random.Range(0, _enemyTypes.Length);
			int _randomSpawn = Random.Range(0, _spawnNodes.Count);

			GameObject _newEnemy = _enemyTypes[_randomEnemy];

			GameObject _instantiatedEnemey = (GameObject)Instantiate(_newEnemy, _spawnNodes[_randomSpawn].position,  Quaternion.identity);

			_instantiatedEnemey.GetComponent<Enemy>().SaveSpawnerReference(this);

			if(_endless)
			{
				_instantiatedEnemey.GetComponent<Enemy>().Color = (ColorElement)Random.Range(0, (float)(ColorElement.NumTypes - 2));
			}
			else
			{
				_instantiatedEnemey.GetComponent<Enemy>().Color = _allowedColors[Random.Range(0, _allowedColors.Length)];
			}
		}

		public void EnemyDestroyed()
		{
			_numEnemies--;
			if(!_endless) _remaining.UpdateEnemiesRemaining(_numInLevel + _numEnemies);
		}
	}
}