using System;
using DG.Tweening;
using MojoCase.Crowd;
using MojoCase.Manager;
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
        
        [SerializeField] private GameObject _lockedGateObject;

        [SerializeField] private FloatReference _bounceEffectPower;
        [SerializeField] private FloatReference _bounceEffectTime;

        [SerializeField] private Color _positiveColor;
        [SerializeField] private Color _negativeColor;
        
        private MaterialPropertyBlock _materialPropertyBlock;
        [SerializeField] private Renderer _gateRenderer;
        
        private Animation _animation;


        private void Awake()
        {
            _animation = gameObject.GetComponent<Animation>();
            _materialPropertyBlock = new MaterialPropertyBlock();
            _gateRenderer.GetPropertyBlock(_materialPropertyBlock);
        }

        private void Start()
        {
            GateValue = Random.Range(-_randomValueRange, _randomValueRange);
            SetGateValueText();
        }

        public void TakeDamage(int damage)
        {
            GateValue++;
            SetGateValueText();
            
            GateHitAnimation();
        }

        private void SetGateValueText()
        {
            var isGateNegative = GateValue < 0;
            var prefix =  isGateNegative ? "-" : "+";
            
            _gateValueText.SetText($"{prefix}{Mathf.Abs(GateValue)}");
            
            _materialPropertyBlock.SetColor("_BaseColor", isGateNegative ? _negativeColor : _positiveColor);
            _gateRenderer.SetPropertyBlock(_materialPropertyBlock);
        }

        private void GateHitAnimation()
        {
            transform.DOComplete();
            transform.DOPunchScale(_bounceEffectPower.Value * Vector3.one, _bounceEffectTime.Value);
            //TODO: Haptic.
        }

        public virtual void ApplyGateEffect(CrowdManager crowdManager)
        {
            var particle = ObjectPooling.Instance.GetFromPool("Gate_Pick_VFX");
            particle.transform.position = transform.position+Vector3.up;
            particle.SetActive(true);
            CloseTheGate();
        }

        private void CloseTheGate()
        {
            GetComponent<Collider>().enabled = false;
            _animation.Play();
        }
    }
}