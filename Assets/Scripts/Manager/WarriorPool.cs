using System;
using MojoCase.Crowd;
using MojoCase.Utilities;
using UnityEngine;

namespace MojoCase.Manager
{
    public class WarriorPool : MonoBehaviour
    {
        [SerializeField] private Warrior _warrior;
        [SerializeField] private int _poolSize;
        [SerializeField] private int _poolExpandAmount;

        private void Awake()
        {
            CreatePool();
        }

        private void CreatePool()
        {
            Poolable.CreatePool<Warrior>(_warrior.gameObject, _poolSize, _poolExpandAmount);
        }
    }
}