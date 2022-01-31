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

       

      


    }
}
