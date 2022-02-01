using com.sluggagames.keepUsAlive.CharacterSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace com.sluggagames.keepUsAlive.AI
{
    enum EnemyStates
    {
        Patrol,
        Aware,
        Pursuit,
        Attack

    }
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] EnemyStates _currentState = EnemyStates.Patrol;
        [SerializeField] EnemyStates _previousState;
        EnemyVisiblity visiblity;
        Enemy _enemy;
       

        [Tooltip("How close does the target need to be in order for me to engage?")]
        [SerializeField] float attackDistance = 2f;
        public float AttackDistance
        {
            get
            {
                return attackDistance;
            }
            set
            {
                attackDistance = value;
            }
        }


        [SerializeField] PatrolPath _patrolPath;
        [Tooltip("How close to the point until considered to have arrived?")]
        [SerializeField] float _pointTolerance;
        [Tooltip("How long should I wait at the waypoint?")]
        [SerializeField] float _pointDwellTime;

        [Tooltip("How slow should I patrol compared to actual movement speed?")]
        [Range(0, 1)]
        [SerializeField] float _patrolSpeedFraction = 0.2f;
        int _currentWaypointIndex = 0;
        Vector3 originalPos;
        float _timeSinceLastSeenSurvivor;
        float _timeSinceArrivedAtPoint;
        


        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
            visiblity = GetComponent<EnemyVisiblity>();

        }

        private void Start()
        {
            originalPos = transform.position;
            _previousState = _currentState;
        
        }

        private void Update()
        {
           
            if (_enemy.characterHealth.IsDead) return;
           if(visiblity.target == null)
            {
                if (!visiblity.UpdateTarget())
                {
                    _currentState = EnemyStates.Patrol;

                }
                return;
            }

            if (visiblity == null)
            {
                Debug.LogError("Missing visibility", this);
            }
           
            if (visiblity.targetIsVisible)
            {

                _previousState = _currentState;
                _currentState = EnemyStates.Pursuit;
            }
            else if (!visiblity.targetIsVisible || visiblity.target == null)
            {
                _previousState = _currentState;
                _currentState = EnemyStates.Patrol;
            }


            UpdateState(_currentState);

            UpdateTimers();
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                if (other.gameObject.transform == visiblity.transform) return;
                visiblity.SetTarget(other.gameObject.transform);
            }
        }


        void UpdateState(EnemyStates currentState)
        {
         
            switch (currentState)
            {
                case EnemyStates.Patrol:
                    PatrolState();
                    break;
                case EnemyStates.Aware:
                    AwareState();
                    break;
                case EnemyStates.Pursuit:
                    PursuitState();
                    break;
                case EnemyStates.Attack:
                   
                    AttackState();
                    break;
                default:
                    PatrolState();
                    break;


            }
        }
        void UpdateTimers()
        {
            _timeSinceArrivedAtPoint += Time.deltaTime;
            _timeSinceLastSeenSurvivor += Time.deltaTime;
        }
        void PursuitState()
        {
            if (_currentState != EnemyStates.Pursuit) return;
            if (visiblity.target == null)
            {
                _previousState = _currentState;
                _currentState = EnemyStates.Patrol;
              

            }

            if (_currentState == EnemyStates.Pursuit && !visiblity.targetIsVisible)
            {
                _previousState = _currentState;
                _currentState = EnemyStates.Aware;
                return;
            }
            if (InAttackRange())
            {
                _previousState = _currentState;
                _currentState = EnemyStates.Attack;
                UpdateState(_currentState);
                return;
            }
            
           if(visiblity.target)
                _enemy.MoveCharacter(visiblity.target.position);

        }



        void PatrolState()
        {

            if (visiblity.targetIsVisible) return;
            Vector3 nextPos = originalPos;
            if (_patrolPath != null)
            {
                if (AtPoint())
                {

                    _timeSinceArrivedAtPoint = 0;
                    CycleWaypoint();
                }
                nextPos = GetCurrentWaypoint();
            }
            if (_timeSinceArrivedAtPoint > _pointDwellTime)
            {
                _enemy.MoveCharacter(nextPos, _patrolSpeedFraction);
            }

        }
        void AwareState()
        {
            if (InAttackRange()) return;
            _enemy.MoveCharacter(transform.position);
            if (visiblity.targetIsVisible)
            {
                _previousState = _currentState;
                _currentState = EnemyStates.Pursuit;
            }
            else
            {
                _previousState = _currentState;
                _currentState = EnemyStates.Patrol;
            }



        }

        void AttackState()
        {
            if (_currentState != EnemyStates.Attack) return;
            _timeSinceLastSeenSurvivor = 0;
            
            Health _attackTarget = visiblity.target.GetComponent<Health>();
         
            //_enemy.MoveCharacter(_attackTarget.transform.position);
            if (!InAttackRange() && visiblity.targetIsVisible)
            {
                _previousState = _currentState;
                _currentState = EnemyStates.Pursuit;
                return;

            }
            else if (!InAttackRange() && !visiblity.targetIsVisible)
            {
                _previousState = _currentState;
                _currentState = EnemyStates.Aware;
                return;
            }
            _enemy.Attack(_attackTarget);

        }


        Vector3 GetCurrentWaypoint()
        {
            return _patrolPath.GetWaypointPos(_currentWaypointIndex);

        }
        void CycleWaypoint()
        {


            _currentWaypointIndex = _patrolPath.GetNextIndex(_currentWaypointIndex);
        }

        bool InAttackRange()
        {
            if (visiblity.target == null) return false;
            float distanceToTarget = GetDistanceToTarget(visiblity.target);
            return distanceToTarget < attackDistance;

        }

     


        bool AtPoint()
        {
            float distanceToWaypoint = GetDistanceToPoint(GetCurrentWaypoint());

            return distanceToWaypoint < _pointTolerance;
        }
        float GetDistanceToTarget(Transform _target)
        {
            return Vector3.Distance(transform.position, _target.position);
        }
        float GetDistanceToPoint(Vector3 _point)
        {
            return Vector3.Distance(transform.position, _point);
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDistance);
            

        }

    }

    /// <summary>
    /// Handles for change range of AttackDistance
    /// </summary>
#if UNITY_EDITOR
    [CustomEditor(typeof(EnemyAI))]
    [CanEditMultipleObjects]
    public class AIEditor : Editor
    {


        protected virtual void OnSceneGUI()
        {
            EnemyAI enemyAI = (EnemyAI)target;
            if (enemyAI == null)
            {
                return;
            }
            Handles.color = Color.red;
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.green;

            Vector3 pos = enemyAI.transform.position + Vector3.up * 2f;
            string posString = pos.ToString();
            Handles.Label(pos, posString + "\nAttackArea: " + enemyAI.AttackDistance.ToString(), style);

            Handles.BeginGUI();
            if (GUILayout.Button("Reset area", GUILayout.Width(100)))
            {
                enemyAI.AttackDistance = 2f;
            }
            Handles.EndGUI();

            Handles.DrawWireArc(enemyAI.transform.position, enemyAI.transform.up, -enemyAI.transform.right, 180, enemyAI.AttackDistance);
            enemyAI.AttackDistance =
                Handles.ScaleValueHandle(enemyAI.AttackDistance, enemyAI.transform.position + enemyAI.transform.forward * enemyAI.AttackDistance, enemyAI.transform.rotation, 1, Handles.ConeHandleCap, 1);
        }



    }
#endif

}
