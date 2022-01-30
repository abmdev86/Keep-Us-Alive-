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

        Dictionary<int, Character> _selectedCharacters = new Dictionary<int, Character>();
        Dictionary<string, GameObject> _selectedCharPortraits = new Dictionary<string, GameObject>();

        CinemachineFreeLook virtualCam;
        CamTracker camTracker;
        Character mainSurvivor;

        bool hasCharacter = false;
        int levelKeyAmount = 0;
        [SerializeField] GameObject activationPadPrefab;

        [SerializeField] GameObject selectedCharPanel;
        [Tooltip("The max amount of characters the player can select at one time.")]
        [SerializeField] int maxSelectableCharacters = 20;
        [SerializeField] Text keyAmountText;

       

        protected override void Awake()
        {
            base.Awake();
            virtualCam = GameObject.FindObjectOfType<CinemachineFreeLook>();
            camTracker = FindObjectOfType<CamTracker>();


        }

        private void Start()
        {
            UpdateCamera(camTracker.transform);
            keyAmountText.text = UpdateTextValue("Key(s):", 0);
        }
        private void Update()
        {

            selectedCharPanel.SetActive(hasCharacter);
            if (hasCharacter && mainSurvivor)
            {
                
                UpdateCamera(mainSurvivor.transform);
            }
            else
            {
                UpdateCamera(camTracker.transform);
            }
            if (GetSelectedCount() > 0)
            {
                hasCharacter = true;
            }

            if (Input.GetMouseButtonDown(1) && GetSelectedCount() > 0)
            {
                MoveSelectedCharacters();
            }
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
        /// Updates the virtual camera's LookAt and Follow targets.
        /// </summary>
        /// <param name="_transform">the target's transform</param>
        private void UpdateCamera(Transform _transform)
        {
            virtualCam.LookAt = _transform.transform;
            virtualCam.Follow = _transform.transform;

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
            //var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            //bool hit = Physics.Raycast(mouseRay, out hitData);
            //if (hitData)
            //{
            //    if (hitData.transform.gameObject.tag == "Ground")
            //        destination = hitData.point;
            //}
            return destination;
        }

        public RaycastHit GetHitData()
        {
            RaycastHit _hitData;
            Ray _mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            return Physics.Raycast(_mouseRay, out _hitData) ? _hitData : _hitData;
        }




        void UpdatePanel(Character _character)
        {
            // we already have this character being displayed
            if (_selectedCharPortraits.ContainsKey(_character.Id.ToString()))
            {
                return;
            }

            GameObject imageObj = new GameObject(_character.Id.ToString(), typeof(Image));
            imageObj.transform.SetParent(selectedCharPanel.transform, false);
            imageObj.GetComponent<Image>().sprite = _character.CharacterIcon;
            //if (_selectedCharPortraits.ContainsKey(imageObj.name))
            //{
            //    Debug.LogWarning("Already added portrait for " + imageObj.name);
            //}
            _selectedCharPortraits.Add(imageObj.name, imageObj);

        }

        public void RemoveFromSelection(Character _charToRemove)
        {
            if (_selectedCharacters.ContainsKey(_charToRemove.Id))
            {
                GameObject _destroyThis = _selectedCharPortraits[_charToRemove.Id.ToString()];
                _selectedCharPortraits.Remove(_charToRemove.Id.ToString());
                Destroy(_destroyThis);
                _selectedCharacters.Remove(_charToRemove.Id);
                hasCharacter = _selectedCharacters.Count > 0;
            }
        }

        void ClearSelection()
        {

            if (GetSelectedCount() > 0)
            {
                _selectedCharacters.Clear();
                UpdateCamera(camTracker.transform);
                foreach (KeyValuePair<string, GameObject> portrait in _selectedCharPortraits)
                {

                    Destroy(portrait.Value.gameObject);


                }
                _selectedCharPortraits.Clear();
                hasCharacter = false;
            }
            else
            {
                return;
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
            if(GetSelectedCount() == 0)
            {
                mainSurvivor = _character;
            }
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

        public GameObject GetActivationPad(Vector3 _spawnPos)
        {
            return Instantiate(activationPadPrefab, _spawnPos, Quaternion.identity);
        }
        public int GetSelectedCount()
        {
            return _selectedCharacters.Count;
        }
    }
}
