using com.sluggagames.keepUsAlive.Core;
using com.sluggagames.keepUsAlive.Utils;
using UnityEngine;
using UnityEngine.AI;


namespace com.sluggagames.keepUsAlive.CharacterSystem
{
    [RequireComponent(typeof(NavMeshAgent))]

    public class Character : MonoBehaviour
    {
       
       internal string _id;
        public string Id
        {
            get
            {
                return _id;
            }
        }
        NavMeshAgent mover;
        [SerializeField] internal GameObject characterUIPrefab;

        [SerializeField] Sprite characterIcon;
        public Sprite CharacterIcon
        {
            get
            {
                return characterIcon;
            }
        }

        [SerializeField] float maxSpeed;
        [SerializeField] internal Health characterHealth;
       
   

        protected virtual void Awake()
        {
            mover = GetComponent<NavMeshAgent>();
            characterHealth = GetComponent<Health>();
            _id = this.GetID();
            this.gameObject.name = "Character " +_id;


        }
        private void Start()
        {
            if(characterUIPrefab != null)
            {
                GameObject _characterUIGO = Instantiate(characterUIPrefab);
                DisplayData _characterUI = _characterUIGO.GetComponent<DisplayData>();
                _characterUI.SetTarget(this.characterHealth);
            }
        }
       

      
     

        public void MoveCharacter(Vector3 _destination, float _speedFraction = 1)
        {
            if (characterHealth.IsDead) return;
            mover.destination = _destination;
            mover.speed = maxSpeed * Mathf.Clamp01(_speedFraction);
        }
    }

}
