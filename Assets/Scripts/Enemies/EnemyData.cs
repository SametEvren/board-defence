using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "Enemy Data", menuName = "ScriptableObjects/Enemy", order = 1)]
    public class EnemyData : ScriptableObject
    {
        public float health;
        public float speed;
    }
}