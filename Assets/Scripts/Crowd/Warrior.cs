﻿using System;
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
        [SerializeField] private GameObject _levelUpVfx;
        
        private CrowdManager _crowdManager;
        private Animator _animator;

        private int _level;

        private float _fireRateModifier;
        private float FireRate => _config.BaseFireRate + _fireRateModifier * .01f;

        private float _lastShootTime;
        
        private bool _isActive;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetupWarrior(int level, int fireRateModifier, CrowdManager crowdManager)
        {
            _level = level;
            
            ChangeCloth();
            SetFireRateModifier(fireRateModifier);
            _crowdManager = crowdManager;
        }

        private void ChangeCloth()
        {
            foreach (var cloth in _clothes)
                cloth.SetActive(false);

            _clothes[_level-1].SetActive(true);
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
            
            if(other.CompareTag("Finish"))
                GameManager.Instance.WinTheLevel();
            
            if(other.CompareTag("CubeStation"))
                other.GetComponent<CubeStation>().HideTheStation();
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

        public void LevelUp()
        {
            _level++;
            ChangeCloth();
            _levelUpVfx.SetActive(true);
        }
    }
}