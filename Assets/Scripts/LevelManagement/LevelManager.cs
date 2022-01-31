
using com.sluggagames.keepUsAlive.Obstacle;
using UnityEngine;
using UnityEngine.UI;

namespace com.sluggagames.keepUsAlive.LevelManagement
{
    public class LevelManager : MonoBehaviour
    {

        [SerializeField] internal Level levelData;
        [SerializeField] GameObject spawnPointPrefab;

      
        private void Awake()
        {
            if (!levelData)
            {
                Debug.LogError("Missing levelData for levelManager", this);
            }
            Instantiate(levelData.enviromentPrefab, Vector3.zero, Quaternion.identity);
        }

    }
}
