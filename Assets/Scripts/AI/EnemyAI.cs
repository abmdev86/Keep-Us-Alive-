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
        EnemyStates _currentState = EnemyStates.Patrol;
        EnemyStates _previousState;
        GameObject[] _targets;
        GameObject _target;
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

        [Tooltip("How close does target have to be in order to start trying to get in attack range?")]
        [SerializeField] float pursuitDistance = 4f;
        [Tooltip("How close does my target need to be to make me aware?")]
        [SerializeField] float awareDistance = 6f;

        [SerializeField] PatrolPath _patrolPath;
        [Tooltip("How close to the point until considered to have arrived?")]
        [SerializeField] float _pointTolerance;
        [Tooltip("How long should I wait at the waypoint?")]
        [SerializeField] float _pointDwellTime;
        [Tooltip("How slow should I patrol compared to actual movement speed?")]
        [Range(0, 1)]
        [SerializeField] float _patrolSpeedFraction = 0.2f;
        int _currentWaypointIndex = 0;
        int _pointIndex = 0;
        Vector3 originalPos;
        float _timeSinceLastSeenSurvivor = Mathf.Infinity;
        float _timeSinceArrivedAtPoint = Mathf.Infinity;
        [SerializeField] float _awarnessTime = 3f;

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
        }

        private void Start()
        {
            originalPos = transform.position;
            _previousState = _currentState;
            _targets = GameObject.FindGameObjectsWithTag("Player");
            if (_targets.Length <= 0)
            {
                Debug.LogWarning("No targets found", this);
            }
        }

        private void Update()
        {
            if (_enemy.IsDead) return;
            UpdateTarget();
            UpdateState();

            switch (_currentState)
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

            }
            UpdateTimers();

        }

        void UpdateState()
        {
            EnemyStates cachedState;
            if (_target && !IsAware(_target))
            {
                _target = null;
            }

            if (_target == null)
            {
                cachedState = _currentState;
                _currentState = EnemyStates.Patrol;
                _previousState = cachedState;
                print($"Changed from {_previousState} to {_currentState}");
            }
            else if (_target && InAttackRange(_target))
            {
                cachedState = _currentState;
                _currentState = EnemyStates.Attack;
                _previousState = cachedState;
                print($"Changed from {_previousState} to {_currentState}");

            }
            else if (_target && InPursuitRange(_target))
            {
                cachedState = _currentState;
                _currentState = EnemyStates.Pursuit;
                _previousState = cachedState;
                print($"Changed from {_previousState} to {_currentState}");
            }
            else if (_timeSinceLastSeenSurvivor < _awarnessTime && IsAware(_target))
            {
                cachedState = _currentState;
                _currentState = EnemyStates.Aware;
                _previousState = cachedState;
                print($"Changed from {_previousState} to {_currentState}");

            }
        }

        void UpdateTarget()
        {
            if (_target != null) return;
            for (int i = 0; i < _targets.Length; i++)
            {
                if (_target == _targets[i]) return;
                if (InAttackRange(_targets[i]))
                {
                    _target = _targets[i];

                }
            }
        }

        void UpdateTimers()
        {
            _timeSinceArrivedAtPoint += Time.deltaTime;
            _timeSinceLastSeenSurvivor += Time.deltaTime;
        }
        void PatrolState()
        {

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
            _enemy.MoveCharacter(originalPos);

        }
        void PursuitState()
        {
            _enemy.MoveCharacter(_target.transform.position);

        }
        void AttackState()
        {
            _timeSinceLastSeenSurvivor = 0;
            _enemy.MoveCharacter(_target.transform.position);

        }

        Vector3 GetCurrentWaypoint()
        {
            return _patrolPath.GetWaypointPos(_currentWaypointIndex);

        }
        void CycleWaypoint()
        {


            _currentWaypointIndex = _patrolPath.GetNextIndex(_currentWaypointIndex);
        }

        bool InAttackRange(GameObject _target)
        {
            float distanceToTarget = GetDistanceToTarget(_target);
            return distanceToTarget < attackDistance;

        }

        bool InPursuitRange(GameObject _target)
        {
            float distanceToTarget = GetDistanceToTarget(_target);
            return distanceToTarget < pursuitDistance && distanceToTarget > attackDistance;
        }
        bool IsAware(GameObject _target)
        {
            float distanceToTarget = GetDistanceToTarget(_target);
            return distanceToTarget < awareDistance;
        }
        bool AtPoint()
        {
            float distanceToWaypoint = GetDistanceToPoint(GetCurrentWaypoint());

            return distanceToWaypoint < _pointTolerance;
        }
        float GetDistanceToTarget(GameObject _target)
        {
            return Vector3.Distance(transform.position, _target.transform.position);
        }
        float GetDistanceToPoint(Vector3 _point)
        {
            return Vector3.Distance(transform.position, _point);
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDistance);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, pursuitDistance);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, awareDistance);
        }

    }

    /// <summary>
    /// Handles for change range of AttackDistance
    /// </summary>
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

}
