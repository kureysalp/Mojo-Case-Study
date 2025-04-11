using UnityEngine;

namespace MojoCase.Manager
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private GameObject _levelToLoad;
        private GameObject _currentLevel;

        private void Awake()
        {
            GameManager.OnLevelLoaded += LoadLevel;
            LoadLevel();
        }

        private void LoadLevel()
        {
            if(_currentLevel != null)
                Destroy(_currentLevel);
            
            _currentLevel = Instantiate(_levelToLoad);
        }

        private void OnDisable()
        {
            GameManager.OnLevelLoaded -= LoadLevel;
        }
    }
}