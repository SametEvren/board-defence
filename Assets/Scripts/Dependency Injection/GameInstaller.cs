using Board;
using Zenject;

namespace Dependency_Injection
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BoardController>().FromComponentInHierarchy().AsSingle().NonLazy();
        }
    }
}
