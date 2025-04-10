using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MojoCase.Game
{
    public class Gate : MonoBehaviour,IDamageable
    {
        private int _gateValue;

        [SerializeField] private TextMeshPro _gateValueText;

        [SerializeField] private int _randomValueRange;

        private bool _isGateLocked;
        
        [SerializeField] private GameObject _lockedGateObject;

        private int _gateLockHealth;

        private void Start()
        {
            if(_isGateLocked)
            {
                _lockedGateObject.SetActive(true);
                _gateLockHealth = Random.Range(_randomValueRange / 2, _randomValueRange);
            }
            
            _gateValue = Random.Range(-_randomValueRange, _randomValueRange);
            SetGateValueText();
        }

        public void TakeDamage(int damage)
        {
            if(_isGateLocked)
                GateLockHit();
            else
            {
                _gateValue++;
                SetGateValueText();
            }
            
            GateHitAnimation();
        }

        private void GateLockHit()
        {
            _gateLockHealth--;
            
            if(_gateLockHealth <= 0)
                UnlockGate();
        }

        private void UnlockGate()
        {
            _isGateLocked = false;
            _lockedGateObject.SetActive(false);
            
            //TODO: Gate unlock effect
        }

        private void SetGateValueText()
        {
            var prefix = _gateValue < 0 ? "-" : "+";
            
            _gateValueText.SetText($"{prefix}{Mathf.Abs(_gateValue)}");
        }

        private void GateHitAnimation()
        {
            //TODO: Bounce anim and haptic.
        }

        public virtual void ApplyGateEffect()
        {
            //TODO: Close gate.
        }
    }
}