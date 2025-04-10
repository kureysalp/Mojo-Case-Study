using System;
using System.Collections.Generic;
using MojoCase.Manager;
using UnityEngine;

namespace MojoCase.Crowd
{
    public class CrowdManager : MonoBehaviour
    {
        private List<Warrior> _warriors = new();

        private int _fireRateModifier;

        private void Start()
        {
            var warriorsAtStart = GetComponentsInChildren<Warrior>();
            foreach (var warrior in warriorsAtStart)
                AddWarrior(warrior,1);
        }

        private void AddWarrior(Warrior warrior, int level)
        {
            _warriors.Add(warrior);
            warrior.SetupWarrior(level,_fireRateModifier, this);
            
            //TODO: Set warrior position.
        }

        private void RemoveWarrior(Warrior warrior)
        {
            _warriors.Remove(warrior);
            ObjectPooling.Instance.Deposit(warrior.gameObject);
        }

        public void AddWarriorInBulk(int count, int level)
        {
            for (int i = 0; i < count; i++)
            {
                var warrior = ObjectPooling.Instance.GetFromPool($"Warrior_Level_{level}");
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
            _warriors.Remove(warrior);
            ObjectPooling.Instance.Deposit(warrior.gameObject);
            //TODO: Play particle on it's position.
        }
    }
}