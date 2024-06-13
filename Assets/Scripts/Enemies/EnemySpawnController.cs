using System;
using System.Collections.Generic;
using Board;
using UnityEngine;
using UnityEngine.Pool;
using Utility;
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
        }

        private void SetInventory()
        {
            enemyInventory.Clear();
            foreach (var enemyItem in _boardController.levelData.enemyInventories)
            {
                enemyInventory.Add(new EnemyInventory(enemyItem.enemyType, enemyItem.amount));
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
        }

        private void OnPutBackInPool(GameObject obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                var enemy = _mummyEnemyPool.Get();
                enemy.GetComponent<Enemy>().InitializeEnemy(new Vector2Int(3,7));
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                var enemy = _catEnemyPool.Get();
                enemy.GetComponent<Enemy>().InitializeEnemy(new Vector2Int(2,7));
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                var enemy = _birdEnemyPool.Get();
                enemy.GetComponent<Enemy>().InitializeEnemy(new Vector2Int(1,7));
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