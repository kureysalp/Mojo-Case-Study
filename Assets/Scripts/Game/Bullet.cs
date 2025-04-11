using System;
using System.Collections;
using System.Collections.Generic;
using MojoCase.Manager;
using UnityEngine;

namespace MojoCase.Game
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private int _damage;
        [SerializeField] private float _speed;
        [SerializeField] private float _lifeTime;

        private void OnEnable()
        {
            StopAllCoroutines();
            StartCoroutine(CO_LifeSpan());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_damage);
                ObjectPooling.Instance.Deposit(gameObject);
            }
        }

        private void ProjectileBehaviour()
        {
            var moveVector =  _speed * Time.deltaTime * Vector3.forward;
            transform.Translate(moveVector);
        }

        private void Update()
        {
            ProjectileBehaviour();
        }

        private IEnumerator CO_LifeSpan()
        {
            yield return new WaitForSeconds(_lifeTime);
            ObjectPooling.Instance.Deposit(gameObject);
        }
        
    }
}