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

        void Die()
        {
            print("Player died ");
        }
    }
}
