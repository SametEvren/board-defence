using Board;
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
        [Inject]
        public void Construct(BoardController boardController)
        {
            _boardController = boardController;
        }
        
        private void HandleButtonClicked()
        {
            if(_boardController.CheckAvailable(itemType))
                itemPlacementController.StartPlacing(itemType);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            HandleButtonClicked();
        }
    }
}