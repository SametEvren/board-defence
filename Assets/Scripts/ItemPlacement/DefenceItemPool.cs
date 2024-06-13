using System;
using Board;
using Defence;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;
using Zenject;

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

        [Inject]
        private IDefenceItemFactory _defenceItemFactory;

        private void OnValidate()
        {
            Assert.IsNotNull(bastetPrefab);
            Assert.IsNotNull(anubisPrefab);
            Assert.IsNotNull(pharaohPrefab);
        }

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
                createFunc: () => _defenceItemFactory.Create(prefab),
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

        public void ReleaseDefenceItem(DefenceItemType defenceItemType, DefenceItem item)
        {
            switch (defenceItemType)
            {
                case DefenceItemType.Bastet:
                    _bastetPool.Release(item);
                    break;
                case DefenceItemType.Anubis:
                    _anubisPool.Release(item);
                    break;
                case DefenceItemType.Pharaoh:
                    _pharaohPool.Release(item);
                    break;
                default:
                    break;
            }
        }
    }
}