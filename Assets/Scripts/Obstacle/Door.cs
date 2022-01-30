using com.sluggagames.keepUsAlive.CharacterSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sluggagames.keepUsAlive.Obstacle
{
    public class Door : ObstacleBase
    {
        bool isOpened;
        [Range(2f, 10f)]
        [SerializeField] float openDoorHeight;
       

        private void Update()
        {
          if(this.currentKeyAmount == this.numOfKeysToActivate)
            {
                ActivateObstacle();
            }
        }

        public override void ActivateObstacle()
        {
            base.ActivateObstacle();
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                Survivor survivor = other.gameObject.GetComponent<Survivor>();
                if (survivor && survivor.hasKey)
                {
                   AddToCurrentKeyCount(survivor.key);
                        survivor.RemoveKey();
               
                }
            }
        }

        public override void AddToCurrentKeyCount(ActivationPad _key)
        {
            this.currentKeyAmount += _key.activationValue;
        }

    }
}
