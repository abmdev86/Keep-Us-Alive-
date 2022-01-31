using com.sluggagames.keepUsAlive.Core;
using com.sluggagames.keepUsAlive.LevelManagement;
using com.sluggagames.keepUsAlive.Obstacle;
using com.sluggagames.keepUsAlive.Utils;
using UnityEngine;

namespace com.sluggagames.keepUsAlive.CharacterSystem
{
    public class Survivor : Character
    {
     
        protected override void Awake()
        {
            base.Awake();
  
        }
        private void Update()
        {
           

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
            GameManager.Instance.AddSurviorToSelected(_survivor);
        }

        //public void AddKey(ActivationPad _key)
        //{
        //    if (hasKey) return;
        //    //   GameManager.Instance.IncreaseCurrentKeyValue(_key);
        //    key = _key;
        //}

        //public void RemoveKey()
        //{
        //    if (!hasKey) return;
        //    // GameManager.Instance.DecreaseCurrentKeyValue(key);
        //    key = null;
        //}

      


    }
}
