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
       // [SerializeField] bool _isDead;
        public bool IsDead
        {
            get
            {
                return characterHealth <= 0;
            }
        }

        private void Update()
        {
           
            if (IsDead)
            {
                print($"Died so isdead is: {IsDead}");
                Die();
                return;
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
                GameManager.Instance.RemoveFromSelection(Survivor);
                // Survivor.DropKey(Survivor.key);

                //play death animation

                Destroy(Survivor.gameObject, 2f);
            }
            if (Monster)
            {
                // play death animation

                Destroy(Monster.gameObject);
            }

        }
    }
}
