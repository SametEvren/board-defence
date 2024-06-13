using System;
using System.Collections.Generic;
using Board;
using Enemies;
using ItemPlacement;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Defence
{
    public abstract class DefenceItem : MonoBehaviour, ISlotOccupier
    {
        [SerializeField] protected DefenceItemData defenceItemData;
        [SerializeField] protected DefenceItemType itemType;
        
        private readonly List<BoardSlot> _affectedSlots = new ();

        public SlotOccupantType OccupantType => SlotOccupantType.Defence;
        public event Action<ISlotOccupier> OnRemovedFromSlot;
        public DefenceItemData Data => defenceItemData;
        public DefenceItemType ItemType => itemType;

        protected DefenceItemAttack Attack;

        private float _currentHealth;

        private DefenceItemPool _defenceItemPool;
        
        [Inject]
        private void Construct(DefenceItemPool defenceItemPool)
        {
            _defenceItemPool = defenceItemPool;
        }
        
        private void OnValidate()
        {
            Assert.IsNotNull(defenceItemData);
            Assert.IsTrue(itemType != DefenceItemType.None);
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

        public void Initialize()
        {
            _currentHealth = Data.health;
            Attack.Initialize(Data, _affectedSlots);
        }

        public void GetEnemiesInRange()
        {
            foreach (var boardSlot in _affectedSlots)
            {
                foreach (var currentOcuppant in boardSlot.CurrentOccupants)
                {
                    if (currentOcuppant.OccupantType == SlotOccupantType.Enemy)
                    {
                        Attack.AddEnemyAsTarget((Enemy)currentOcuppant);
                    }
                }
                
            }
        }

        public void SetAffectedSlots(List<BoardSlot> newSlots)
        {
            if (_affectedSlots != null && _affectedSlots.Count > 0)
                StopListeningToAffectedSlots();

            foreach (var slot in newSlots)
            {
                _affectedSlots.Add(slot);
                slot.OnOccupationChanged += HandleChangeInArea;
            }
            GetEnemiesInRange();
        }

        private void StopListeningToAffectedSlots()
        {
            if (_affectedSlots == null) return;

            foreach (var slot in _affectedSlots)
                slot.OnOccupationChanged -= HandleChangeInArea;

            _affectedSlots.Clear();
        }

        protected virtual void HandleChangeInArea(ISlotOccupier occupier, bool added)
        {
            if (occupier is not { OccupantType: SlotOccupantType.Enemy }) return;

            if (added)
                Attack.AddEnemyAsTarget((Enemy)occupier);
            else
                Attack.RemoveEnemyFromTargets((Enemy)occupier);
        }

        private void OnDefeat()
        {
            StopListeningToAffectedSlots();
            Attack.ClearEnemiesInRange(); 
            _defenceItemPool.ReleaseDefenceItem(itemType, this);
            OnRemovedFromSlot?.Invoke(this);
        }
    }
}
