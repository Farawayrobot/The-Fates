/// The Fates Narrative Event System
/// The Fates:
/// Unity Facing access to the Fate System
/// 08/01/24
/// by Levi Scully
///
/// This script provides access to the Fate System within Unity, allowing events to be
/// controlled and managed through coroutines. It seems designed for a narrative-driven
/// setup, likely focusing on user interaction and storytelling.

using System;
using System.Collections;
using System.Collections.Generic;
using TheFates.Utilities;
using UnityEngine;

namespace TheFates.QuestSystem
{
    
    [Serializable]
    public class UnchangingFate : MonoBehaviour
    {
        [SerializeField] private QuestLine questLine;
        [SerializeField] public IEnumerator fatesGoldenThread;
        
        [SerializeField]
        private Camera playerCam;

        [SerializeField]
        private EventTimer startupTimer;
        
        public void InitPrefabs(Challenge challenge)
        {
            startupTimer = gameObject.AddComponent<EventTimer>();
            startupTimer.SetTimerDuration(1.0f);
            playerCam = Camera.main;
            questLine = challenge.ParentQuestLine;
        }

        public virtual IEnumerator TheCrossRoads(Challenge challenge)
        {
            throw new NotImplementedException();
        }
    }
}