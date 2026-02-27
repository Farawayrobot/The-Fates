using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TheFates
{


    [CreateAssetMenu(fileName = "NewQuestLine", menuName = "Quests/QuestLine")]
    public class QuestLine : SerializedScriptableObject
    {

        [Title("Quest Line Info")] [GUIColor(0.8f, 1f, 0.8f)] // Makes the name field a light green
        public string questLineName;
        
        [Space(10)] [ListDrawerSettings(ShowIndexLabels = true, CustomAddFunction = "AddNewQuest")]
        public List<Quest> quests = new List<Quest>();

        // Optional: Custom logic when adding a new quest
        private Quest AddNewQuest()
        {
            return new Quest() { questName = "New Quest " + (quests.Count + 1) };
        }
        
        
        protected override void OnAfterDeserialize()
        {
            base.OnAfterDeserialize();
            RefreshLinks();
        }

        public void RefreshLinks()
        {
            if (quests == null) return;
            // Pass 'this' so every child knows which QuestLine it belongs to
            foreach (var q in quests) q.Link(this); 
        }
    }
}