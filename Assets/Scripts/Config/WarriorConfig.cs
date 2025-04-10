using UnityEngine;

namespace MojoCase.Config
{
    [CreateAssetMenu(fileName = "SO_Warrior_Config", menuName = "Scriptable Objects/Warrior Config", order = 0)]
    public class WarriorConfig : ScriptableObject
    {
        [SerializeField] private float _baseFireRate;
        [SerializeField] private float _baseDamage;

        public float BaseFireRate => _baseFireRate;

        public float BaseDamage => _baseDamage;
    }
}