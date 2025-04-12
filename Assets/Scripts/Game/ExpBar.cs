using System;
using System.Collections;
using MojoCase.Manager;
using MojoCase.Utilities;
using TMPro;
using UnityEngine;

namespace MojoCase.Game
{
    public class ExpBar : MonoBehaviour
    {
        [SerializeField] private SlicedFilledImage _fillBar;
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _endPoint;

        [SerializeField] private TextMeshProUGUI _currentLevelText;
        [SerializeField] private TextMeshProUGUI _nextLevelText;

        [SerializeField] private float _cubeMoveTime;
        [SerializeField] private int _maxLevel;
         private int _currentLevel;

        private int _currentExp;
        [SerializeField] private int _expToLevelUp;

        private Camera _camera;

        private GameObject _player;
        
        public static event Action OnLevelUp;

        private void Awake()
        {
            _camera = Camera.main;
            _player = GameObject.Find("Player");

            CubeStation.OnShootCube += FillTheBar;
            AddExpGate.OnAddExp += AddExp;
            GameManager.OnLevelLoaded += Reset;
        }

        private void Reset()
        {
            _currentLevel = 1;
            _currentExp = 0;
            SetLevelText();
            _fillBar.fillAmount = 0;
        }

        private void Start()
        {
            _currentLevel = 1;
            SetLevelText();
        }

        private void FillTheBar(Rigidbody cubeRigidbody)
        {
            _currentExp++;
            var fillAmount = _currentExp / (float)_expToLevelUp;
            _fillBar.fillAmount = fillAmount;
            
            StartCoroutine(CO_MoveCubeToBar(fillAmount, cubeRigidbody));

            if (_currentLevel == _maxLevel) return;
            if (_currentExp >= _expToLevelUp)
                LevelUp();
        }

        private void AddExp(int exp)
        {
            if (_currentLevel == _maxLevel) return;
            var expDifference = _expToLevelUp - _currentExp;
            
            _currentExp += exp;
            if (_currentExp >= _expToLevelUp)
                LevelUp();
           
            if (exp > expDifference)
                _currentExp = exp - expDifference;
            
            var fillAmount = _currentExp / (float)_expToLevelUp;
            _fillBar.fillAmount = fillAmount;
        }

        private void LevelUp()
        {
            _currentLevel++;
            SetLevelText();
            _fillBar.fillAmount = 0;
            _currentExp = 0;
            OnLevelUp?.Invoke();
        }

        private void SetLevelText()
        {
            _currentLevelText.text = _currentLevel.ToString();
            _nextLevelText.text = _currentLevel != _maxLevel ? (_currentLevel + 1).ToString() : "MAX";
        }

        private IEnumerator CO_MoveCubeToBar(float fillAmount, Rigidbody cubeRigidbody)
        {
            cubeRigidbody.ResetVelocity();
            cubeRigidbody.isKinematic = true;
         
            var cubeStartPosition = cubeRigidbody.transform.position;
            var elapsedTime = 0f;
            
            while (elapsedTime < _cubeMoveTime)
            {
                elapsedTime += Time.deltaTime;
                var cubePositionOnScreen = Vector3.Lerp(_startPoint.position, _endPoint.position, fillAmount);
                cubePositionOnScreen.z = _player.transform.position.z - _camera.transform.position.z;
                var positionToCubeFly = _camera.ScreenToWorldPoint(cubePositionOnScreen);

                cubeRigidbody.transform.position =
                    Vector3.Lerp(cubeStartPosition, positionToCubeFly, elapsedTime / _cubeMoveTime);
                yield return null;
            }

            ObjectPooling.Instance.Deposit(cubeRigidbody.gameObject);
        }
        
        private void OnDisable()
        {
            CubeStation.OnShootCube -= FillTheBar;
            AddExpGate.OnAddExp -= AddExp;
            GameManager.OnLevelLoaded -= Reset;
        }
    }
}