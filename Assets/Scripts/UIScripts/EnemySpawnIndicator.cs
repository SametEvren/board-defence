using System;
using Board;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class EnemySpawnIndicator : MonoBehaviour
    {
        private Image _enemySpawnIndicator;
        private Sprite _mummySprite;
        private Sprite _catSprite;
        private Sprite _birdSprite;

        private Action _onComplete;

        public void SetSpawnIndicator(Image indicator, Sprite mummySprite, Sprite catSprite, Sprite birdSprite)
        {
            _enemySpawnIndicator = indicator;
            _mummySprite = mummySprite;
            _catSprite = catSprite;
            _birdSprite = birdSprite;
        }

        public void SetSprite(EnemyType enemyType, Action onComplete)
        {
            _onComplete = onComplete;

            switch (enemyType)
            {
                case EnemyType.Mummy:
                    _enemySpawnIndicator.sprite = _mummySprite;
                    break;
                case EnemyType.Cat:
                    _enemySpawnIndicator.sprite = _catSprite;
                    break;
                case EnemyType.Bird:
                    _enemySpawnIndicator.sprite = _birdSprite;
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