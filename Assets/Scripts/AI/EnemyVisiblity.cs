#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
#endif
using UnityEngine;

namespace com.sluggagames.keepUsAlive.AI
{
    /// <summary>
    /// Detectes when a given target is visibile to this object. it is visible when both in front and in range of this object.
    /// </summary>
    public class EnemyVisiblity : MonoBehaviour
    {
        public Transform target = null;

        /// <summary>
        ///  if the target is more than this distance away it cannot see it.
        /// </summary>
        public float maxDistance = 10f;
        /// <summary>
        /// angle of visibility arc
        /// </summary>
        public float angle = 45f;

        /// <summary>
        /// Visualize changes with color
        /// </summary>
        [SerializeField] bool visualize = true;

        /// <summary>
        /// Public accessed bool that returns if target is visible or not.
        /// </summary>
        public bool targetIsVisible { get; private set; }

        List<Transform> targets = new List<Transform>();
        GameObject[] _survivorsInMap;

        public void SetTarget(Transform _target)
        {
            if (_target == null) return;
            target = _target;
           
        }
        private void Start()
        {
            _survivorsInMap = GameObject.FindGameObjectsWithTag("Player");
            // set random target.
            target = _survivorsInMap[Random.Range(0, _survivorsInMap.Length)].transform;

        }
       
        public bool UpdateTarget()
        {
           
            targets.Clear();
   
            if (_survivorsInMap.Length == 0) return false;
           
            for (int i = 0; i < _survivorsInMap.Length; i++)
            {
                if (_survivorsInMap[i] == null) continue;
                if (targets.Contains(_survivorsInMap[i].transform)) continue;

                targets.Add(_survivorsInMap[i].transform);

            }
            if (targets.Count > 0)
            {
                foreach (Transform tran in targets)
                {
                    
                    if (tran == target) continue;
                    target = tran;
                }

            }
            return true;
        }

        private void Update()
        {
            if(_survivorsInMap.Length == 0)
            {
                print("No survivors left");
                target = null;
                Destroy(gameObject);
            }

            if (target == null)
            {
                UpdateTarget();
                
                return;
            }

            targetIsVisible = CheckVisibility();
            if (visualize)
            {
                var color = targetIsVisible ? Color.red : Color.white;
                GetComponent<Renderer>().material.color = color;

            }
        }

        public bool CheckVisibilityToPoint(Vector3 _worldPoint)
        {
            var directionToTarget = _worldPoint - transform.position;
            var degreesToTarget = Vector3.Angle(transform.forward, directionToTarget);
            var withinArc = degreesToTarget < (angle / 2);
            if (withinArc == false)
            {
                return false;
            }

            var distanceToTarget = directionToTarget.magnitude;
            var rayDistance = Mathf.Min(maxDistance, distanceToTarget);

            var ray = new Ray(transform.position, directionToTarget);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                if (hit.collider.gameObject == target)
                {
                    // we can see the target 
                    return true;
                }
                // something between us and target cannot see target 
                return false;
            }
            else
            {
                // no obstructed LOS we can see target
                return true;
            }
        }

        public bool CheckVisibility()
        {
            var directionToTarget = target.position - transform.position;
            var degreesToTarget = Vector3.Angle(transform.forward, directionToTarget);
            var withinArc = degreesToTarget < (angle / 2);
            if (withinArc == false)
            {
                return false;
            }

            var distanceToTarget = directionToTarget.magnitude;

            var rayDistance = Mathf.Min(maxDistance, distanceToTarget);

            var ray = new Ray(transform.position, directionToTarget);
            RaycastHit hit;

            var canSee = false;

            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                if (hit.collider.transform == target)
                {
                    canSee = true;
                }
                Debug.DrawLine(transform.position, hit.point);

            }
            else
            {
                Debug.DrawRay(transform.position, directionToTarget.normalized * rayDistance);
            }
            return canSee;
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// allows editing of visible range for enemy visibility
    /// </summary>
    [CustomEditor(typeof(EnemyVisiblity))]
    public class EnemyVisibilityEditor : Editor
    {
        private void OnSceneGUI()
        {
            // ref EnemyVisiblity we are looking at
            var visibility = target as EnemyVisiblity;

            // start to draw at 10% opacity
            Handles.color = new Color(1, 1, 1, 0.1f);

            // draw arc
            var forwardPointMinusHalfAngle = Quaternion.Euler(0, -visibility.angle / 2, 0) * visibility.transform.forward;
            Vector3 arcStart = forwardPointMinusHalfAngle * visibility.maxDistance;

            Handles.DrawSolidArc(visibility.transform.position, Vector3.up, arcStart, visibility.angle, visibility.maxDistance);
            Handles.color = Color.white;

            Vector3 handlePosition = visibility.transform.position + visibility.transform.forward * visibility.maxDistance;

            // draw handle and stor results
            visibility.maxDistance = Handles.ScaleValueHandle(visibility.maxDistance, handlePosition, visibility.transform.rotation, 1, Handles.ConeHandleCap, 0.25f);
        }
    }
#endif
}
