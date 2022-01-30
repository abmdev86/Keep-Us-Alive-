using com.sluggagames.keepUsAlive.CharacterSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sluggagames.keepUsAlive.Obstacle
{
    public class ActivationPad : MonoBehaviour
    {
       [SerializeField] ObstacleBase obstacleToControl;
        [SerializeField] float activationTime = 2f;
        float currentTime;



        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag != "Player") return;
            Survivor survivor = other.gameObject.GetComponent<Survivor>();
            if (survivor)
            {
                if (survivor.hasKey) return;
                currentTime += Time.deltaTime;
                if (currentTime >= activationTime)
                {
                    currentTime = 0;
                    survivor.AddKey(); 
                    Destroy(gameObject);
                }

            }
       
            
        }

      

    }
}
