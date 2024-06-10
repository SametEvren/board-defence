using Board;
using UnityEngine;

namespace Defence
{
    [CreateAssetMenu(fileName = "Defence Item Data", menuName = "ScriptableObjects/Defence Item", order = 0)]
    public class DefenceItemData : ScriptableObject
    {
        public float damage;
        public int range;
        public float interval;
        public AttackPattern attackPattern;
    }

    public enum AttackPattern
    {
        Forward,
        Backward,
        Left,
        Right,
        Plus,
        Diagonal,
        All
    }
}