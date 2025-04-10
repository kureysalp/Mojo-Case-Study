﻿using TMPro;
using UnityEngine;

namespace MojoCase.Game
{
    public class Block : MonoBehaviour, IDamageable
    {
        private float _health;
        private TextMeshPro _healthText;
        
        public void TakeDamage(int damage)
        {
            _health -= damage;
            
            SetHealthText();
            
            if(_health <= 0)
                DestroyTheBlock();
        }

        private void DestroyTheBlock()
        {
            gameObject.SetActive(false);
        }

        private void SetHealthText()
        {
            _healthText.SetText(_health.ToString());
            
            //TODO: Health text pop animation.
        }
    }
}