using System;
using GPHive.Game;
using MojoCase.Config;
using MojoCase.Manager;
using UnityEngine;

namespace MojoCase.Crowd
{
    public class Warrior : MonoBehaviour
    {
        [SerializeField] private WarriorConfig  _config;
        
        [SerializeField] private Transform _bulletSpawnPoint;

        [SerializeField] private GameObject[] _clothes;

        private int _level;

        private float _fireRateModifier;
        private float FireRate => _config.BaseFireRate + _fireRateModifier * .1f;

        private float _lastShootTime;


        public bool _isActive;

        private void Start()
        {
            SetupWarrior(1);
        }

        public void SetupWarrior(int level)
        {
            _level = level;
            
            foreach (var cloth in _clothes)
                cloth.SetActive(false);
            
            _clothes[_level-1].SetActive(true);
        }

        private void Shoot()
        {
            if(Time.time - _lastShootTime < 1 / FireRate) return;
            
            _lastShootTime = Time.time;

            var bullet = ObjectPooling.Instance.GetFromPool($"Bullet_Level_{_level}");
            bullet.transform.position = _bulletSpawnPoint.position;
            bullet.SetActive(true);
        }

        private void Update()
        {
            if(!_isActive) return;
            
            Shoot();
        }
    }
}