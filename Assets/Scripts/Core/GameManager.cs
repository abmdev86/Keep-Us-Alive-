using com.sluggagames.keepUsAlive.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.sluggagames.keepUsAlive.Core
{
    public class GameManager : Singleton<GameManager>
    {

        Dictionary<int, Character> _selectedCharacters = new Dictionary<int, Character>();
        Dictionary<string, GameObject> _selectedCharPortraits = new Dictionary<string, GameObject>();

        [SerializeField] GameObject selectedCharPanel;
        [SerializeField]
        [Tooltip("The max amount of characters the player can select at one time.")]
        int maxSelectableCharacters = 20;
        bool hasCharacter = false;
        public Vector3 GetMousePosition()
        {


            Vector3 destination = Vector3.zero;
            RaycastHit hitData;
            var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hit = Physics.Raycast(mouseRay, out hitData);
            if (hit)
            {
                if (hitData.transform.gameObject.tag == "Ground")
                    destination = hitData.point;
            }
            print(destination);
            return destination;
        }

        public RaycastHit GetHitData()
        {
            RaycastHit _hitData;
            Ray _mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            return Physics.Raycast(_mouseRay, out _hitData) ? _hitData : _hitData;
        }


        private void Update()
        {
            selectedCharPanel.SetActive(hasCharacter);
            if (_selectedCharacters.Count > 0)
            {
                hasCharacter = true;
            }
         
            if (Input.GetMouseButtonDown(1))
            {
                MoveSelectedCharacters();
            }
            if (Input.GetMouseButtonDown(0))
            {
                ClearSelection();
            }

        }

        void UpdatePanel(Character _character)
        {
            if (_selectedCharPortraits.ContainsKey(_character.Id.ToString()))
            {
                return;
            }
           
                GameObject imageObj = new GameObject(_character.Id.ToString(), typeof(Image));
                imageObj.transform.SetParent(selectedCharPanel.transform, false);
                imageObj.GetComponent<Image>().sprite = _character.CharacterIcon;
            if (_selectedCharPortraits.ContainsKey(imageObj.name))
            {
                Debug.LogWarning("Already added portrait for " + imageObj.name);
            }
                _selectedCharPortraits.Add(imageObj.name, imageObj);
            
           


        }

        void ClearSelection()
        {
            if (GetHitData().transform.gameObject.tag != "Ground") return;
            if (_selectedCharacters.Count > 0)
            {
                _selectedCharacters.Clear();
                foreach(KeyValuePair<string, GameObject> portrait in _selectedCharPortraits)
                {
                    
                    Destroy(portrait.Value.gameObject);
                   
                   
                }
                _selectedCharPortraits.Clear();
                hasCharacter = false;
            }
            
        }

        void MoveSelectedCharacters()
        {
            if (_selectedCharacters.Count == 0)
            {
                Debug.LogWarning("Nothing to move", this);
                return;
            }
            foreach (KeyValuePair<int, Character> character in _selectedCharacters)
            {
                character.Value.MoveCharacter(GetMousePosition());
            }
        }
        public void AddCharacterToSelected(Character _character)
        {
            if (_selectedCharacters.ContainsKey(_character.Id))
            {
                Debug.LogWarning("Character already selected ", this);
                return;
            }
            if (_selectedCharacters.Count >= maxSelectableCharacters)
            {
                Debug.LogWarning("Can't select anymore characters", this);
                return;
            }
            _selectedCharacters.Add(_character.Id, _character);
            UpdatePanel(_character);
        }

        public int GetSelectedCount()
        {
            return _selectedCharacters.Count;
        }
    }
}
