using UnityEngine;
using Zenject;

namespace Utility
{
    public static class Extensions
    {
        public static TComponent InstantiatePrefab<TComponent>(this DiContainer container, TComponent prefab)
            where TComponent : MonoBehaviour
        {
            var obj = container.InstantiatePrefab(prefab);
            return obj.GetComponent<TComponent>();
        }
    }
}