using System;
using System.Collections.Generic;
using UnityEngine;

namespace Board
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "LevelData/Level", order = 0)]
    public class LevelData : ScriptableObject
    {
        public Vector2Int gridSize;
        public Vector2Int buildableArea;
        public List<DefenceItemInventory> defenceItemInventories;
        public List<EnemyInventory> enemyInventories;
    }
    
    [Serializable]
    public struct EnemyInventory
    {
        public EnemyType enemyType;
        public int amount;
    }

    [Serializable]
    public struct DefenceItemInventory : IEquatable<DefenceItemInventory>
    {
        public DefenceItemType defenceItemType;
        public int amount;

        public bool Equals(DefenceItemInventory other)
        {
            return this.defenceItemType == other.defenceItemType && this.amount == other.amount;
        }

        public override bool Equals(object obj)
        {
            if (obj is DefenceItemInventory)
            {
                return Equals((DefenceItemInventory)obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + defenceItemType.GetHashCode();
                hash = hash * 23 + amount.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(DefenceItemInventory left, DefenceItemInventory right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DefenceItemInventory left, DefenceItemInventory right)
        {
            return !(left == right);
        }
    }


    public enum EnemyType
    {
        None = 0,
        Mummy = 1,
        Bird = 2,
        Cat = 3
    }

    public enum DefenceItemType
    {
        None = 0,
        Bastet = 1,
        Anubis = 2,
        Pharaoh = 3
    }
}
