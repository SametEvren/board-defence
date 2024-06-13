using Board;
using Defence;
using Enemies;
using Game;
using ItemPlacement;
using Player;
using UIScripts;
using Zenject;

namespace Dependency_Injection
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IEnemyFactory>().To<EnemyFactory>().AsSingle();
            Container.Bind<IDefenceItemFactory>().To<DefenceItemFactory>().AsSingle();
            Container.Bind<BoardController>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<DefenceItemPool>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<EnemySpawnController>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<PlayerController>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<DamageUI>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<GameStateController>().FromComponentInHierarchy().AsSingle().NonLazy();
        }
    }
}
