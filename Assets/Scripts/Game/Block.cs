using DG.Tweening;
using MojoCase.Utilities;
using TMPro;
using UnityEngine;

namespace MojoCase.Game
{
    public class Block : MonoBehaviour, IDamageable
    {
        private float _health;
        [SerializeField] private TextMeshPro _healthText;

        [SerializeField] private FloatReference _hitEffectPower;
        [SerializeField] private FloatReference _hitEffectTime;

        public void SetBlockHealth(float health)
        {
            _health = health;
            SetHealthText();
        }
        
        public void TakeDamage(int damage)
        {
            _health--;
            SetHealthText();
            
            if(_health <= 0)
                DestroyTheBlock();
            
            transform.DOComplete();
            transform.DOPunchScale(Vector3.one * _hitEffectPower.Value, _hitEffectTime.Value);
        }

        private void DestroyTheBlock()
        {
            gameObject.SetActive(false);
        }

        private void SetHealthText()
        {
            _healthText.SetText(_health.ToString());
        }
    }
}