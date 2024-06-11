using Board;
using Defence;
using UnityEngine;
using UnityEngine.Pool;

namespace ItemPlacement
{
    public class DefenceItemPool : MonoBehaviour
    {
        private ObjectPool<DefenceItem> _bastetPool;
        [SerializeField] private DefenceItem bastetPrefab;
        private const int BastetCapacity = 10;
        
        private ObjectPool<DefenceItem> _anubisPool;
        [SerializeField] private DefenceItem anubisPrefab;
        private const int AnubisCapacity = 10;
        
        private ObjectPool<DefenceItem> _pharaohPool;
        [SerializeField] private DefenceItem pharaohPrefab;
        private const int PharaohCapacity = 10;
        
        private void Start()
        {
            SetPools();
        }

        private void SetPools()
        {
            _bastetPool = CreatePool(bastetPrefab, BastetCapacity);
            _anubisPool = CreatePool(anubisPrefab, AnubisCapacity);
            _pharaohPool = CreatePool(pharaohPrefab, PharaohCapacity);
        }
    
        private ObjectPool<DefenceItem> CreatePool(DefenceItem prefab, int capacity)
        {
            return new ObjectPool<DefenceItem>(
                createFunc: () => Instantiate(prefab),
                actionOnGet: ActionOnGet,
                actionOnRelease: OnPutBackInPool,
                defaultCapacity: capacity
            );
        }

        private void ActionOnGet(DefenceItem obj)
        {
            obj.gameObject.SetActive(true);
        }

        private void OnPutBackInPool(DefenceItem obj)
        {
            obj.gameObject.SetActive(false);
        }
        
        public DefenceItem GetDefenceItem(DefenceItemType defenceItemType)
        {
            switch (defenceItemType)
            {
                case DefenceItemType.Bastet:
                    return _bastetPool.Get();
                case DefenceItemType.Anubis:
                    return _anubisPool.Get();
                case DefenceItemType.Pharaoh:
                    return _pharaohPool.Get();
                default:
                    return null;
            }
        }
    }
}