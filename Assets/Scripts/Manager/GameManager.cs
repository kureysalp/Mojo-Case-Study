using System;

namespace MojoCase.Manager
{
    public class GameManager : Singleton<GameManager>
    {
        public enum GameState
        {
            Idle,
            Playing,
            Ended
        }

        private GameState _currentGameState;
        
        public GameState CurrentGameState => _currentGameState;

        public static event Action OnGameStart;
        public static event Action OnLevelSuccess;
        public static event Action OnLevelFailed;
        public static event Action OnLevelLoaded;
        public static event Action OnGameEnded;

        public void StartTheGame()
        {
            _currentGameState = GameState.Playing;
            OnGameStart?.Invoke();
        }

        public void WinTheLevel()
        {
            OnLevelSuccess?.Invoke();
        }
        
        public void FailTheLevel()
        {
            OnLevelFailed?.Invoke();
        }
        
        private void EndTheGame()
        {
            _currentGameState = GameState.Ended;
            OnGameEnded?.Invoke();
        }

        public void LoadTheLevel()
        {
            _currentGameState = GameState.Idle;
            OnLevelLoaded?.Invoke();
        }
    }
}