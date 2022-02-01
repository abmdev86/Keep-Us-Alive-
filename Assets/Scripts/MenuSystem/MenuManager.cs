using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sluggagames.keepUsAlive.MenuSystem
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] List<Menu> menus = new List<Menu>();

        private void Start()
        {
            ShowMenu(menus[0]);
        }

        public void ShowMenu(Menu _menuToShow)
        {
            // is this the menu we are tracking
            if (menus.Contains(_menuToShow))
            {
                Debug.Log($"{_menuToShow.name} is in the list of menus");
                return;
            }

            foreach(var otherMenu in menus)
            {

                // enable this menu and disable the others
                if (otherMenu == _menuToShow)
                {
                    otherMenu.gameObject.SetActive(true);

                    // invoke Menu's "Did Appear"
                    otherMenu.menuDidAppear.Invoke();
                }
                else
                {
                    // check if this menu is currently active
                    if (otherMenu.gameObject.activeInHierarchy)
                    {
                        // active so invoke "Will disappear"
                        otherMenu.menuWillDisappear.Invoke();
                    }
                    //mark inactive
                    otherMenu.gameObject.SetActive(false);
                }
            }
        }
    }
}
