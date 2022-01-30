using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sluggagames.keepUsAlive.Obstacle
{
    public class ObstacleBase : MonoBehaviour
    {
        [Tooltip("The number of activation pads required to move obstacle")]
        [SerializeField] internal int KeyActivationAmount = 2;
        internal int currentKeyAmount;
        public int CurrentActivationAmount
        {
            get
            {
                return currentKeyAmount;

            }
           
        }

        public virtual void AddToCurrentKeyAmount()
        {
            currentKeyAmount += 1;
        }

        public virtual void ActivateObstacle()
        {
            if (currentKeyAmount != KeyActivationAmount) return;
            print("moving obstacle");

        }

        public int GetKeyActivationAmount()
        {
            return KeyActivationAmount;
        }
        public int GetCurrentKeyAmount()
        {
            return currentKeyAmount;
        }
    }
}
