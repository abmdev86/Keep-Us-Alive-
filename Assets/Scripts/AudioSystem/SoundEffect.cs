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
            if (clips.Length == 0)
            {
                return null;

            }
            return clips[Random.Range(0, clips.Length)];
        }
    }
}
