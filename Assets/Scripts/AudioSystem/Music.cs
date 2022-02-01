using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sluggagames.keepUsAlive.AudioSystem
{
    [CreateAssetMenu]
    public class Music : ScriptableObject
    {
        public List<AudioClip> musicClips = new List<AudioClip>();


        public AudioClip GetMusicClipAtIndex(int _index)
        {
            if (IsEmpty()) return null;
            return musicClips[_index];

        }

        public AudioClip GetRandomClip()
        {
            if (IsEmpty()) return null;
            return musicClips[Random.Range(1, musicClips.Count)];
        }

        public AudioClip GetMainMenuMusic()
        {
            if (IsEmpty()) return null;
            return musicClips[0];
        }

        bool IsEmpty()
        {
            return musicClips.Count == 0;
        }
    }
}
