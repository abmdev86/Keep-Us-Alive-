using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sluggagames.keepUsAlive.MenuSystem
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup _canvasGroup;

        private void Awake()
        {
            try
            {
                _canvasGroup = GetComponentInChildren<CanvasGroup>();
            }
            catch (MissingReferenceException ex )
            {

                Debug.LogError(ex.Message);
                GameObject go = new GameObject("_errCanvas");
                go.AddComponent<CanvasGroup>();
                go.transform.SetParent(transform);
                return;
            }
            
           
        }
        public void FadeOutImmediately()
        {
            _canvasGroup.alpha = 1;

        }

        public IEnumerator FadeOut(float _time)
        {
            while(_canvasGroup.alpha < 1)
            {
                _canvasGroup.alpha += Time.deltaTime / _time;
                yield return null;
            }
        }
        public IEnumerator FadeIn(float _time)
        {
            while (_canvasGroup.alpha > 0)
            {
                _canvasGroup.alpha -= Time.deltaTime / _time;
                yield return null;
            }
        }
    }
}
