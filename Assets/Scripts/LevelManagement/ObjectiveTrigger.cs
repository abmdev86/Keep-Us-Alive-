#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
#endif
using UnityEngine;
using static com.sluggagames.keepUsAlive.LevelManagement.Quest;

namespace com.sluggagames.keepUsAlive.LevelManagement
{
    /// <summary>
    /// Combines a quest, an objective in that quest, and an objective Status to use.
    /// </summary>
    [System.Serializable]
    public class ObjectiveTrigger
    {
        public Quest quest;

        public Status statusToApply;

        public int objectiveNumber;

        
        public void Invoke()
        {
            
            QuestManager manager = Object.FindObjectOfType<QuestManager>();
            
            manager.UpdateObjectiveStatus(quest, objectiveNumber, statusToApply);
        }

    }

#if UNITY_EDITOR
    // override how it looks in editor
    [CustomPropertyDrawer(typeof(ObjectiveTrigger))]
    public class ObjectiveTriggerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            SerializedProperty questProp = property.FindPropertyRelative("quest");
            SerializedProperty statusProp = property.FindPropertyRelative("statusToApply");
            SerializedProperty objectiveNumProp = property.FindPropertyRelative("objectiveNumber");

            int lineSpacing = 2;

            Rect firstLinePos = position;

            firstLinePos.height = base.GetPropertyHeight(questProp, label);

            Rect secondLinePos = position;
            secondLinePos.y = firstLinePos.y + firstLinePos.height + lineSpacing;

            secondLinePos.height = base.GetPropertyHeight(statusProp, label);

            Rect thirdLinePos = position;
            thirdLinePos.y = secondLinePos.y + secondLinePos.height + lineSpacing;
            thirdLinePos.height = base.GetPropertyHeight(objectiveNumProp, label);

            EditorGUI.PropertyField(firstLinePos, questProp, new GUIContent("Quest"));
            EditorGUI.PropertyField(secondLinePos, statusProp, new GUIContent("Status"));

            thirdLinePos = EditorGUI.PrefixLabel(thirdLinePos, new GUIContent("Objective"));

            var quest = questProp.objectReferenceValue as Quest;
            if(quest != null && quest.Objectives.Count > 0)
            {
                string[] objectiveNames = quest.Objectives.Select(o => o.name).ToArray();

                var selectedObjective = objectiveNumProp.intValue;
                if(selectedObjective >= quest.Objectives.Count)
                {
                    selectedObjective = 0;
                }

                var newSelectedObjective = EditorGUI.Popup(thirdLinePos, selectedObjective, objectiveNames);
                if(newSelectedObjective != selectedObjective)
                {
                    objectiveNumProp.intValue = newSelectedObjective;
                }
            }
            else
            {
                using(new EditorGUI.DisabledGroupScope(true))
                {
                    EditorGUI.Popup(thirdLinePos, 0, new[] { "-" });
                }
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int lineCount = 3;
            int lineSpacing = 2;
            var lineHeight = base.GetPropertyHeight(property, label);
            return (lineHeight * lineCount) + (lineSpacing * (lineCount - 1));
        }
    }
#endif
}
