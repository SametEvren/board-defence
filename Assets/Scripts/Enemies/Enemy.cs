using System;
using Board;
using UnityEngine;
using Zenject;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour, ISlotOccupier
    {
        [SerializeField] protected EnemyData enemyData;
        protected Vector2Int _currentBoardCoordinates;
        public SlotOccupantType OccupantType => SlotOccupantType.Enemy;
        public event Action OnRemovedFromSlot;

        protected BoardController BoardController;

        [Inject]
        public void Construct(BoardController boardController)
        {
            BoardController = boardController;
        }

        public void MoveToSlot(BoardSlot targetSlot)
        {
            if(targetSlot.CurrentOccupant is { OccupantType: SlotOccupantType.Defence })
                return;
            
            OnRemovedFromSlot?.Invoke();
            //Transform
            targetSlot.OccupySlot(this);
        }

        public abstract void BeginMovement(Transform target);
    }
}