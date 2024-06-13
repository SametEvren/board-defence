using System;
using Game;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Board
{
    public class BoardController : MonoBehaviour
    {
        public event Action<BoardSlot> LastBlocksSet;

        [SerializeField] private BoardSlot slotPrefab;
        
        public BoardSlot[,] BoardSlots { get; private set; }
        private GameController _gameController;
        
        [Inject]
        public void Construct(GameController gameController)
        {
            _gameController = gameController;
        }

        private void OnValidate()
        {
            Assert.IsNotNull(slotPrefab);
        }

        private void Start()
        {
            SpawnBoard();
        }

        private void SpawnBoard()
        {
            var levelData = _gameController.LevelData;
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
                
                slot.InitializeSlot(new Vector2Int(x, y), isPlaceable);

                if (y == levelData.gridSize.y - 1)
                {
                    LastBlocksSet?.Invoke(slot);
                }
            }
        }
    }
}
