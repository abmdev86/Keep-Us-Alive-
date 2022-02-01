using com.sluggagames.keepUsAlive.CharacterSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sluggagames.keepUsAlive.LevelManagement
{
    /// <summary>
    /// Attached usually to characters to trigger and objective based on a factor like health.
    /// </summary>
    public class TriggerObjectiveOnHealth : MonoBehaviour
    {
       internal Health characterHealth;
      [SerializeField]  internal ObjectiveTrigger objective = new ObjectiveTrigger();

        private void Awake()
        {
            characterHealth = GetComponent<Health>();
        }

        private void Update()
        {
            
            if (this.characterHealth.IsDead)
            {
                
                objective.Invoke();
            }
        }

    }
}
