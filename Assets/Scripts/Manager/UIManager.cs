using UnityEngine;

namespace MojoCase.Manager
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _levelStartUI;
        [SerializeField] private GameObject _levelSuccessUI;
        [SerializeField] private GameObject _levelFailedUI;

        private void Awake()
        {
            GameManager.OnGameStart += CloseLevelStartUI;
            GameManager.OnLevelLoaded += ShowLevelStartUI;
            GameManager.OnLevelFailed += ShowLevelFailedUI;
            GameManager.OnLevelSuccess += ShowLevelSuccessUI;
        }

        public void StartGameInput()
        {
            GameManager.Instance.StartTheGame();
        }
        
        public void ReplayGameInput()
        {
            GameManager.Instance.LoadTheLevel();
        }

        private void ShowLevelStartUI()
        {
            _levelStartUI.SetActive(true);
            _levelSuccessUI.SetActive(false);
            _levelFailedUI.SetActive(false);
        }
        private void CloseLevelStartUI()
        {
            _levelStartUI.SetActive(false);
        }

        private void ShowLevelSuccessUI()
        {
            _levelSuccessUI.SetActive(true);
        }

        private void ShowLevelFailedUI()
        {
            _levelFailedUI.SetActive(true);
        }

        private void OnDisable()
        {
            GameManager.OnGameStart -= CloseLevelStartUI;
            GameManager.OnLevelLoaded -= ShowLevelStartUI;
            GameManager.OnLevelFailed -= ShowLevelFailedUI;
            GameManager.OnLevelSuccess -= ShowLevelSuccessUI;
        }
    }
}