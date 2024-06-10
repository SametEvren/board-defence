using Board;
using ItemPlacement;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIScripts
{
    [RequireComponent(typeof(EventTrigger))]
    public class ItemPlacementButton : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private DefenceItemType itemType;
        [SerializeField] private ItemPlacementController itemPlacementController;
        
        private void HandleButtonClicked()
        {
            itemPlacementController.StartPlacing(itemType);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            HandleButtonClicked();
        }
    }
}