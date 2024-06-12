using Utility;
using Zenject;

namespace Defence
{
    public class DefenceItemFactory : IDefenceItemFactory
    {
        private readonly DiContainer _container;

        public DefenceItemFactory(DiContainer container)
        {
            _container = container;
        }

        public DefenceItem Create(DefenceItem prefab)
        {
            return _container.InstantiatePrefab<DefenceItem>(prefab);
        }
    }
}