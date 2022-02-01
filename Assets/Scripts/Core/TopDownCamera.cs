using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sluggagames.keepUsAlive.Core
{
    public class TopDownCamera : MonoBehaviour
    {
        /// <summary>
        /// Speed the camera will move (units per seconds)
        /// </summary>
        [Tooltip("Speed the camera will move (units per seconds)")]
        [SerializeField] float movementSpeed = 20f;

        /// <summary>
        /// The lower-left position of the camera, on its current X-Z plane (-1,-1)
        /// </summary>
        [Tooltip("The lower-left position of the camera, on its current X-Z plane")]
        [SerializeField] Vector2 minimumLimit = -Vector2.one;

        /// <summary>
        /// The upper-right position of the camera, on its current X-Z plane (1,1)
        /// </summary>
        [Tooltip("The upper-right position of the camera, on its current X-Z plane")]
        [SerializeField] Vector2 maximumLimit = Vector2.one;

        /// <summary>
        /// Computes the bounding box that the camera is allowed to be in 
        /// </summary>
        Bounds bounds
        {
            get
            {
                // bounding box that is 0 high and positioned at the current height of the camera.
                float cameraHeight = transform.position.y;
             

                // figure position of corners of bounding box in World space.
                Vector3 minLimit = new Vector3(minimumLimit.x, cameraHeight, minimumLimit.y);
                Vector3 maxLimit = new Vector3(maximumLimit.x, cameraHeight, maximumLimit.y);

                // create and return bounding box with these values
                Bounds newBounds = new Bounds();
                newBounds.min = minimumLimit;
                newBounds.max = maximumLimit;
                return newBounds;

            }
        }

        private void Update()
        {
            MoveCamera();

        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
        private void MoveCamera()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // compute how much movement to apply this frame
            Vector3 offSet = new Vector3(horizontal, Input.mouseScrollDelta.y *movementSpeed, vertical) * Time.deltaTime * movementSpeed;

            // figure out new position
            Vector3 newPosition = transform.position + offSet;

            // check if we are within bounds
            if (bounds.Contains(newPosition))
            {
                // then move
                transform.position = newPosition;
            }
            else
            {
                // get closest point to boundary and move there instead
                transform.position = bounds.ClosestPoint(newPosition);
            }
        }
    }
}
