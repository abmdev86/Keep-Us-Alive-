using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sluggagames.keepUsAlive.Obstacle
{
    /// <summary>
    /// Moves an object at a fixed speed through a series of points. At least 2 points required
    /// </summary>
    public class MovingPlatform : ObstacleBase
    {
        /// <summary>
        /// position the platform will move through stored in local position
        /// </summary>
        [Tooltip("position the platform will move through stored in local position")]
        [SerializeField] Vector3[] points = { };

        /// <summary>
        /// Speed at which it will move between points
        /// </summary>
        [Tooltip("Speed at which it will move between points")]
        [SerializeField] float speed = 10f;

        /// <summary>
        /// The index into points array. this is the point we're trying to move towards
        /// </summary>
        int nextPoint = 0;
        /// <summary>
        /// where the platform was when the game started.
        /// </summary>
        Vector3 startPosition;

        /// <summary>
        /// How fast this platform is moving in units per second
        /// </summary>
        public Vector3 velocity { get; private set; }

        /// <summary>
        /// The point we are currently moving towards
        /// </summary>
        Vector3 currentPoint
        {
            get
            {
                // no points so returning current position
                if (HasPoints())
                {
                    return transform.position;
                }
                return points[nextPoint] + startPosition;
            }
        }

        /// <summary>
        /// Used to initialize the platfrom
        /// </summary>
        private void Start()
        {
            if (Has2Points())
            {
                Debug.LogError("Platform needs at least 2 points to move", this);
                return;
            }
            startPosition = transform.position;
            transform.position = currentPoint;
        }

        /// <summary>
        /// update every time physics updates
        /// </summary>
        private void FixedUpdate()
        {
            // move towards target at fix speed 
            var newPosition = Vector3.MoveTowards(transform.position, currentPoint, speed * Time.fixedDeltaTime);
            if(Vector3.Distance(newPosition, currentPoint) < 0.001f)
            {
                newPosition = currentPoint;

                // move to the next target, wrapping around to the start if needed.
                nextPoint += 1;
                nextPoint %= points.Length;
            }
            // calc current velocity units per second.
            velocity = (newPosition - transform.position) / Time.fixedDeltaTime;

            // update our current position.
            transform.position = newPosition;
            
        }

        private void OnDrawGizmos()
        {
            if (Has2Points())
            {
                return;
            }

            // points are stored in local space, so we need to offset them
            // to know where they are in world space.
            Vector3 offSetPostion = transform.position;

            //if we are playing our transform is moving so we need to use
            // the cached  start postion to figure out where our points are in world space.
            if (Application.isPlaying)
            {
                offSetPostion = startPosition;
            }
            Gizmos.color = Color.blue;

            //loop over points
            for(int p =0; p < points.Length; p++)
            {
                // Get this point and the next one, wrapping around to the first
                Vector3 p1 = points[p];
                Vector3 p2 = points[(p + 1) % points.Length];

                // draw point
                Gizmos.DrawSphere(offSetPostion + p1, 0.1f);

                // draw lines between points
                Gizmos.DrawLine(offSetPostion + p1, offSetPostion + p2);
            }
        }
        bool Has2Points()
        {
            return points == null || points.Length < 2;
        }
        bool HasPoints()
        {
            return points == null || points.Length == 0;
        }

    }
}
