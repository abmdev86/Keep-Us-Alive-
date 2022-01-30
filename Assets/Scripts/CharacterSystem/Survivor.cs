using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sluggagames.keepUsAlive.CharacterSystem
{
    public class Survivor : Character
    {
        internal int keyAmount = 0;
        internal bool hasKey = false;
      
        private void Update()
        {
            if(keyAmount > 0)
            {
                hasKey = true;
            }
            else if( keyAmount <= 0)
            {
                hasKey = false;
            }
        }

        public void AddKey()
        {
            if (hasKey) return;
            keyAmount += 1;
        }
        
        public void RemoveKey()
        {
            if (!hasKey) return;
            keyAmount -= 1;
            if (keyAmount < 0) keyAmount = 0;
        }

        

    }
}
