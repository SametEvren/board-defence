using System;
using System.Linq;
using Board;
using DG.Tweening;
using Player;
using UnityEngine;
using Zenject;

namespace Enemies
{
    [RequireComponent(typeof(Animator))]
    public sealed class EnemyMovement : MonoBehaviour
    {
        public event Action<BoardSlot> OnStoppedByEnemy;
        public event Action OnReachedPlayer;
            
        private BoardController _boardController;
        private PlayerController _playerController;
        
        [Inject]
        public void Construct(BoardController boardController, PlayerController playerController)
        {
            _boardController = boardController;
            _playerController = playerController;
        }

        private Sequence _movementSequence;

        private Vector2Int _currentCoordinates;
        private float _speed;
        private Action<BoardSlot> _onEnterSlot;
        private Animator _animator;
        private BoardSlot _currentTarget;
        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        private static readonly int SpeedOfMovement = Animator.StringToHash("speedOfMovement");

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
                OnReachedPlayer?.Invoke();
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
            _animator.SetBool(IsWalking, false);
            _currentTarget.OnOccupationChanged += HandleObstacleOccupationChanged;
            OnStoppedByEnemy?.Invoke(_currentTarget);
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
            
            _animator.SetBool(IsWalking, true);
            _animator.SetFloat(SpeedOfMovement, _speed);
            
            _movementSequence = DOTween.Sequence()
                .Append(transform.DOMove(_currentTarget.transform.position, movementDuration).SetEase(Ease.Linear))
                .Insert(movementDuration/2f,DOVirtual.DelayedCall(0, () =>
                {
                    _currentCoordinates = _currentTarget.BoardCoordinates;
                    _onEnterSlot?.Invoke(_currentTarget);
                }))
                .OnComplete(MakeNextMove);
        }

        public void KillMovementSequence()
        {
            _movementSequence.Kill(false);
        }
    }
}