using System;
using UnityEngine;
using UnityEngine.Assertions;
using Utility;

namespace Board
{
    public class BoardController : MonoBehaviour
    {
        public LevelData levelData;
        [SerializeField] private BoardSlot slotPrefab;
        [SerializeField] private SerializedDictionary<DefenceItemType, int> defenceInventory;
        
        public BoardSlot[,] BoardSlots { get; private set; }
        
        public event Action<DefenceItemType, int> InventoryUpdated;

        private void OnValidate()
        {
            Assert.IsNotNull(levelData);
            Assert.IsNotNull(slotPrefab);
        }

        private void Start()
        {
            SpawnLevel();
            SetInventory();
        }

        private void SpawnLevel()
        {
            GameObject parent = new GameObject("Level Stones");

            var offsetX = (levelData.gridSize.x - 1) / 2f;
            var offsetY = (levelData.gridSize.y - 1) / 2f;

            BoardSlots = new BoardSlot[levelData.gridSize.x, levelData.gridSize.y];

            for (var x = 0; x < levelData.gridSize.x; x++)
            for (var y = 0; y < levelData.gridSize.y; y++)
            {
                var position = new Vector3(x - offsetX, 0, y - offsetY);
                var slot = Instantiate(slotPrefab, position, Quaternion.identity, parent.transform);
                
                BoardSlots[x, y] = slot;

                var isPlaceable = x < levelData.buildableArea.x && y < levelData.buildableArea.y;
                
                slot.InitializeSlot(new Vector2Int(x,y), isPlaceable);
            }
        }
        
        private void SetInventory()
        {
            defenceInventory.Clear();
            foreach (var defenceItem in levelData.defenceItemInventories)
            {
                defenceInventory.Add(defenceItem.defenceItemType, defenceItem.amount);
                InventoryUpdated?.Invoke(defenceItem.defenceItemType, defenceItem.amount);
            }
        }

        public void UpdateInventory(DefenceItemType itemType)
        {
            var newValue = defenceInventory[itemType] - 1;
            defenceInventory[itemType] = newValue;
            InventoryUpdated?.Invoke(itemType, newValue);
        }

        public bool CheckAvailable(DefenceItemType itemType)
        {
            return defenceInventory[itemType] > 0;
        }
    }
}