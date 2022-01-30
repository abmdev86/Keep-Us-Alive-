using com.sluggagames.keepUsAlive.AI;
using com.sluggagames.keepUsAlive.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sluggagames.keepUsAlive.CharacterSystem
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float characterHealth = 1;
        public float CharacterHealth
        {
            get
            {
                return characterHealth;
            }
        }
        [SerializeField] bool _isDead;
        public bool IsDead
        {
            get
            {
                if (characterHealth <= 0)
                {
                    return _isDead = true;
                }
                else
                {
                    return _isDead = false;
                }
            }
        }

        private void Update()
        {
            if (IsDead)
            {
                Die();
            }
        }

        public void TakeDamage(float v)
        {
            characterHealth -= v;
        }

        void Die()
        {
            Survivor Survivor = GetComponent<Survivor>();
            Enemy Monster = GetComponent<Enemy>();
            if (!Survivor && !Monster) return;
            if (Survivor)
            {
                // drop any held keys
                GameManager.Instance.RemoveFromSelection((Character)Survivor);
                Survivor.DropKey(Survivor.key);
                
                //play death animation

                Destroy(Survivor.gameObject);
            }
            if (Monster)
            {
                // play death animation

                Destroy(Monster.gameObject);
            }
            
        }
    }
}