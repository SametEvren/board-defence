using Board;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UIScripts
{
    public class EnemyUI : MonoBehaviour
    {
        public GameObject enemySpawnIndicator;
        public Canvas worldSpaceCanvas;
        
        public Sprite mummySprite;
        public Sprite catSprite;
        public Sprite birdSprite;
        
        private BoardController _boardController;
        [Inject]
        public void Construct(BoardController boardController)
        {
            _boardController = boardController;
        }

        private void OnEnable()
        {
            _boardController.LastBlocksSet += OnLastBlocksSet;
        }

        private void OnDisable()
        {
            _boardController.LastBlocksSet -= OnLastBlocksSet;
        }

        private void OnLastBlocksSet(BoardSlot slot)
        {
            var indicator = Instantiate(enemySpawnIndicator, worldSpaceCanvas.transform);
            indicator.transform.position = slot.transform.position;

            var enemySpawnIndicatorComponent = slot.gameObject.AddComponent<EnemySpawnIndicator>();
            enemySpawnIndicatorComponent.SetSpawnIndicator(indicator.GetComponent<Image>(), mummySprite, catSprite, birdSprite);
            
            if (indicator.GetComponent<UISetTarget>() == null)
            {
                indicator.AddComponent<UISetTarget>().target = slot.transform;
            }

            indicator.GetComponent<UISetTarget>().offset = new Vector3(0, 0.1f, 0);
        }
    }
}