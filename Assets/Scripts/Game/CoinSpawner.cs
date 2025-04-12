using System;
using System.Collections;
using MojoCase.Manager;
using MojoCase.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MojoCase.Game
{
    public class CoinSpawner : MonoBehaviour
    {
        [SerializeField] private int _coinCount;
        [SerializeField] private float _coinExplosionForce;
        [SerializeField] private float _coinSpawnRate;
        [SerializeField] private float _coinWaitTime;
        [SerializeField] private float _coinTravelTime;
        
        private Transform _coinUITransform;
        private Camera _camera;
        private Transform _playerTransform;

        public void SpawnCoins()
        {
            _camera = Camera.main;
            _playerTransform = GameObject.Find("Player").transform;
            _coinUITransform = GameObject.Find("Coin_Icon_UI").transform;
            StartCoroutine(CO_CoinSpawner());
        }

        private IEnumerator CO_CoinSpawner()
        {
            var spawnedCoinCount = 0;

            while (spawnedCoinCount < _coinCount)
            {
                var coin = Poolable.Get<Coin>();
                coin.transform.position = transform.position;
                coin.gameObject.SetActive(true);
                var randomForce = Random.insideUnitSphere * _coinExplosionForce;
                coin.Explode(randomForce);
                StartCoroutine(CO_CoinSender(coin));
                yield return new WaitForSeconds(1 / _coinSpawnRate);
                spawnedCoinCount++;
            }
        }

        private IEnumerator CO_CoinSender(Coin coin)
        {
            yield return new WaitForSeconds(_coinWaitTime);
            var startPosition = coin.transform.position;
            var elapsedTime = 0f;

            while (elapsedTime < _coinTravelTime)
            {
                elapsedTime += Time.deltaTime;
                var positionOnUI = _coinUITransform.position; 
                positionOnUI.z = _playerTransform.position.z - _camera.transform.position.z;
                var positionToCoinFly = _camera.ScreenToWorldPoint(positionOnUI);
                coin.transform.position = Vector3.Lerp(startPosition, positionToCoinFly, elapsedTime / _coinTravelTime);
                yield return null;
            }
            PlayerEconomy.Instance.AddCoin(1);
            coin.ReturnToPool();
        }
    }
}