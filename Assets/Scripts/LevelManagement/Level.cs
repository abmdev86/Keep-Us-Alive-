
using com.sluggagames.keepUsAlive.CharacterSystem;
using com.sluggagames.keepUsAlive.Obstacle;
using System.Collections.Generic;
using UnityEngine;

namespace com.sluggagames.keepUsAlive.LevelManagement
{
    [CreateAssetMenu(fileName = "Level", menuName = "Level", order = 99)]
    public class Level : ScriptableObject
    {
        [SerializeField] internal string levelName;
        [SerializeField] internal GameObject enviromentPrefab;
        [SerializeField] internal GameObject ActivationPadPrefab;
        internal Quest quest;
        [SerializeField] internal List<Survivor> survivorsToSpawn = new List<Survivor>();



        public GameObject GetActivationPad(Vector3 _spawnPos)
        {
            return Instantiate(ActivationPadPrefab, _spawnPos, Quaternion.identity);
        }

    }
}
