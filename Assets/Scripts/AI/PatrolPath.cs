using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sluggagames.keepUsAlive.AI
{
    public class PatrolPath : MonoBehaviour
    {
        [Range(0, 1)]
        [SerializeField] float pointRadius = 1f;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            for(int i =0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);
                Gizmos.DrawSphere(GetWaypointPos(i), pointRadius);
                Gizmos.DrawLine(GetWaypointPos(i), GetWaypointPos(j));
            }
        }
        internal Vector3 GetWaypointPos(int pointIndex)
        {
            return transform.GetChild(pointIndex).position;
        }

        internal int GetNextIndex(int pointIndex)
        {
           
            if(pointIndex + 1  == transform.childCount)
            {
                return 0;
            }
            return pointIndex + 1;

        }
    }
}
