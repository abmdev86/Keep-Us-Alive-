
using UnityEngine;

namespace com.sluggagames.keepUsAlive.Obstacle
{
    public class ObstacleBase : MonoBehaviour
    {
        [Tooltip("The number of activation pads required to move obstacle")]
        [SerializeField] internal int numOfKeysToActivate = 2;
        internal int currentKeyAmount = 0;


        public virtual void AddToCurrentKeyCount(ActivationPad _key)
        {
            //currentKeyAmount += 1;
            currentKeyAmount += _key.activationValue;
        }

        public virtual void ActivateObstacle()
        {
            if (currentKeyAmount != numOfKeysToActivate) return;
            print("moving obstacle");

        }

        public int GetKeyActivationAmount()
        {
            return numOfKeysToActivate;
        }
        public int GetCurrentKeyAmount()
        {
            return currentKeyAmount;
        }
    }
}
