using com.sluggagames.keepUsAlive.CharacterSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sluggagames.keepUsAlive.AI
{
    public class Enemy : Character
    {
        [Range(0, 1)]
        [SerializeField] internal float damage = .02f;
        [SerializeField] internal float attackRate = 3.4f;
        float timeSinceLastAttack;
        [SerializeField] float timeBetweenAttacks = 2f;

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
           

        }
        public float Damage
        {
            get
            {
                return damage;
            }
        }

     
        public void Attack(Health _target)
        {
         
            if (_target.IsDead) return;
            transform.LookAt(_target.transform);
            if(timeSinceLastAttack > timeBetweenAttacks)
            {
                _target.TakeDamage(damage);
                timeSinceLastAttack = 0;
            }
            
            
           
        }
    }
}
