using System;
using Board;
using UnityEngine;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private LevelProgression levelProgression;
        public LevelData LevelData { get; private set; }

        private void Awake()
        {
            GetCurrentLevel();
        }

        private void GetCurrentLevel()
        {
            var currentLevel = PlayerPrefs.GetInt("Level", 0);
            var levelIndex = currentLevel % levelProgression.levelDataList.Count;
            LevelData = levelProgression.levelDataList[levelIndex];
        }
    }
}