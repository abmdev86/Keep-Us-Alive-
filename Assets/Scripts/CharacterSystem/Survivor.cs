using com.sluggagames.keepUsAlive.Core;
using com.sluggagames.keepUsAlive.Obstacle;
using com.sluggagames.keepUsAlive.Utils;
using UnityEngine;

namespace com.sluggagames.keepUsAlive.CharacterSystem
{
    public class Survivor : Character, ISelectable
    {
      // internal int keyAmount = 0;
        [SerializeField]
        internal bool hasKey = false;
        internal ActivationPad key;

        protected override void Awake()
        {
            base.Awake();
          
            print($"Survivor {_id} created");
        }
        private void Update()
        {
            CheckForKeys();

        }

        private void CheckForKeys()
        {
            if (key)
            {
                hasKey = true;
            }
            else
            {
                hasKey = false;
            }
        }

        private void OnMouseDown()
        {
           
            if (characterHealth.IsDead) return;
            AddToSelectedObjects(this);
            // add an effect
        }
        public void AddToSelectedObjects(Survivor _survivor)
        {
            if (_survivor.Id == null) return;
            GameManager.Instance.AddCharacterToSelected(_survivor);
        }

        public void AddKey(ActivationPad _key)
        {
            if (hasKey) return;
            GameManager.Instance.IncreaseCurrentKeyValue(_key);
            key = _key;
        }
        
        public void RemoveKey()
        {
            if (!hasKey) return;
            GameManager.Instance.DecreaseCurrentKeyValue(key);
            key = null;
        }

        public void DropKey(ActivationPad _key)
        {
            if (!hasKey) return;
            GameManager.Instance.DecreaseCurrentKeyValue(_key);
           GameObject keyObject = GameManager.Instance.GetActivationPad(transform.position);
            ActivationPad _newKey = keyObject.GetComponent<ActivationPad>();
            _newKey.activationValue = _key.activationValue;
            _newKey.activationTime = _key.activationTime;

        }

        public void Selected()
        {
            Debug.Log($"{this.Id} was selected");
        }
    }
}
