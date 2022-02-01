using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sluggagames.keepUsAlive
{
    public class BoxSelectable : MonoBehaviour
    {

       public void Selected()
        {
            print("Selected Me! " + this.gameObject.name);

        }
    }
}
