using com.sluggagames.keepUsAlive.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using com.sluggagames.keepUsAlive.MenuSystem;

namespace com.sluggagames.keepUsAlive.Core
{
    public class GameSceneManager : Singleton<GameSceneManager>
    {
        [SerializeField] TMP_InputField playerNameInput;
        [SerializeField] float fadeInTime = 1f;
        [SerializeField] float fadeOutTime = 2f;
        [SerializeField] float fadeWaitTime = 0.5f;

        public void LoadLevelByIndex(int _index)
        {
            if(_index < 0)
            {
                Debug.LogError($"Unable able to load at index {_index}");
                return;
            }
            StartCoroutine(LoadLevelAsync(_index));
        }

        IEnumerator LoadLevelAsync(int _index)
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);

            // do back end stuff like saving

            yield return SceneManager.LoadSceneAsync(_index);

            // load anything needed for the level.
            yield return new WaitForSeconds(fadeWaitTime);

            yield return fader.FadeIn(fadeInTime);
        }

        private void OnLevelWasLoaded(int level)
        {
            print(level);
        }
    }
}
