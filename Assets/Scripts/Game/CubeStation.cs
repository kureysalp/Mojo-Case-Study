using UnityEngine;
using Random = UnityEngine.Random;

namespace MojoCase.Game
{
    public class CubeStation : MonoBehaviour
    {
        [SerializeField] private int _ringCount;
        [SerializeField] private int _verticalLayerCount;
        [SerializeField] private float _cubeSpacing;
        [SerializeField] private float _yOffsetScale;
        [SerializeField] private float _zRotationOffset;

        [SerializeField] private GameObject _cubePrefab;
        
        
        private void Start()
        {
            GenerateTheStation();
        }

        private void GenerateTheStation()
        {
            var currentRing = 1;
            
            while (currentRing <= _ringCount)
            {
                var radius = currentRing * _cubeSpacing;
                var circumference = 2 * Mathf.PI * radius;

                var cubeCountInRing = Mathf.FloorToInt(circumference / _cubeSpacing);
                var angleStep = 360f / cubeCountInRing;

                for (int layerIndex = 0; layerIndex < _verticalLayerCount; layerIndex++)
                {
                    var yPos = layerIndex * _cubeSpacing;
                    for (int cubeIndexInRing = 0; cubeIndexInRing < cubeCountInRing; cubeIndexInRing++)
                    {
                        var cube = Instantiate(_cubePrefab, transform);
                        
                        var angle = angleStep * cubeIndexInRing * Mathf.Deg2Rad;
                        var xPos = Mathf.Sin(angle) * radius;
                        var zPos = Mathf.Cos(angle) * radius;

                        cube.transform.localPosition = new Vector3(xPos, yPos, zPos);
                        var lookAtPosition = new Vector3(transform.position.x, cube.transform.position.y + Random.Range(0f,1f) * _yOffsetScale,
                            transform.position.z);
                        cube.transform.LookAt(lookAtPosition);
                        cube.transform.Rotate(Vector3.forward * Random.Range(-_zRotationOffset, _zRotationOffset), Space.Self);
                    }
                }

                currentRing++;
            }

        }
    }
}