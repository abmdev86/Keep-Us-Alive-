using Cinemachine;
using com.sluggagames.keepUsAlive.CharacterSystem;
using com.sluggagames.keepUsAlive.Obstacle;
using com.sluggagames.keepUsAlive.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.sluggagames.keepUsAlive.Core
{
    public class GameManager : Singleton<GameManager>
    {

        Dictionary<string, Survivor> selectedSurvivors = new Dictionary<string, Survivor>();
        Dictionary<string, GameObject> selectedSurvivorPotraits = new Dictionary<string, GameObject>();

        bool hasCharacter = false;
        int levelKeyAmount = 0;
        [SerializeField] GameObject activationPadPrefab; // move to spawner

        [SerializeField] GameObject selectedCharPanel;
        [Tooltip("The max amount of characters the player can select at one time.")]
        [SerializeField] int maxSelectableCharacters = 20;
        [SerializeField] Text keyAmountText; // move to UI manager.

       


        private void Start()
        {
            
            keyAmountText.text = UpdateTextValue("Key(s):", 0);
        }
        private void Update()
        {
            // show character panel if we have a character to show.
            selectedCharPanel.SetActive(hasCharacter);
           
           // if the dictionary has 1 or more characters in it then we have a character.
            if (GetSelectedCount() > 0)
            {
                hasCharacter = true;
            }

            // right mouse clicking and having survivors selected will move them
            if (Input.GetMouseButtonDown(1) && GetSelectedCount() > 0)
            {
                MoveSelectedCharacters();
            }

            // if we click the ground we clear our selection if we have anything.
            if (Input.GetMouseButtonDown(0) && GetSelectedCount() > 0)
            {
                if (GetHitData().transform.gameObject.tag != "Ground") return;
                
                ClearSelection();
            }

        }
        public void IncreaseCurrentKeyValue(ActivationPad _key)
        {
            
            levelKeyAmount += _key.activationValue;
            keyAmountText.text = UpdateTextValue("Key", levelKeyAmount);
            StoreKeyAmount(levelKeyAmount);
        }

        public void DecreaseCurrentKeyValue(ActivationPad _key)
        {
            levelKeyAmount -= _key.activationValue;
            keyAmountText.text = UpdateTextValue("Key", levelKeyAmount);
        }
        void StoreKeyAmount(int _value)
        {
            PlayerPrefs.SetString("CurrentKeys", _value.ToString());
        }

        string UpdateTextValue(string msg, int v)
        {
            return $"{msg}: {v}";
        }

      

        /// <summary>
        ///  Gets the current ScreenPointToRay of the mousePosition
        /// </summary>
        /// <returns>hit.point as Vector3</returns>
        public Vector3 GetMousePosition()
        {
            Vector3 destination = Vector3.zero;
            RaycastHit hitData = GetHitData();
            destination = hitData.point;
        
            return destination;
        }

        /// <summary>
        /// Get the data point of where the mouse clicked.
        /// </summary>
        /// <returns></returns>
        public RaycastHit GetHitData()
        {
            RaycastHit _hitData;
            Ray _mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            return Physics.Raycast(_mouseRay, out _hitData) ? _hitData : _hitData;
        }




        void UpdatePanel(Survivor _survivor)
        {
            // we already have this character being displayed
            if (selectedSurvivorPotraits.ContainsKey(_survivor.Id.ToString()))
            {
                return;
            }

            GameObject imageObj = new GameObject(_survivor.Id.ToString(), typeof(Image));
            imageObj.transform.SetParent(selectedCharPanel.transform, false);
            imageObj.GetComponent<Image>().sprite = _survivor.CharacterIcon;
        
            selectedSurvivorPotraits.Add(imageObj.name, imageObj);

        }

        public void RemoveFromSelection(Survivor _survivorToRemove)
        {
            if (selectedSurvivors.ContainsKey(_survivorToRemove.Id))
            {
                GameObject _destroyThis = selectedSurvivorPotraits[_survivorToRemove.Id.ToString()];
                selectedSurvivorPotraits.Remove(_survivorToRemove.Id.ToString());
                Destroy(_destroyThis);
                selectedSurvivors.Remove(_survivorToRemove.Id);
                hasCharacter = selectedSurvivors.Count > 0;
            }
        }

        void ClearSelection()
        {

            if (GetSelectedCount() > 0)
            {
                selectedSurvivors.Clear();
                
                foreach (KeyValuePair<string, GameObject> portrait in selectedSurvivorPotraits)
                {

                    Destroy(portrait.Value.gameObject);


                }
                selectedSurvivorPotraits.Clear();
                hasCharacter = false;
            }
            else
            {
                return;
            }

        }

        void MoveSelectedCharacters()
        {
            if (selectedSurvivors.Count == 0)
            {
                Debug.LogWarning("Nothing to move", this);
                return;
            }
            foreach (KeyValuePair<string, Survivor> survivor in selectedSurvivors)
            {
                survivor.Value.MoveCharacter(GetMousePosition());
            }
        }
        public void AddSurviorToSelected(Survivor _survivor)
        {
            Survivor newSurvivor = _survivor;

            if (selectedSurvivors.Count == 0)
            {
                selectedSurvivors.Add(newSurvivor.Id, newSurvivor);
                UpdatePanel(newSurvivor);

            }
            else
            {
               
                ClearSelection();
                selectedSurvivors.Add(newSurvivor.Id, newSurvivor);
                UpdatePanel(newSurvivor);
            }
            if (selectedSurvivors.ContainsKey(newSurvivor.Id))
            {
                Debug.LogWarning("Character already selected ", this);
                return;
            }
            if (selectedSurvivors.Count >= maxSelectableCharacters)
            {
                Debug.LogWarning("Can't select anymore characters", this);
                return;
            }
           
        }

        public GameObject GetActivationPad(Vector3 _spawnPos)
        {
            return Instantiate(activationPadPrefab, _spawnPos, Quaternion.identity);
        }
        public int GetSelectedCount()
        {
            return selectedSurvivors.Count;
        }
    }
}
