using System.Collections.Generic;
using MojoCase.Manager;
using MojoCase.Utilities;
using UnityEngine;

namespace MojoCase.Crowd
{
    public class CrowdManager : MonoBehaviour
    {
        private readonly List<Warrior> _warriors = new();

        private int _fireRateModifier;

        private void Awake()
        {
            GameManager.OnGameStart += EnableShooting;
            GameManager.OnGameEnded += DisableShooting;
        }
        
        private void Start()
        {
            var warrior = Poolable.Get<Warrior>();
            AddWarrior(warrior,1);
        }

        private void AddWarrior(Warrior warrior, int level)
        {
            _warriors.Add(warrior);
            warrior.SetupWarrior(level,_fireRateModifier, this);
            
            warrior.transform.SetParent(transform);
            warrior.transform.ResetLocalTransform();
            
            //TODO: Set warrior position.
        }

        private void RemoveWarrior(Warrior warrior)
        {
            _warriors.Remove(warrior);
            warrior.ReturnToPool();
        }

        public void AddWarriorInBulk(int count, int level)
        {
            for (int i = 0; i < count; i++)
            {
                var warrior = Poolable.Get<Warrior>();
                AddWarrior(warrior.GetComponent<Warrior>(), level); // Come back here to fix getcomponent on every add. 
            }
        }
        
        public void RemoveWarriorInBulk(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var warriorToRemove= _warriors[^1];
                RemoveWarrior(warriorToRemove);
                //TODO: VFX
            }
        }

        public void AddFireRateModifier(int fireRateModifier)
        {
            _fireRateModifier += fireRateModifier;
            foreach (var warrior in _warriors)
                warrior.SetFireRateModifier(_fireRateModifier);
        }

        public void KillWarrior(Warrior warrior)
        {
            RemoveWarrior(warrior);
            //TODO: Play particle on it's position.
        }

        private void EnableShooting()
        {
            foreach (var warrior in _warriors)
                warrior._isActive = true;
        }
        
        private void DisableShooting()
        {
            foreach (var warrior in _warriors)
                warrior._isActive = false;
        }

        private void OnDisable()
        {
            GameManager.OnGameStart -= EnableShooting;
            GameManager.OnGameEnded -= DisableShooting;
        }
    }
}