using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sluggagames.keepUsAlive.LevelManagement
{
    public class TriggerObjective : MonoBehaviour
    {
        [SerializeField] ObjectiveTrigger objective = new ObjectiveTrigger();
      
        private void Update()
        {
           // gameObject.SetActive(objective.quest.Objectives[objective.objectiveNumber].isVisible);
          
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
               
                this.objective.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}
