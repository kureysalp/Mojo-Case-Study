using System;
using MojoCase.Config;
using MojoCase.Manager;
using UnityEngine;

namespace MojoCase.Game
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private PlayerConfig _playerConfig;

        [SerializeField] private Transform _horizontalMover;
        
        private Vector3 _movementInput;
        private Vector3 _currentInputVector;
        private Vector3 _lastTouchPosition;
        private Vector3 _movementVector;

        private Vector3 _playerSwerveStartPosition;
        
        private bool _isControllerActive;

        private void Awake()
        {
            GameManager.OnGameStart += ActivateTheController;
            GameManager.OnGameEnded += DeactivateTheController;
            GameManager.OnLevelLoaded += Reset;
        }

        private void Update()
        {
            if(!_isControllerActive) return;
            
            SwerveInput();
            Movement();
        }

        private void Reset()
        {
            transform.position = Vector3.zero;
        }
        
        private void SwerveInput()
        {
            if(Input.touchCount == 0) return;
            
            var touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _lastTouchPosition = touch.position;
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    var delta = touch.position.x - _lastTouchPosition.x;
                    _movementInput = _playerConfig.SwerveSensitivity * Time.deltaTime * _playerConfig.HorizontalSpeed * delta * Vector3.right;
                    _lastTouchPosition = touch.position;
                    break;
                case TouchPhase.Ended:
                    _movementInput = Vector3.zero;
                    break;
            }
        }

        private void Movement()
        {
            var horizontalMovement = _movementInput;
            var expectedHorizontalPosition = transform.position + horizontalMovement;
            expectedHorizontalPosition.y = 0;
            expectedHorizontalPosition.z = 0;
            
            if (expectedHorizontalPosition.magnitude > _playerConfig.HorizontalMovementLimit)
            {
                var difference = _playerConfig.HorizontalMovementLimit - Mathf.Abs(transform.position.x);
                horizontalMovement = horizontalMovement.normalized * difference;
            }
              
            var forwardMovement = _playerConfig.ForwardSpeed * Time.deltaTime * Vector3.forward;
            transform.Translate(forwardMovement + horizontalMovement);
        }

        private void ActivateTheController()
        {
            _isControllerActive = true;
        }
        
        private void DeactivateTheController()
        {
            _isControllerActive = false;
        }

        private void OnDisable()
        {
            GameManager.OnGameStart += ActivateTheController;
            GameManager.OnGameEnded += DeactivateTheController;
            GameManager.OnLevelLoaded -= Reset;
        }
    }
}
