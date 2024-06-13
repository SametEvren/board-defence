using System;
using System.Collections;
using System.Collections.Generic;
using Board;
using UIScripts;
using UnityEngine;
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
        [SerializeField] private ParticleSystem enemySpawnParticle;

        public event Action AllEnemiesDefeated;
        
        private const float SpawnInterval = 5f;
        private int _totalEnemies;
        private int _defeatedEnemies;

        public event Action<EnemyType, int> EnemyInventoryUpdated;
        
        [Inject]
        private IEnemyFactory _enemyFactory;
        
        private BoardController _boardController;
        
        [Inject]
        public void Construct(BoardController boardController)
        {
            _boardController = boardController;
        }
        
        private void Start()
        {
            SetInventory();
            SetPools();
            StartCoroutine(SpawnEnemiesInSequence());
        }

        private void SetInventory()
        {
            enemyInventory.Clear();
            foreach (var enemyItem in _boardController.levelData.enemyInventories)
            {
                enemyInventory.Add(new EnemyInventory(enemyItem.enemyType, enemyItem.amount));
                _totalEnemies += enemyItem.amount;
                EnemyInventoryUpdated?.Invoke(enemyItem.enemyType, enemyItem.amount);
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
                                EnemyInventoryUpdated?.Invoke(enemy.enemyType, enemy.amount);
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
            var particleInstance = Instantiate(enemySpawnParticle, slot.transform.position, Quaternion.identity);
            particleInstance.Play();

            yield return new WaitForSeconds(particleInstance.main.duration);

            GameObject spawnedEnemy = GetEnemy(enemyType);
            if (spawnedEnemy != null)
            {
                spawnedEnemy.GetComponent<Enemy>().InitializeEnemy(slot.BoardCoordinates);
            }

            Destroy(particleInstance.gameObject, particleInstance.main.duration);
        }

        private Vector2Int GetRandomSpawnPosition()
        {
            if (_boardController.possibleSpawnPositions.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, _boardController.possibleSpawnPositions.Count);
                return _boardController.possibleSpawnPositions[randomIndex];
            }
            return Vector2Int.zero;
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
