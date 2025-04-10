using DG.Tweening;
using MojoCase.Crowd;
using MojoCase.Utilities;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MojoCase.Game
{
    public class Gate : MonoBehaviour, IDamageable
    {
        protected int GateValue;

        [SerializeField] private TextMeshPro _gateValueText;

        [SerializeField] private int _randomValueRange;

        [SerializeField] private bool _isGateLocked;
        
        [SerializeField] private GameObject _lockedGateObject;

        [SerializeField] private FloatReference _bounceEffectPower;
        [SerializeField] private FloatReference _bounceEffectTime;
        
        private Animation _animation;

        private int _gateLockHealth;

        private void Start()
        {
            if(_isGateLocked)
            {
                _lockedGateObject.SetActive(true);
                _gateLockHealth = Random.Range(_randomValueRange / 2, _randomValueRange);
            }
            
            GateValue = Random.Range(-_randomValueRange, _randomValueRange);
            SetGateValueText();
        }

        public void TakeDamage(int damage)
        {
            if(_isGateLocked)
                GateLockHit();
            else
            {
                GateValue++;
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
            var prefix = GateValue < 0 ? "-" : "+";
            
            _gateValueText.SetText($"{prefix}{Mathf.Abs(GateValue)}");
        }

        private void GateHitAnimation()
        {
            transform.DOComplete();
            transform.DOPunchScale(_bounceEffectPower.Value * Vector3.one, _bounceEffectTime.Value);
            //TODO: Haptic.
        }

        public virtual void ApplyGateEffect(CrowdManager crowdManager)
        {
            gameObject.SetActive(false);
            CloseTheGate();
        }

        private void CloseTheGate()
        {
            GetComponent<Collider>().enabled = false;
            _animation.Play();
        }
    }
}