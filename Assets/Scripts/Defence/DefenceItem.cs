using Board;
using UnityEngine;

namespace Defence
{
    public abstract class DefenceItem : MonoBehaviour, ISlotOccupier
    {
        [SerializeField] protected DefenceItemData defenceItemData;
        [SerializeField] protected DefenceItemType itemType;


        public SlotOccupantType OccupantType => SlotOccupantType.Defence;
        public DefenceItemData Data => defenceItemData;

        public DefenceItemType ItemType => itemType;
    }
}