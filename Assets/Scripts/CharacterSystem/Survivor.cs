using com.sluggagames.keepUsAlive.Core;
using com.sluggagames.keepUsAlive.Obstacle;
using UnityEngine;

namespace com.sluggagames.keepUsAlive.CharacterSystem
{
    public class Survivor : Character
    {
        internal int keyAmount = 0;
        internal bool hasKey = false;
        public ActivationPad key = null;
      
        private void Update()
        {
            if (keyAmount > 0)
            {
                hasKey = true;
            }
            else if (keyAmount <= 0)
            {
                hasKey = false;
            }

         
            print($"hasKey? {hasKey}");
        }

        public void AddKey(ActivationPad _key)
        {
            if (hasKey) return;
            keyAmount = _key.activationValue;
            GameManager.Instance.IncreaseCurrentKeyValue(_key);
            // keyAmount += 1;
            key = _key;
        }
        
        public void RemoveKey()
        {
            if (!hasKey) return;
            key = null;
        }

        public void DropKey(ActivationPad _key)
        {
            if (!hasKey) return;
            GameManager.Instance.DecreaseCurrentKeyValue(_key);
           GameObject keyObject = GameManager.Instance.GetActivationPad(transform.position);
            ActivationPad _newKey = keyObject.GetComponent<ActivationPad>();
            _newKey.activationValue = _key.activationValue;
            _newKey.activationTime = _key.activationTime;

        }

        

    }
}
