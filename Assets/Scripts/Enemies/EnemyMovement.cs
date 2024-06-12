using System;
using System.Linq;
using Board;
using DG.Tweening;
using UnityEditor.Animations;
using UnityEngine;
using Zenject;

namespace Enemies
{
    public sealed class EnemyMovement : MonoBehaviour
    {
        private BoardController _boardController;
        [Inject]
        public void Construct(BoardController boardController)
        {
            _boardController = boardController;
        }

        private Sequence _movementSequence;

        private Vector2Int _currentCoordinates;
        private float _speed;
        private Action<BoardSlot> _onEnterSlot;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        internal void Initialize(Vector2Int currentBoardCoordinates, float movementSpeed, Action<BoardSlot> onEnterSlot)
        {
            _movementSequence?.Kill();
            _movementSequence = null;

            _currentCoordinates = currentBoardCoordinates;
            _speed = movementSpeed;
            _onEnterSlot = onEnterSlot;
            MakeNextMove();
        }

        private void MakeNextMove()
        {
            var gridSize = new Vector2Int(
                _boardController.BoardSlots.GetLength(0),
                _boardController.BoardSlots.GetLength(1));
            var nextCoordinates = new Vector2Int(
                _currentCoordinates.x,
                _currentCoordinates.y - 1
            );

            if (nextCoordinates.y >= gridSize.y || nextCoordinates.y < 0) return;

            var nextSlot = _boardController.BoardSlots[nextCoordinates.x, nextCoordinates.y];
            
            MoveToSlot(nextSlot);
        }

        private void MoveToSlot(BoardSlot targetSlot)
        {
            var currentOccupants = targetSlot.CurrentOccupants;
            if (currentOccupants != null && 
                currentOccupants.Any(o => o.OccupantType == SlotOccupantType.Defence))
                return;
            
            BeginMovement(targetSlot);
        }

        private void BeginMovement(BoardSlot targetSlot)
        {
            var movementDuration = 1f / _speed;
            
            _animator.SetBool("isWalking", true);
            _animator.SetFloat("walkSpeed", _speed);
            
            _movementSequence = DOTween.Sequence()
                .Append(transform.DOMove(targetSlot.transform.position, movementDuration).SetEase(Ease.Linear))
                .Insert(movementDuration/2f,DOVirtual.DelayedCall(0, () =>
                {
                    _currentCoordinates = targetSlot.BoardCoordinates;
                    _onEnterSlot?.Invoke(targetSlot);
                }))
                .OnComplete(MakeNextMove);
        }
    }
}