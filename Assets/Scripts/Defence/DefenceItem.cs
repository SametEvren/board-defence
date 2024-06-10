using System;
using System.Collections.Generic;
using Board;
using UnityEngine;

namespace Defence
{
    public abstract class DefenceItem : MonoBehaviour, ISlotOccupier
    {
        [SerializeField] protected DefenceItemData defenceItemData;
        [SerializeField] protected DefenceItemType itemType;
        [SerializeField] protected List<BoardSlot> affectedSlots;

        public SlotOccupantType OccupantType => SlotOccupantType.Defence;
        public event Action OnRemovedFromSlot;
        public DefenceItemData Data => defenceItemData;
        public DefenceItemType ItemType => itemType;

        public void SetAffectedSlots(List<BoardSlot> newSlots)
        {
            if (affectedSlots != null && affectedSlots.Count > 0) 
                StopListeningToAffectedSlots();

            foreach (var slot in newSlots)
            {
                affectedSlots.Add(slot);
                slot.OnOccupationChanged += HandleChangeInArea;
            }
        }

        private void StopListeningToAffectedSlots()
        {
            foreach (var slot in affectedSlots) 
                slot.OnOccupationChanged -= HandleChangeInArea;
        }

        protected virtual void HandleChangeInArea(ISlotOccupier occupier)
        {
            if(occupier == null || occupier.OccupantType == SlotOccupantType.Defence)
                return;
        }

        private void OnDestroy()
        {
            OnRemovedFromSlot?.Invoke();
        }
    }
}