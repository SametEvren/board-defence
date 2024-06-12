using System;
using System.Collections.Generic;
using Board;
using Enemies;
using ItemPlacement;
using UnityEngine;
using Zenject;

namespace Defence
{
    public abstract class DefenceItem : MonoBehaviour, ISlotOccupier
    {
        [SerializeField] protected DefenceItemData defenceItemData;
        [SerializeField] protected DefenceItemType itemType;
        [SerializeField] protected List<BoardSlot> affectedSlots;
        [SerializeField] protected List<Enemy> affectedEnemies;

        public SlotOccupantType OccupantType => SlotOccupantType.Defence;
        public event Action<ISlotOccupier> OnRemovedFromSlot;
        public DefenceItemData Data => defenceItemData;
        public DefenceItemType ItemType => itemType;

        private float _currentHealth;

        private DefenceItemPool _defenceItemPool;

        [Inject]
        private void Construct(DefenceItemPool defenceItemPool)
        {
            _defenceItemPool = defenceItemPool;
        }

        public void TakeDamage(float amount)
        {
            _currentHealth -= amount;
            //TODO: Particles etc.
            
            if (_currentHealth <= 0)
            {
                OnDefeat();
            }
        }

        public virtual void Initialize()
        {
            _currentHealth = Data.health;
        }

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

        protected virtual void HandleChangeInArea(ISlotOccupier occupier, bool added)
        {
            if(occupier == null || occupier.OccupantType == SlotOccupantType.Defence)
                return;
        }

        private void OnDefeat()
        {
            _defenceItemPool.ReleaseDefenceItem(itemType, this);
            OnRemovedFromSlot?.Invoke(this);
        }
    }
}