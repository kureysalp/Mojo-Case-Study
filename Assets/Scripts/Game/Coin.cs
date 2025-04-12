using System;
using System.Collections;
using MojoCase.Manager;
using MojoCase.Utilities;
using UnityEngine;

namespace MojoCase.Game
{
    public class Coin : Poolable
    {
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Explode(Vector3 force)
        {
            _rigidbody.AddForce(force, ForceMode.Impulse);
        }
    }
}