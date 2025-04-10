using UnityEngine;

namespace MojoCase.Config
{
    [CreateAssetMenu(fileName = "Player Config", menuName = "Scriptable Objects/Player Config", order = 0)]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private float _forwardSpeed;
        [SerializeField] private float _horizontalSpeed;
        [SerializeField] private float _swerveSensitivity;
        [SerializeField] private float _horizontalMovementLimit;

        public float ForwardSpeed => _forwardSpeed;

        public float HorizontalSpeed => _horizontalSpeed;
        public float SwerveSensitivity => _swerveSensitivity;

        public float HorizontalMovementLimit => _horizontalMovementLimit;
    }
}