using Board;
using Enemies;
using ItemPlacement;
using Zenject;

namespace Dependency_Injection
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IEnemyFactory>().To<EnemyFactory>().AsSingle();
            Container.Bind<BoardController>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<DefenceItemPool>().FromComponentInHierarchy().AsSingle().NonLazy();
        }
    }
}
