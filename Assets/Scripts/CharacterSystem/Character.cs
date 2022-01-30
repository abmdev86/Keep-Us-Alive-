using com.sluggagames.keepUsAlive.Core;
using UnityEngine;
using UnityEngine.AI;


namespace com.sluggagames.keepUsAlive.CharacterSystem
{
    [RequireComponent(typeof(NavMeshAgent))]

    public class Character : MonoBehaviour, IDisplayObject
    {
       
        int _id;
        public int Id
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
            int randomId = Random.Range(1, 100) * 1000;
            _id = randomId;
            

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
            if (characterHealth.IsDead) return;
            mover.destination = _destination;
            mover.speed = maxSpeed * Mathf.Clamp01(_speedFraction);
        }
    }

}
