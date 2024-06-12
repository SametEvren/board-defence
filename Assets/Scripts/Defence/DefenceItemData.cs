using Board;
using UnityEngine;

namespace Defence
{
    [CreateAssetMenu(fileName = "Defence Item Data", menuName = "ScriptableObjects/Defence Item", order = 0)]
    public class DefenceItemData : ScriptableObject
    {
        public float damage;
        public float health;
        public int range;
        public float interval;
        public AttackPattern attackPattern;
    }

    public enum AttackPattern
    {
        Forward,
        Backward,
        Right,
        Left,
        Plus,
        Diagonal,
        All
    }
}