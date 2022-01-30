using com.sluggagames.keepUsAlive.CharacterSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sluggagames.keepUsAlive.Utils
{
    public static class GameUtility 
    {
        
        public static string GetID(this Character character)
        {
            Hash128 hash = new Hash128();
            int randomId = Random.Range(1, 100) * 1000;
            hash.Append(randomId);
            hash.Append(character.CharacterIcon.ToString());
            return hash.ToString();
        }
    }
}
