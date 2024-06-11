using System.Collections.Generic;
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
        
        [Inject]
        private IEnemyFactory _enemyFactory;

        public List<GameObject> enemyList;
        
        private void Start()
        {
            SetPools();
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
    }
}