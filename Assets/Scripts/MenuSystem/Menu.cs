
using UnityEngine;
using UnityEngine.Events;

namespace com.sluggagames.keepUsAlive.MenuSystem
{
    public class Menu : MonoBehaviour
    {
        public UnityEvent menuDidAppear = new UnityEvent();

        public UnityEvent menuWillDisappear = new UnityEvent();
    }
}
