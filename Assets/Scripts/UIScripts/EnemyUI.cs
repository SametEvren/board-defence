using Board;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Zenject;

namespace UIScripts
{
    public class EnemyUI : MonoBehaviour
    {
        [SerializeField] private GameObject enemySpawnIndicator;
        [SerializeField] private Canvas worldSpaceCanvas;
        
        [SerializeField] private Sprite mummySprite;
        [SerializeField] private Sprite catSprite;
        [SerializeField] private Sprite birdSprite;
        
        private BoardController _boardController;
        [Inject]
        public void Construct(BoardController boardController)
        {
            _boardController = boardController;
        }
        
        private void OnValidate()
        {
            Assert.IsNotNull(enemySpawnIndicator);
            Assert.IsNotNull(worldSpaceCanvas);
            Assert.IsNotNull(mummySprite);
            Assert.IsNotNull(catSprite);
            Assert.IsNotNull(birdSprite);
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