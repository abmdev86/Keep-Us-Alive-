using com.sluggagames.keepUsAlive.CharacterSystem;
using UnityEngine;

namespace com.sluggagames.keepUsAlive.Obstacle
{
    public class ActivationPad : MonoBehaviour
    {

        [SerializeField] internal float activationTime = 2f;
        [SerializeField] internal int activationValue = 1;
        float currentTime = 0;



        private void OnTriggerStay(Collider other)
        {
            
            if (other.gameObject.tag != "Player") return;
            Survivor survivor = other.gameObject.GetComponent<Survivor>();
            if (survivor)
            {
                if (survivor.hasKey) return;
               
                currentTime += Time.deltaTime;

               // print($"+{currentTime}");

                if (currentTime >= activationTime)
                {
                    currentTime = 0;
                    survivor.AddKey(this);
                    gameObject.SetActive(false);
                }

            }


        }



    }
}
