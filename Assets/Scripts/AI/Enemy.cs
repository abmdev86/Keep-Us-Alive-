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

        public float Damage
        {
            get
            {
                return damage;
            }
        }

        internal IEnumerator Attack(Health _target)
        {
            _target.TakeDamage(damage);
            yield return new WaitForSeconds(attackRate);
        }
    }
}
