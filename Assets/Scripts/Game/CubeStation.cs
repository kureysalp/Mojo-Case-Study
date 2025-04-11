using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MojoCase.Manager;
using MojoCase.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MojoCase.Game
{
    public class CubeStation : MonoBehaviour, IDamageable
    {
        [SerializeField] private int _ringCount;
        [SerializeField] private int _verticalLayerCount;
        [SerializeField] private float _cubeSpacing;
        [SerializeField] private float _yOffsetScale;
        [SerializeField] private float _zRotationOffset;

        [SerializeField] private float _cubeJumpPower;

        [SerializeField] private GameObject _cubePrefab;

        [SerializeField] private FloatReference _stationBouncePower;
        [SerializeField] private FloatReference _stationBounceTime;

        [SerializeField] private float _cubeSendDelay;

        private readonly List<CubeStationLayer> _layers = new();
        
        private readonly List<GameObject> _cubes = new();

        private int _currentRing;
        
        public static event Action<Rigidbody> OnShootCube;
        
        
        private void Start()
        {
            GenerateTheStation();
        }

        private void GenerateTheStation()
        {
            _currentRing = 1;
            
            while (_currentRing <= _ringCount)
            {
                var poolName = $"Station_Cube_Color_{Random.Range(0, 3)}";
                var cubeLayer = new CubeStationLayer(poolName); 
                _layers.Add(cubeLayer);
                
                var radius = _currentRing * _cubeSpacing;
                var circumference = 2 * Mathf.PI * radius;

                var cubeCountInRing = Mathf.FloorToInt(circumference / _cubeSpacing);
                var angleStep = 360f / cubeCountInRing;

                for (int layerIndex = 0; layerIndex < _verticalLayerCount; layerIndex++)
                {
                    var yPos = layerIndex * _cubeSpacing;
                    for (int cubeIndexInRing = 0; cubeIndexInRing < cubeCountInRing; cubeIndexInRing++)
                    {
                        var cube = ObjectPooling.Instance.GetFromPool(poolName); 
                        cube.SetActive(true);
                        cube.transform.SetParent(transform);
                        
                        var angle = angleStep * cubeIndexInRing * Mathf.Deg2Rad;
                        var xPos = Mathf.Sin(angle) * radius;
                        var zPos = Mathf.Cos(angle) * radius;

                        cube.transform.localPosition = new Vector3(xPos, yPos, zPos);
                        var yLookAtOffset = Random.Range(0f,1f) * _yOffsetScale;
                        var lookAtPosition = new Vector3(transform.position.x, cube.transform.position.y + yLookAtOffset,
                            transform.position.z);
                        cube.transform.LookAt(lookAtPosition);
                        var zRotateOffset = Random.Range(-_zRotationOffset, _zRotationOffset);
                        cube.transform.Rotate(Vector3.forward * zRotateOffset, Space.Self);
                        cube.transform.localScale = Vector3.one;
                        
                        _cubes.Add(cube);
                        cubeLayer.AddCube();
                    }
                }
                _currentRing++;
            }

            _currentRing--;
        }

        public void TakeDamage(int damage)
        {
            for (int i = 0; i < damage; i++)
            {
                var currentLayer = _layers[_currentRing - 1];
                if (currentLayer.CubeCount == 0)
                {
                    _currentRing--;
                    if(_currentRing < 0) break;
                    currentLayer = _layers[_currentRing - 1];
                }
                currentLayer.RemoveCube();

                var cubeToDestroy = _cubes[^1];
                
                var cubeToSpawn = ObjectPooling.Instance.GetFromPool(currentLayer.PoolName + "_Physics");
                cubeToSpawn.transform.position = cubeToDestroy.transform.position;
                cubeToSpawn.transform.rotation = cubeToDestroy.transform.rotation;
                cubeToSpawn.SetActive(true);

                var cubeRigidbody = cubeToSpawn.GetComponent<Rigidbody>(); // TODO: Change this later.

                var randomForceVector = Random.onUnitSphere * _cubeJumpPower;
                randomForceVector.x /= 2;
                randomForceVector.y = Mathf.Abs(randomForceVector.y);
                cubeRigidbody.isKinematic = false;
                cubeRigidbody.AddForce(randomForceVector, ForceMode.Impulse);
                cubeRigidbody.AddTorque(randomForceVector, ForceMode.Impulse);

                ObjectPooling.Instance.Deposit(cubeToDestroy);
                _cubes.RemoveAt(_cubes.Count - 1);
                StartCoroutine(CO_SendCube(cubeRigidbody)); 
            }

            transform.DOComplete();
            transform.DOPunchPosition(Vector3.one * _stationBouncePower.Value, _stationBounceTime.Value);
        }

        private IEnumerator CO_SendCube(Rigidbody cubeRigidbody)
        {
            yield return new WaitForSeconds(_cubeSendDelay);
            OnShootCube?.Invoke(cubeRigidbody);
        }

        public void HideTheStation()
        {
            GetComponent<Collider>().enabled = false;
            transform.DOScale(Vector3.zero, .5f);
        }
    }

    public class CubeStationLayer
    {
        private int _cubeCount;
        private string _poolName;

        public CubeStationLayer(string poolName)
        {
            _poolName = poolName;
        }

        public string PoolName => _poolName;

        public int CubeCount => _cubeCount;

        public void AddCube()
        {
            _cubeCount++;
        }
        
        public void RemoveCube()
        {
            _cubeCount++;
        }
    }
}