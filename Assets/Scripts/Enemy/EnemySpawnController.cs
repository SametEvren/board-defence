using UnityEngine;
using UnityEngine.Pool;

namespace Enemy
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
            return new ObjectPool<GameObject>(() => Instantiate(prefab, enemyParent), ActionOnGet, OnPutBackInPool, defaultCapacity: capacity);
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
