using System;
using UnityEngine;

namespace Game
{
    public class GameStateController : MonoBehaviour
    {
        public GameState CurrentState { get; private set; } = GameState.Playing;

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