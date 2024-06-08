using Board;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace UIScripts
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private List<InventoryItem> inventoryItems;
        private BoardController _boardController;

        [Inject]
        public void Construct(BoardController boardController)
        {
            _boardController = boardController;
        }
    
        private void OnEnable()
        {
            _boardController.InventoryUpdated += UpdateInventoryUI;
        }

        private void OnDisable()
        {
            _boardController.InventoryUpdated -= UpdateInventoryUI;
        }

        private void UpdateInventoryUI(DefenceItemType itemType, int amount)
        {
            var inventoryItem = FindInventoryItem(itemType);
            if (inventoryItem != null)
            {
                inventoryItem.amountText.text = amount.ToString();
            }
        }

        private InventoryItem FindInventoryItem(DefenceItemType itemType)
        {
            return inventoryItems.Find(item => item.itemType == itemType);
        }
    }
    
    [System.Serializable]
    public class InventoryItem
    {
        public DefenceItemType itemType;
        public TextMeshProUGUI amountText;

        public InventoryItem(DefenceItemType itemType, TextMeshProUGUI amountText)
        {
            this.itemType = itemType;
            this.amountText = amountText;
        }
    }
}



