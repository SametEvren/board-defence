using Board;
using DG.Tweening;
using UnityEngine;

namespace Enemies
{
    public class BirdEnemy : Enemy
    {
        protected override void BeginMovement(BoardSlot targetSlot)
        {
            var movementDuration =
                Vector3.Distance(targetSlot.transform.position, transform.position) / enemyData.speed;
            
            MovementSequence = DOTween.Sequence()
                .Append(transform.DOMove(targetSlot.transform.position, movementDuration))
                .Insert(movementDuration/2f,DOVirtual.DelayedCall(0, () =>
                {
                    TriggerSlotRemoveAction();
                    targetSlot.OccupySlot(this);
                    _currentBoardCoordinates = targetSlot.BoardCoordinates;
                }))
                .OnComplete(MakeNextMove);
        }
    }
}