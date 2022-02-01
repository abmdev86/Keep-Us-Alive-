using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sluggagames.keepUsAlive.AudioSystem
{
    [CreateAssetMenu]
    public class SoundEffect : ScriptableObject
    {
        public AudioClip[] clips;

        public AudioClip GetRandomClip()
        {
        
            if (clips.Length == 0) return null;

            return clips[Random.Range(0, clips.Length)];
        
        }

        public AudioClip GetClipAtIndex(int _index)
        {
            if (clips.Length == 0) return null;
            return clips[_index];
        }

        public AudioClip GetClipByName(string _clipName)
        {
            AudioClip ac = null;
            if (clips.Length == 0) return null;
            foreach(AudioClip clip in clips)
            {
                if(clip.name == _clipName)
                {
                    ac = clip;
                }
                else
                {
                  
                    continue;
                }
            }
            return ac;
        }
    }
}
