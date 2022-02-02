using com.sluggagames.keepUsAlive.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sluggagames.keepUsAlive.AudioSystem
{
    public class AudioManager : Singleton<AudioManager>
    {
       public SoundEffect[] effects;
        public Music[] musicClips;
        AudioSource _source;
        Dictionary<string, SoundEffect> _effectDictionary;
        Dictionary<string, Music> _musicDictionary;
        AudioListener _listener;


        protected override void Awake()
        {
            base.Awake();
            _source = GetComponentInChildren<AudioSource>();
            _effectDictionary = new Dictionary<string, SoundEffect>();
            foreach(SoundEffect effect in effects)
            {
                _effectDictionary[effect.name] = effect;
            }
            _musicDictionary = new Dictionary<string, Music>();
            foreach(Music music in musicClips)
            {
                print($"{music.name}");
                _musicDictionary[music.name] = music;
            }
        }
        
        /// <summary>
        /// Plays a sound by name, at the same position as the audio listener.
        /// </summary>
        /// <param name="_effectName"></param>
        /// <param name="_worldPosition"></param>
        public void PlayEffect(string _effectName)
        {
            // if no listener find one to use
            CheckForListener();

            PlayEffect(_effectName, _listener.transform.position);
        }

        private void CheckForListener()
        {
            if (_listener == null)
            {
                _listener = FindObjectOfType<AudioListener>();
            }
        }

        public void UIConfirmationSound()
        {
            CheckForListener();
          
            PlaySpecificEffect("Confirm", _listener.transform.position);
        }

        public void PlaySpecificEffect(string _effectName, Vector3 _worldPosition)
        {
            if (_effectDictionary.ContainsKey(_effectName) == false)
            {
                Debug.LogWarning($"Effect {_effectName} is not registered");
            }
            var clip = _effectDictionary[_effectName].GetClipAtIndex(19);
            if (!clip)
            {
                Debug.LogWarning($"Effect {_effectName} has no clips to play");
                return;
            }
            
            AudioSource.PlayClipAtPoint(clip, _worldPosition);
        }

        public void PlayEffect(string _effectName, Vector3 _worldPosition)
        {
            if(_effectDictionary.ContainsKey(_effectName) == false)
            {
                Debug.LogWarning($"Effect {_effectName} is not registered");
            }

            var clip = _effectDictionary[_effectName].GetRandomClip();
            if (!clip)
            {
                Debug.LogWarning($"Effect {_effectName} has no clips to play");
                return;
            }
            AudioSource.PlayClipAtPoint(clip, _worldPosition);
        }

        public void PlayMenuMusic()
        {
            if (_listener == null)
            {
                _listener = FindObjectOfType<AudioListener>();
            }
            if (!_musicDictionary.ContainsKey("Music")) return;
         
            MenuMusic("Music");
        }

        void MenuMusic(string _musicName)
        {
            if (_listener == null)
            {
                _listener = FindObjectOfType<AudioListener>();
            }
            
            var clip = _musicDictionary[_musicName].GetMainMenuMusic();
            if (!clip) return;
            _source.clip = clip;
            _source.Play();
        }

        public void PlayMusic(string _musicName)
        {
            if(_listener == null)
            {
                _listener = FindObjectOfType<AudioListener>();
            }
            PlayMusicFromSource(_musicName);
        }

         void PlayMusicFromSource(string _musicName)
        {
            if(_musicDictionary.ContainsKey(_musicName) == false)
            {
                Debug.LogWarning($"Music {_musicName} has no music clips to play");

            }
            var clip = _musicDictionary[_musicName].GetRandomClip();
            if (!clip)
            {
                Debug.LogWarning($"Music {_musicName} has no clips to play");
                return;
            }
            _source.clip = clip;
            _source.Play();
        }
    }
}
