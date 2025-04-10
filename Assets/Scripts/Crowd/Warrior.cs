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
        
        private CrowdManager _crowdManager;

        private int _level;

        private float _fireRateModifier;
        private float FireRate => _config.BaseFireRate + _fireRateModifier * .1f;

        private float _lastShootTime;


        public bool _isActive;

        public void SetupWarrior(int level, int fireRateModifier, CrowdManager crowdManager)
        {
            _level = level;
            
            foreach (var cloth in _clothes)
                cloth.SetActive(false);
            
            _clothes[_level-1].SetActive(true);
            SetFireRateModifier(fireRateModifier);
            _crowdManager = crowdManager;
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

        public void SetFireRateModifier(int rate)
        {
            _fireRateModifier = rate;
        }


        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Block"))
                Die();    
        }

        private void Die()
        {
            _crowdManager.KillWarrior(this);
        }
    }
}