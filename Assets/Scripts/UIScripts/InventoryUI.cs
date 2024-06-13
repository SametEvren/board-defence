using Board;
using System.Collections.Generic;
using ModestTree;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
        
        private void OnValidate()
        {
            Assert.IsNotNull(inventoryItems);
        }
        
        private void OnEnable()
        {
            _boardController.DefenceInventoryUpdated += UpdateDefenceInventoryUI;
        }

        private void OnDisable()
        {
            _boardController.DefenceInventoryUpdated -= UpdateDefenceInventoryUI;
        }

        private void UpdateDefenceInventoryUI(DefenceItemType itemType, int amount)
        {
            var inventoryItem = FindInventoryItem(itemType);
            if (inventoryItem != null)
            {
                inventoryItem.amountText.text = amount.ToString();
                
                if (amount == 0) inventoryItem.button.interactable = false;
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
        public Button button;

        public InventoryItem(DefenceItemType itemType, TextMeshProUGUI amountText, Button button)
        {
            this.itemType = itemType;
            this.amountText = amountText;
            this.button = button;
        }
    }
}



