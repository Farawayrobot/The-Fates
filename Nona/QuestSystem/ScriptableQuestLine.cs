// The Fates Narrative Event System
// Questline Scriptable Object
// This script defines a QuestLine class which is a Scriptable Object used in the Fates Narrative Event System.
// It contains information about a quest line, including a name and a list of quests.
// The QuestLine class is derived from SerializedScriptableObject in the Sirenix.OdinInspector namespace.
// It allows for easy creation and management of quest lines through the Unity Editor.
// The class includes logic to add new quests to the list, as well as a method to refresh links between the quests and the quest line.
// The OnAfterDeserialize method is overridden to call RefreshLinks after deserialization to ensure proper linking.


/// 08/01/24 User Study System
/// 02/26/26 The Fates Quest refactor
/// by Levi Scully

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TheFates.QuestSystem
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