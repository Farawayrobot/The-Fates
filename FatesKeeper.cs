using System.Collections.Generic;
using Sirenix.OdinInspector;
using TheFates.Nona.QuestSystem;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TheFates
{
    public class FatesKeeper : MonoBehaviour
    {
        #region Singleton Pattern
        private static FatesKeeper _instance;
        public static FatesKeeper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<FatesKeeper>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("FatesKeeper (Auto)");
                        _instance = go.AddComponent<FatesKeeper>();
                    }
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            _instance = this;
        }
        #endregion

        [Title("Narrative Management")]
        // This list stays exactly as you had it, showing the nested Quests -> Stages -> Challenges (and buttons)
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        [ListDrawerSettings(ShowFoldout = true, DraggableItems = false)]
        [SerializeField] private List<QuestLine> questLines = new List<QuestLine>();

        [Header("Runtime Settings")]
        [SerializeField] private GameObject challengePrefab; 

        #region Challenge Spawning Logic

        public void SpawnChallengeObject(Challenge challenge)
        {
            if (challenge == null) return;

#if UNITY_EDITOR
            // Your original dynamic loading logic
            if (challengePrefab == null)
            {
                string path = "Assets/TheFates/QuestSystem/Challenge Object.prefab";
                challengePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (challengePrefab == null)
                {
                    Debug.LogError($"Could not find prefab at {path}.");
                    return;
                }
            }
#endif

            string parentName = "-- ] QL: " + challenge.ParentQuestLine.questLineName + " [ --"; 
            Transform questLineParent = this.transform.Find(parentName);
            
            if (questLineParent == null)
            {
                GameObject newParent = new GameObject(parentName);
                newParent.transform.SetParent(this.transform);
                questLineParent = newParent.transform;
                
                // Attach the Woven Logic
                AttachWovenLogic(newParent, challenge.ParentQuestLine.questLineName);
            }
            else
            {
                // Duplicate check
                string checkName = $"[{challenge.ParentQuestLine.questLineName}] {challenge.ParentQuest.questName}: {challenge.ParentStage.stageName} | {challenge.challengeName}";
                if (questLineParent.Find(checkName))
                {
                    Debug.LogError($"[FatesKeeper] A GameObject already exists for {checkName}.");
                    return;
                }
            }
            
            GameObject go = Instantiate(challengePrefab, questLineParent);
            ChallengeObject controller = go.GetComponent<ChallengeObject>();
            controller.Initialize(challenge);
        }

        private void AttachWovenLogic(GameObject newParent, string qlName)
        {
            string scriptName = "TheFates.ThreadsOf" + qlName.Replace("_", "");
            System.Type scriptType = System.Type.GetType($"{scriptName}, Assembly-CSharp");

            if (scriptType != null)
            {
                Component temp = newParent.AddComponent(scriptType);
                if (temp is UnchangingFate questScript)
                {
                    // You'll need to pass the appropriate challenge or data here as per your original logic
                }
            }
        }

        #endregion

        public Quest GetQuest(string qName)
        {
            foreach (var ql in questLines)
            {
                var found = ql.quests.Find(q => q.questName == qName);
                if (found != null) return found;
            }
            return null;
        }

        public void RegisterQuestLine(QuestLine ql)
        {
            if (!questLines.Contains(ql)) questLines.Add(ql);
        }
    }
}