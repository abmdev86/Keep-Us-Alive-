
using com.sluggagames.keepUsAlive.Core;
using UnityEngine;


namespace com.sluggagames.keepUsAlive.LevelManagement
{
    public class LevelManager : MonoBehaviour
    {

        [SerializeField] internal Level levelData;
        [SerializeField] GameObject spawnPointPrefab;
        [SerializeField]
        internal bool hasWon = false;
        internal bool hasLost = false;
        QuestManager questManager;


      
        private void Awake()
        {
            if (!levelData)
            {
                Debug.LogError("Missing levelData for levelManager", this);
            }
            questManager = FindObjectOfType<QuestManager>();
            if(questManager == null)
            {
                Debug.LogError("missing quest manager", this);
            }
            Instantiate(levelData.enviromentPrefab, Vector3.zero, Quaternion.identity);
        }

        private void Update()
        {
            
            if(questManager.activeQuest.questStatus == Quest.Status.Complete)
            {
                print("won");

                hasWon = true;
            }else if(questManager.activeQuest.questStatus == Quest.Status.Failed || !GameManager.Instance.CheckForSurvivors())
            {
                print("lost");
                hasLost = true;
                GameManager.Instance.GameOver();
            }
        }

    }
}
