using MojoCase.Manager;
using UnityEngine;

namespace MojoCase.Game
{
    public class ParticlePoolReturn : MonoBehaviour
    {
        public void OnParticleSystemStopped()
        {
            ObjectPooling.Instance.Deposit(gameObject);
        }
    }
}