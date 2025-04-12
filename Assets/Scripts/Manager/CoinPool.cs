using MojoCase.Game;
using MojoCase.Utilities;
using UnityEngine;

namespace MojoCase.Manager
{
    public class CoinPool : MonoBehaviour
    {
        [SerializeField] private Coin _coin;
        [SerializeField] private int _poolSize;
        [SerializeField] private int _poolExpandAmount;

        private void Awake()
        {
            CreatePool();
        }

        private void CreatePool()
        {
            Poolable.CreatePool<Coin>(_coin.gameObject, _poolSize, _poolExpandAmount);
        }
    }
}