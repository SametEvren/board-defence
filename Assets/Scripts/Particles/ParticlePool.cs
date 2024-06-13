using System;
using Board;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

namespace Particles
{
    public class ParticlePool : MonoBehaviour
    {
        private ObjectPool<ParticleSystem> _enemyDisappearPool;
        [SerializeField] private ParticleSystem enemyDisappearParticle;
        
        private ObjectPool<ParticleSystem> _anubisAttackPool;
        [SerializeField] private ParticleSystem anubisAttackParticle;
        
        private ObjectPool<ParticleSystem> _bastetAttackPool;
        [SerializeField] private ParticleSystem bastetAttackParticle;
        
        private ObjectPool<ParticleSystem> _enemySpawnPool;
        [SerializeField] private ParticleSystem enemySpawnParticle;

        private void Start()
        {
            SetPools();
        }

        private void SetPools()
        {
            _enemyDisappearPool = CreatePool(enemyDisappearParticle, 50);
            _anubisAttackPool = CreatePool(anubisAttackParticle, 30);
            _bastetAttackPool = CreatePool(bastetAttackParticle, 21);
            _enemySpawnPool = CreatePool(enemySpawnParticle, 5);
        }

        #region Pool Methods
        private ObjectPool<ParticleSystem> CreatePool(ParticleSystem prefab, int capacity)
        {
            return new ObjectPool<ParticleSystem>(
                createFunc: () => Instantiate(prefab),
                actionOnGet: ActionOnGet,
                actionOnRelease: OnPutBackInPool,
                defaultCapacity: capacity
            );
        }

        private void ActionOnGet(ParticleSystem obj)
        {
            obj.gameObject.SetActive(true);
        }

        private void OnPutBackInPool(ParticleSystem obj)
        {
            obj.gameObject.SetActive(false);
        }
        #endregion
        
        public void SpawnDisappearParticle(Vector3 position)
        {
            var enemyDisappearParticle = _enemyDisappearPool.Get();
            enemyDisappearParticle.transform.position = position;
            enemyDisappearParticle.transform.parent = transform;
            enemyDisappearParticle.Play();

            DOVirtual.DelayedCall(1f, () =>
            {
                _enemyDisappearPool.Release(enemyDisappearParticle);
            });
        }

        public ParticleSystem GetAttackVFX(DefenceItemType itemType)
        {
            switch (itemType)
            {
                case DefenceItemType.Bastet:
                    return _bastetAttackPool.Get();
                case DefenceItemType.Anubis:
                    return _anubisAttackPool.Get();
                default:
                    throw new ArgumentOutOfRangeException(nameof(itemType), itemType, null);
            }
        }

        public void ReleaseAttackVFX(DefenceItemType itemType, ParticleSystem itemToRelease)
        {
            switch (itemType)
            {
                case DefenceItemType.Bastet:
                    _bastetAttackPool.Release(itemToRelease);
                    break;
                case DefenceItemType.Anubis:
                    _anubisAttackPool.Release(itemToRelease);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(itemType), itemType, null);
            }
        }

        public ParticleSystem GetEnemySpawnParticle()
        {
            return _enemySpawnPool.Get();
        }

        public void ReleaseEnemySpawnVFX(ParticleSystem _enemySpawnParticle)
        {
            _enemySpawnPool.Release(_enemySpawnParticle);
        }
    }
}