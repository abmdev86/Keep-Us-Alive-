using com.sluggagames.keepUsAlive.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace com.sluggagames.keepUsAlive.CharacterSystem
{
    [RequireComponent(typeof(NavMeshAgent))]

    public class Character : MonoBehaviour
    {
        [SerializeField]
        int _id;
        NavMeshAgent mover;
        public int Id
        {
            get
            {
                return _id;
            }
        }
        [SerializeField] float health = 1;
        [SerializeField] bool _isDead;
        public bool IsDead
        {
            get
            {
                if (health <= 0)
                {
                    return _isDead = true;
                }
                else
                {
                    return _isDead = false;
                }
            }
        }
        [SerializeField] float maxSpeed;
        [SerializeField]
        Sprite characterIcon;
        public Sprite CharacterIcon
        {
            get
            {
                return characterIcon;
            }
        }

        protected virtual void Awake()
        {
            mover = GetComponent<NavMeshAgent>();
            int randomId = Random.Range(1, 100) * 1000;
            _id = randomId;

        }

        private void OnMouseDown()
        {
            if (this.gameObject.tag == "Enemy") return;
            AddToSelectedObjects(this);
            // add an effect
        }

        public void AddToSelectedObjects(Character _character)
        {
            if (_character.Id < 0) return;
            GameManager.Instance.AddCharacterToSelected(_character);
        }

        public void MoveCharacter(Vector3 _destination, float _speedFraction = 1)
        {
            if (_isDead) return;
            mover.destination = _destination;
            mover.speed = maxSpeed * Mathf.Clamp01(_speedFraction);
        }
    }

}
