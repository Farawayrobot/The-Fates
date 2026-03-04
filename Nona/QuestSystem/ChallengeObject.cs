/// The Fates Narrative Event System
/// Challenge Object Monobehavior
///
/// 08/01/24 User Study System
/// 02/26/26 The Fates Quest refactor
/// by Levi Scully

using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using TheFates;
using Unity.VisualScripting; // Use your namespace

namespace TheFates.QuestSystem
{
    public class ChallengeObject : MonoBehaviour
    {
        [ReadOnly] [HideInInspector]
        public Challenge data; // The reference to your serializable class

        [Button(ButtonSizes.Small), FoldoutGroup("Debug Events")]
        public void ManualStarted() => data.ChallengeStartedEventBroadcast();

        [Button(ButtonSizes.Small), FoldoutGroup("Debug Events")]
        public void ManualComplete() => data.ChallengeCompletedEventBroadcast();

        [Button(ButtonSizes.Small), FoldoutGroup("Debug Events")]
        public void ManualEventSubscribe() => SubscribeToChallengeDebugEvents();

        public void Initialize(Challenge challengeData)
        {
            this.data = challengeData;

            // Grab all names safely
            string qlName = data.ParentQuestLine?.questLineName ?? "NoLine";
            string qName = data.ParentQuest?.questName ?? "NoQuest";
            string sName = data.ParentStage?.stageName ?? "NoStage";
            string cDesc = data.challengeName;
            data.ChallengeStatus = ChallengeStatus.NotCollected;


            // Result: [Main Story] Farmer Jon: Stage 1 | Collect Wool
            this.gameObject.name = $"-- ] {qlName} {qName} : {sName} | {cDesc} [ --";
            SubscribeToChallengeDebugEvents();

        }

        private void SubscribeToChallengeDebugEvents()
        {
            // Subscribe to the events you wrote!
            data.ChallengeHasStarted += OnChallengeStarted;
            data.ChallengeHasCompleted += OnChallengeCompleted;

        }

        private IEnumerator activeState;

        private void OnChallengeStarted(Challenge c)
        {
            // 1. Initialize the state machine
            var logicProvider = data.ParentQuestLine.GetComponent<UnchangingFate>();
            activeState = logicProvider.TheCrossRoads(c);

            // 2. Trigger the FIRST step (e.g., Play Audio & Start Timer)
            AdvanceLogic();
        }

        public void AdvanceLogic()
        {
            if (activeState != null)
            {
                // Move to the next block of code until the next 'yield'
                if (!activeState.MoveNext())
                {
                    // If MoveNext is false, the function has finished
                    activeState = null;
                    Debug.Log("Challenge Logic Sequence Complete");
                }
            }
        }

        private void OnChallengeCompleted(Challenge c)
        {
            Debug.Log($"GO {name} reacted to Complete!");
            AdvanceLogic(); // Disable the object, play a sound, etc.
        }

        private void OnDestroy()
        {
            if (data != null)
            {
                data.ChallengeHasStarted -= OnChallengeStarted;
                data.ChallengeHasCompleted -= OnChallengeCompleted;
            }
        }
    }
}