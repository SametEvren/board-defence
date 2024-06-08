using System;
using UnityEngine;
using Utility;

namespace Board
{
    public class BoardController : MonoBehaviour
    {
        public LevelData levelData;
        [SerializeField] private GameObject stonePrefab;
        [SerializeField] private GameObject buildableStonePrefab;
        [SerializeField] private SerializedDictionary<DefenceItemType, int> defenceInventory;
        
        public event Action<DefenceItemType, int> InventoryUpdated;
        
        private void Start()
        {
            if (levelData == null || stonePrefab == null || buildableStonePrefab == null)
            {
                Debug.LogError("LevelData or Prefabs are not assigned.");
                return;
            }
            
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
                    GameObject prefab = (x < levelData.buildableArea.x && y < levelData.buildableArea.y) ? buildableStonePrefab : stonePrefab;
                    Instantiate(prefab, position, Quaternion.identity, parent.transform);
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