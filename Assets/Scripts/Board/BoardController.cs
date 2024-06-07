using UnityEngine;

namespace Board
{
    public class BoardController : MonoBehaviour
    {
        [SerializeField] private LevelData levelData;
        [SerializeField] private GameObject stonePrefab;
        [SerializeField] private GameObject buildableStonePrefab;

        private void Start()
        {
            SpawnLevel();
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

                    if (x < levelData.buildableArea.x && y < levelData.buildableArea.y)
                    {
                        Instantiate(buildableStonePrefab, position, Quaternion.identity, parent.transform);
                    }
                    else
                    {
                        Instantiate(stonePrefab, position, Quaternion.identity, parent.transform);
                    }
                }
            }
        }
    }
}
