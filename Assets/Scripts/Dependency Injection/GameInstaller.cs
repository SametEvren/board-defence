using Audio;
using Board;
using Defence;
using Enemies;
using Game;
using ItemPlacement;
using Particles;
using Player;
using UIScripts;
using Zenject;

namespace Dependency_Injection
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindFactories();
            BindControllers();
            BindPools();
            BindUI();
        }

        private void BindFactories()
        {
            Container.Bind<IEnemyFactory>().To<EnemyFactory>().AsSingle();
            Container.Bind<IDefenceItemFactory>().To<DefenceItemFactory>().AsSingle();
        }

        private void BindControllers()
        {
            Container.Bind<BoardController>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<EnemySpawnController>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<GameStateController>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<ItemPlacementController>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<GameController>().FromComponentInHierarchy().AsSingle().NonLazy();
        }

        private void BindPools()
        {
            Container.Bind<DefenceItemPool>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<ParticlePool>().FromComponentInHierarchy().AsSingle().NonLazy();
        }

        private void BindUI()
        {
            Container.Bind<DamageUI>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<AudioController>().FromComponentInHierarchy().AsSingle().NonLazy();
        }
    }
}