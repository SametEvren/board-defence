using Board;
using Game;
using ItemPlacement;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UIScripts
{
    [RequireComponent(typeof(EventTrigger))]
    public class ItemPlacementButton : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private DefenceItemType itemType;
        [SerializeField] private ItemPlacementController itemPlacementController;
        
        private BoardController _boardController;
        private GameStateController _gameStateController;
        
        [Inject]
        public void Construct(BoardController boardController, GameStateController gameStateController)
        {
            _boardController = boardController;
            _gameStateController = gameStateController;
        }
        
        private void HandleButtonClicked()
        {
            if(_boardController.CheckAvailable(itemType) && _gameStateController.CurrentState == GameState.Playing)
                itemPlacementController.StartPlacing(itemType);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            HandleButtonClicked();
        }
    }
}