using System;
using GPHive.Game;
using MojoCase.Config;
using MojoCase.Game;
using MojoCase.Manager;
using MojoCase.Utilities;
using UnityEngine;

namespace MojoCase.Crowd
{
    public class Warrior : Poolable
    {
        private static readonly int AttackAnimationTrigger = Animator.StringToHash("attack");
        private static readonly int RunAnimationTrigger = Animator.StringToHash("run");
        private static readonly int IdleAnimationTrigger = Animator.StringToHash("idle");
        private static readonly int RateAnimationFloat = Animator.StringToHash("fireRate");
        [SerializeField] private WarriorConfig  _config;
        
        [SerializeField] private Transform _bulletSpawnPoint;

        [SerializeField] private GameObject[] _clothes;
        
        private CrowdManager _crowdManager;
        private Animator _animator;

        private int _level;

        private float _fireRateModifier;
        private float FireRate => _config.BaseFireRate + _fireRateModifier * .1f;

        private float _lastShootTime;
        
        public bool _isActive;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

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

            _animator.SetTrigger(AttackAnimationTrigger);
        }

        private void ShootBullet()
        {
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
            _animator.SetFloat(RateAnimationFloat,FireRate);
        }


        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Block"))
                Die();
            
            if(other.CompareTag("Gate"))
                other.GetComponent<Gate>().ApplyGateEffect(_crowdManager);
        }

        private void Die()
        {
            _crowdManager.KillWarrior(this);
        }

        public void ActivateTheWarrior()
        {
            _isActive = true;
            _animator.SetTrigger(RunAnimationTrigger);
        }
        
        public void DeactivateTheWarrior()
        {
            _isActive = false;
            _animator.SetTrigger(IdleAnimationTrigger);
        }
        
        
    }
}