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
        private BoardSlot _currentTarget;

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

            if (nextCoordinates.y >= gridSize.y || nextCoordinates.y < 0)
            {
                //TODO: HandleExitGrid();
                return;
            }

            _currentTarget = _boardController.BoardSlots[nextCoordinates.x, nextCoordinates.y];
            
            MoveToSlot(_currentTarget);
        }

        private void MoveToSlot(BoardSlot targetSlot)
        {
            var currentOccupants = targetSlot.CurrentOccupants;
            
            if (currentOccupants != null && 
                currentOccupants.Any(o => o.OccupantType == SlotOccupantType.Defence))
            {
                HandleObstacleReached();
                return;
            }
            
            BeginMovement();
        }

        private void HandleObstacleReached()
        {
            _animator.SetBool("isWalking", false);
            _currentTarget.OnOccupationChanged += HandleObstacleOccupationChanged;
        }

        private void HandleObstacleOccupationChanged(ISlotOccupier occupier, bool isOccupied)
        {
            if (isOccupied) return;

            if (_currentTarget.CurrentOccupants != null &&
                _currentTarget.CurrentOccupants.Any(o => o.OccupantType == SlotOccupantType.Defence)) return;

            _currentTarget.OnOccupationChanged -= HandleObstacleOccupationChanged;
            BeginMovement();
        }

        private void BeginMovement()
        {
            var movementDuration = 1f / _speed;
            
            _animator.SetBool("isWalking", true);
            _animator.SetFloat("walkSpeed", _speed);
            
            _movementSequence = DOTween.Sequence()
                .Append(transform.DOMove(_currentTarget.transform.position, movementDuration).SetEase(Ease.Linear))
                .Insert(movementDuration/2f,DOVirtual.DelayedCall(0, () =>
                {
                    _currentCoordinates = _currentTarget.BoardCoordinates;
                    _onEnterSlot?.Invoke(_currentTarget);
                }))
                .OnComplete(MakeNextMove);
        }
    }
}