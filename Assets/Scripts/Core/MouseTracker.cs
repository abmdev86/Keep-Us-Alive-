using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sluggagames.keepUsAlive.Core
{
    public class MouseTracker : MonoBehaviour
    {

        private void Update()
        {
            // int layerMask = 1 << 8;

            //RaycastHit hit;
            //if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity, layerMask))
            //{
            //    Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * hit.distance, Color.yellow);
            //    print($"{hit.transform.name}");
            //    transform.position = hit.point;
            //}
            transform.position = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        }

    }
}
