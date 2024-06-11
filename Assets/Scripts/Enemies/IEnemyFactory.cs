using UnityEngine;

namespace Enemies
{
    public interface IEnemyFactory
    {
        GameObject Create(GameObject prefab, Transform parent);
    }
}