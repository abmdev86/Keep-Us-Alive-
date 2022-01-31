using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
//using static com.sluggagames.keepUsAlive.LevelManagement.Quest;

namespace com.sluggagames.keepUsAlive.LevelManagement
{
    /// <summary>
    /// Represents the player's current progress through a quest
    /// </summary>
    public class QuestStatus
    {
        /// <summary>
        /// Data that describes the quest created as a scriptable object in the editor
        /// </summary>
        public Quest questData;

        /// <summary>
        /// map objective numbers to their status
        /// </summary>
        public Dictionary<int, Quest.Status> objectiveStatues;

        public QuestStatus(Quest _questData)
        {
            this.questData = _questData;
            objectiveStatues = new Dictionary<int, Quest.Status>();
            for (int i = 0; i < questData.Objectives.Count; i++)
            {
                var objectiveData = questData.Objectives[i];
                objectiveStatues[i] = objectiveData.initialStatus;
            }
        }
        public Quest.Status questStatus
        {
            get
            {
                for (int i = 0; i < questData.Objectives.Count; i++)
                {
                    var objectiveData = questData.Objectives[i];

                    // optional quest do not matter
                    if (objectiveData.optional) continue;
                    var objectiveStatus = objectiveStatues[i];

                    if (objectiveStatus == Quest.Status.Failed)
                    {
                        // mandatory objective failed so quest failed.
                        return Quest.Status.Failed;
                    }
                    else if (objectiveStatus != Quest.Status.Complete)
                    {
                        return Quest.Status.NotYetComplete;
                    }
                }

                // all objectives that are required are complete so the quest is complete.

                return Quest.Status.Complete;
            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < questData.Objectives.Count; i++)
            {
                var objectiveData = questData.Objectives[i];
                var objectiveStatus = objectiveStatues[i];

                // do not show uncompleted hidding objectives
                if (objectiveData.isVisible == false && objectiveStatus == Quest.Status.NotYetComplete)
                {
                    continue;
                }
                // display optional after the name of the objective if it is optional
                if (objectiveData.optional)
                {
                    stringBuilder.AppendFormat("{0} (Optional) - {1}\n",
                        objectiveData.name, objectiveStatus.ToString());

                }
                else
                {
                    stringBuilder.AppendFormat("{0} - {1}\n", objectiveData.name, objectiveStatus.ToString());
                }
            }

            // adds blank line
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("Status: {0}", this.questStatus.ToString());

            return stringBuilder.ToString();
        }

    }

    public class QuestManager : MonoBehaviour
    {
        // quest that starts when game starts
        [SerializeField] Quest startingQuest = null;
        [SerializeField] Text objectiveSummary = null;
        QuestStatus activeQuest;
        private void Start()
        {
            if (startingQuest != null)
            {
                StartQuest(startingQuest);
            }
        }

        public void StartQuest(Quest _quest)
        {
            activeQuest = new QuestStatus(_quest);
            UpdateObjectiveSummaryText();
            Debug.Log($"Started quest {activeQuest.questData.questName}");
        }

        void UpdateObjectiveSummaryText()
        {
            string label;
            if (activeQuest == null)
            {
                label = "No active quest";

            }
            else
            {
                label = activeQuest.ToString();
            }
            objectiveSummary.text = label;
        }

        public void UpdateObjectiveStatus(Quest _quest, int _objectiveNumber, Quest.Status _status)
        {
            
            if (activeQuest == null)
            {
                Debug.LogError("Missing a Quest", this);
                return;
            }
            if (activeQuest.questData != _quest)
            {
                Debug.LogWarning($"Attempted to set an objective status for quest{_quest.name}, but this \n is not the active quest. Ignoring...");
                return;
            }
          
            // update object status
            activeQuest.objectiveStatues[_objectiveNumber] = _status;
            UpdateObjectiveSummaryText();
        }
    }
}
