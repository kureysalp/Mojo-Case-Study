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

        private int _warriorCountInCurrentRing;
        private int _currentRing;
        [SerializeField] private float _soldierSpacing;
        [SerializeField] private float _soldierPositionOffset;

        private void Awake()
        {
            GameManager.OnGameStart += EnableShooting;
            GameManager.OnGameEnded += DisableShooting;
        }
        
        private void Start()
        {
            _currentRing = 1;
            var warrior = Poolable.Get<Warrior>();
            AddWarrior(warrior,1);
        }

        private void AddWarrior(Warrior warrior, int level)
        {
            _warriors.Add(warrior);
            warrior.SetupWarrior(level, _fireRateModifier, this);
            
            warrior.transform.SetParent(transform);
            warrior.transform.ResetLocalTransform();
            
            //TODO: Set warrior position.
            
            if(_warriors.Count == 1) return;
            
            var radius = _soldierSpacing * _currentRing;
            var circumference = 2 * Mathf.PI * radius;

            var warriorSizeOfCurrentRing = Mathf.FloorToInt(circumference / _soldierSpacing);
            var angleStep = 360f / warriorSizeOfCurrentRing;

            var angle = _warriorCountInCurrentRing * angleStep * Mathf.Deg2Rad;
            var xPos =  Mathf.Sin(angle) * radius;
            var zPos = Mathf.Cos(angle) * radius;

            var randomOffset = Random.insideUnitSphere * _soldierPositionOffset;
            randomOffset.y = 0;
            var newWarriorPosition = new Vector3(xPos, 0f, zPos) + randomOffset;
            
            warrior.transform.localPosition = newWarriorPosition;

            _warriorCountInCurrentRing++;

            if (_warriorCountInCurrentRing >= warriorSizeOfCurrentRing)
            {
                _currentRing++;
                _warriorCountInCurrentRing = 0;
            }

            Debug.Log(_currentRing);
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
                AddWarrior(warrior, level); 
                warrior.ActivateTheWarrior();
            }
        }
        
        public void RemoveWarriorInBulk(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if(_warriors.Count == 0) break;
                var warriorToRemove= _warriors[^1];
                RemoveWarrior(warriorToRemove);
                //TODO: VFX
            }
            
            if(count >= _warriors.Count)
                GameManager.Instance.FailTheLevel();
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
                warrior.ActivateTheWarrior();
        }
        
        private void DisableShooting()
        {
            foreach (var warrior in _warriors)
                warrior.DeactivateTheWarrior();
        }

        private void OnDisable()
        {
            GameManager.OnGameStart -= EnableShooting;
            GameManager.OnGameEnded -= DisableShooting;
        }
    }
}