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

            for (int x = 0; x < levelData.gridSize.x; x++)
            {
                for (int y = 0; y < levelData.gridSize.y; y++)
                {
                    var position = new Vector3(x - offsetX, 0, y - offsetY);
                    var slot = Instantiate(slotPrefab, position, Quaternion.identity, parent.transform);
                    if (x < levelData.buildableArea.x && y < levelData.buildableArea.y)
                        slot.RenderAsPlaceable();
                }
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
    }
}