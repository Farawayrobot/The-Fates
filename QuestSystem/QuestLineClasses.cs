/// The Fates Narrative Event System
/// Quest, stage and challenge c# classes
/// 
/// 08/01/24 User Study System
/// 02/26/26 The Fates Quest refactor
/// by Levi Scully


using System;
using UnityEngine;
using Sirenix.OdinInspector; 
using System.Collections.Generic;
using Unity.Mathematics;

namespace TheFates
{

    [Serializable]
    public class Challenge {
        [HorizontalGroup("Row", Width = 0.7f), HideLabel]
        public string challengeName;
        private int3 challengeID;
        
        public int3 GetChallengeID() => challengeID;
            
        [HorizontalGroup("Row"), LabelWidth(50)]
        public int amount;
        
        #region Parent Getters
        [NonSerialized] private Stage _parentStage;
        [NonSerialized] private Quest _parentQuest;
        
        [NonSerialized] private QuestLine _parentQuestLine;
        public QuestLine ParentQuestLine => _parentQuestLine;
        
        public Stage ParentStage => _parentStage;
        public Quest ParentQuest => _parentQuest;
        #endregion
        
        #region Link Logic
        public void Link(Stage stage, Quest quest, QuestLine questLine)
        {
            _parentStage = stage;
            _parentQuest = quest;
            _parentQuestLine = questLine;
            challengeID.x = quest.GetQuestID();
            challengeID.y = stage.GetStageID();
            challengeID.z = stage.challenges.FindIndex(x => x.challengeName == challengeName);
        }
        
        
        #endregion
        
        #region Events
                 
        public event Action<Challenge> ChallengeHasStarted;
        public event Action<Challenge> ChallengeHasCompleted;

        #endregion
        

        #region Broadcast Methods
        
        public void ChallengeStartedEventBroadcast() => ChallengeHasStarted?.Invoke(this);
        public void ChallengeCompletedEventBroadcast() => ChallengeHasCompleted?.Invoke(this);

        #endregion

        #region Buttons

        [Button(ButtonSizes.Small)]
        public void ManualSpawn() => FatesKeeper.Instance.SpawnChallengeObject(this);

        [Button(ButtonSizes.Small), FoldoutGroup("Debug Events")]
        public void ManualStarted() => ChallengeStartedEventBroadcast();
        
        [Button(ButtonSizes.Small), FoldoutGroup("Debug Events")]
        public void ManualComplete() => ChallengeCompletedEventBroadcast();
        
 

        #endregion
        
        }

        #region  Stage

        [Serializable]
        public class Stage {
            [Title("Stage Details", titleAlignment: TitleAlignments.Centered)]
            public string stageName;
            private int stageID;
            
            #region Parent Getters
            [NonSerialized] private Quest _parentQuest;
            public Quest ParentQuest => _parentQuest;
            
            [NonSerialized] private QuestLine _parentQuestLine;
            public QuestLine ParentQuestLine => _parentQuestLine;
            
            #endregion
            
            
            [ListDrawerSettings(ShowFoldout = false, ListElementLabelName = "challengeName")]
            public List<Challenge> challenges = new List<Challenge>();
            
            public void Link(Quest q, QuestLine ql) {
                _parentQuest = q;
                _parentQuestLine = ql;
                stageID = q.stages.FindIndex(x => x.stageName == stageName);
                foreach (var c in challenges) c.Link(this, q, ql);
            }

            public int GetStageID()
            {
                return stageID;
            }
        }

        #endregion

        #region Quest
        
        [Serializable]
        public class Quest {
            [BoxGroup("$questName", ShowLabel = true)] // The box title updates as you type the name
            public string questName;
            private int questID;
            [NonSerialized] private QuestLine _parentQuestLine;
            public QuestLine ParentQuestLine => _parentQuestLine;
            
            [ListDrawerSettings(DraggableItems = true, ShowPaging = true, NumberOfItemsPerPage = 5)]
            public List<Stage> stages = new List<Stage>();
            
            public void Link(QuestLine ql) {
                _parentQuestLine = ql;
                questID = ql.quests.FindIndex(x => x.questName == questName);
                foreach (var s in stages) s.Link(this, ql);
            }

            public int GetQuestID()
            {
                return questID;
            }
    
        }

            #endregion

}