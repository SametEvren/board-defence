using System;
using System.Collections;
using System.Collections.Generic;
using Board;
using DG.Tweening;
using Game;
using Particles;
using UIScripts;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;
using Zenject;

namespace Enemies
{
    public class EnemySpawnController : MonoBehaviour
    {
        [SerializeField] private Transform enemyParent;
        
        private ObjectPool<GameObject> _mummyEnemyPool;
        [SerializeField] private GameObject mummyPrefab;
        private const int MummyCapacity = 10;
        
        private ObjectPool<GameObject> _catEnemyPool;
        [SerializeField] private GameObject catPrefab;
        private const int CatCapacity = 10;
        
        private ObjectPool<GameObject> _birdEnemyPool;
        [SerializeField] private GameObject birdPrefab;
        private const int BirdCapacity = 10;
        
        [SerializeField] private List<EnemyInventory> enemyInventory;
        public event Action AllEnemiesDefeated;
        
        private const float SpawnInterval = 5f;
        private int _totalEnemies;
        private int _defeatedEnemies;
        public List<Vector2Int> possibleSpawnPositions { get; private set; }
        
        
        [Inject]
        private IEnemyFactory _enemyFactory;
        
        private BoardController _boardController;
        private GameController _gameController;
        private ParticlePool _particlePool;
        
        [Inject]
        public void Construct(BoardController boardController, GameController gameController, ParticlePool particlePool)
        {
            _boardController = boardController;
            _gameController = gameController;
            _particlePool = particlePool;
        }
        
        private void OnValidate()
        {
            Assert.IsNotNull(enemyParent);
            Assert.IsNotNull(mummyPrefab);
            Assert.IsNotNull(catPrefab);
            Assert.IsNotNull(birdPrefab);
        }
        
        private void Start()
        {
            SetInventory();
            CalculatePossibleSpawnPositions();
            SetPools();
            StartCoroutine(SpawnEnemiesInSequence());
        }

        private void SetInventory()
        {
            enemyInventory.Clear();
            foreach (var enemyItem in _gameController.LevelData.enemyInventories)
            {
                enemyInventory.Add(new EnemyInventory(enemyItem.enemyType, enemyItem.amount));
                _totalEnemies += enemyItem.amount;
            }
        }

        private void SetPools()
        {
            _mummyEnemyPool = CreatePool(mummyPrefab, MummyCapacity);
            _catEnemyPool = CreatePool(catPrefab, CatCapacity);
            _birdEnemyPool = CreatePool(birdPrefab, BirdCapacity);
        }
    
        private ObjectPool<GameObject> CreatePool(GameObject prefab, int capacity)
        {
            return new ObjectPool<GameObject>(
                createFunc: () => _enemyFactory.Create(prefab, enemyParent),
                actionOnGet: ActionOnGet,
                actionOnRelease: OnPutBackInPool,
                defaultCapacity: capacity
            );
        }

        private void ActionOnGet(GameObject obj)
        {
            obj.gameObject.SetActive(true);
            var enemy = obj.GetComponent<Enemy>();
            enemy.OnEnemyVanished += HandleEnemyVanished;
        }

        private void OnPutBackInPool(GameObject obj)
        {
            obj.gameObject.SetActive(false);
            var enemy = obj.GetComponent<Enemy>();
            enemy.OnEnemyVanished -= HandleEnemyVanished;
        }

        private IEnumerator SpawnEnemiesInSequence()
        {
            yield return new WaitForEndOfFrame();

            foreach (var enemy in enemyInventory)
            {
                while (enemy.amount > 0)
                {
                    Vector2Int spawnPosition = GetRandomSpawnPosition();
                    BoardSlot slot = _boardController.BoardSlots[spawnPosition.x, spawnPosition.y];

                    if (slot.TryGetComponent<EnemySpawnIndicator>(out var indicator))
                    {
                        bool isSpawning = true;

                        indicator.SetSprite(enemy.enemyType, () => {
                            if (isSpawning)
                            {
                                StartCoroutine(SpawnEnemyWithParticleEffect(enemy.enemyType, slot));
                                enemy.amount--;
                                isSpawning = false;
                            }
                        });

                        while (isSpawning)
                        {
                            yield return null;
                        }
                    }

                    yield return new WaitForSeconds(SpawnInterval);
                }
            }
        }

        private IEnumerator SpawnEnemyWithParticleEffect(EnemyType enemyType, BoardSlot slot)
        {
            var enemySpawnParticle = _particlePool.GetEnemySpawnParticle();
            enemySpawnParticle.transform.position = slot.transform.position;
            enemySpawnParticle.Play();

            yield return new WaitForSeconds(enemySpawnParticle.main.duration);

            GameObject spawnedEnemy = GetEnemy(enemyType);
            if (spawnedEnemy != null)
            {
                spawnedEnemy.GetComponent<Enemy>().InitializeEnemy(slot.BoardCoordinates);
            }
            DOVirtual.DelayedCall(enemySpawnParticle.main.duration, () =>
            {
                _particlePool.ReleaseEnemySpawnVFX(enemySpawnParticle);
            });
        }

        private Vector2Int GetRandomSpawnPosition()
        {
            if (possibleSpawnPositions.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, possibleSpawnPositions.Count);
                return possibleSpawnPositions[randomIndex];
            }
            return Vector2Int.zero;
        }
 
        private void CalculatePossibleSpawnPositions()
        {
            var levelData = _gameController.LevelData;

            possibleSpawnPositions = new List<Vector2Int>();

            for (int x = 0; x < levelData.gridSize.x; x++)
            {
                int y = levelData.gridSize.y - 1;
                possibleSpawnPositions.Add(new Vector2Int(x, y));
            }
        }
        
        private void HandleEnemyVanished(Enemy enemy)
        {
            _defeatedEnemies++;
            if (_defeatedEnemies >= _totalEnemies)
            {
                AllEnemiesDefeated?.Invoke();
            }
        }

        public GameObject GetEnemy(EnemyType enemyType)
        {
            switch (enemyType)
            {
                case EnemyType.None:
                    return null;
                case EnemyType.Mummy:
                    return _mummyEnemyPool.Get();
                case EnemyType.Bird:
                    return _birdEnemyPool.Get();
                case EnemyType.Cat:
                    return _catEnemyPool.Get();
                default:
                    return null;
            }
        }

        public void ReleaseEnemy(EnemyType enemyType, GameObject item)
        {
            switch (enemyType)
            {
                case EnemyType.None:
                    break;
                case EnemyType.Mummy:
                    _mummyEnemyPool.Release(item);
                    break;
                case EnemyType.Bird:
                    _birdEnemyPool.Release(item);
                    break;
                case EnemyType.Cat:
                    _catEnemyPool.Release(item);
                    break;
            }
        }
    }

    [Serializable]
    public class EnemyInventory
    {
        public EnemyType enemyType;
        public int amount;

        public EnemyInventory(EnemyType _enemyType, int _amount)
        {
            enemyType = _enemyType;
            amount = _amount;
        }
    }
}
