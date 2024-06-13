using System;
using Board;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class EnemySpawnIndicator : MonoBehaviour
    {
        [SerializeField] private Image _enemySpawnIndicator;
        public Sprite mummySprite;
        public Sprite catSprite;
        public Sprite birdSprite;

        private Action _onComplete;

        public void SetSpawnIndicator(Image indicator, Sprite _mummySprite, Sprite _catSprite, Sprite _birdSprite)
        {
            _enemySpawnIndicator = indicator;
            mummySprite = _mummySprite;
            catSprite = _catSprite;
            birdSprite = _birdSprite;
        }

        public void SetSprite(EnemyType enemyType, Action onComplete)
        {
            _onComplete = onComplete;

            switch (enemyType)
            {
                case EnemyType.Mummy:
                    _enemySpawnIndicator.sprite = mummySprite;
                    break;
                case EnemyType.Cat:
                    _enemySpawnIndicator.sprite = catSprite;
                    break;
                case EnemyType.Bird:
                    _enemySpawnIndicator.sprite = birdSprite;
                    break;
            }

            _enemySpawnIndicator.fillAmount = 0;
            _enemySpawnIndicator.DOFillAmount(1, 5f).OnComplete(OnFillComplete);
        }

        private void OnFillComplete()
        {
            _onComplete?.Invoke();
            _enemySpawnIndicator.fillAmount = 0;
        }
    }
}