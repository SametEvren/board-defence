using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    public class GameStateController : MonoBehaviour
    {
        [SerializeField] private GameState currentState = GameState.Playing;

        public GameState CurrentState
        {
            get => currentState;
            private set => currentState = value;
        }

        public event Action<GameState> OnGameStateChanged;

        public void SetGameState(GameState newState)
        {
            if (CurrentState != newState)
            {
                CurrentState = newState;
                OnGameStateChanged?.Invoke(newState);
            }
        }
    }

    public enum GameState
    {
        Playing,
        Paused,
        Victory,
        Defeat
    }
}