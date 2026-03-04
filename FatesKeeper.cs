/// The Fates Narrative Event System
/// FatesKeeper
/// 
/// 08/01/24 User Study System
/// 02/26/26 The Fates Quest refactor
/// by Levi Scully

using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using TheFates.QuestSystem;
using UnityEditor;
using UnityEditor.Analytics;
using UnityEngine;

    // NO UnityEditor using statement here at all

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
                // Optional: Don'tDestroyOnLoad(gameObject);
            }
            #endregion
            
            #region Nona
            [VerticalGroup("NonaOuter", PaddingTop = 20, PaddingBottom = 20)]
            [FoldoutGroup("NonaOuter/Nona The Spinner")]
            
            [PropertySpace(SpaceBefore = 0, SpaceAfter = 10)]
            [Title("Create New Quest Line")]
            [InlineButton("CreateNewQuestLineAsset", "Create Asset")]
            public string newQuestLineName;
            
            private string savePath = "Assets/TheFates/Nona/QuestSystem/QuestLines";
            
            [InlineEditor(InlineEditorModes.FullEditor),ListDrawerSettings(ShowFoldout = true)]
            [SerializeField] List<QuestLine> questLines = new List<QuestLine>();
            
            private void CreateNewQuestLineAsset()
            {
                // We wrap the logic here
#if UNITY_EDITOR
                // Create the instance
                QuestLine newAsset = ScriptableObject.CreateInstance<QuestLine>();
                newAsset.questLineName = newQuestLineName;

                // Ensure folder exists
                if (!System.IO.Directory.Exists(savePath))
                {
                    System.IO.Directory.CreateDirectory(savePath);
                }

                // We use the FULL name: UnityEditor.AssetDatabase
                string fullPath = $"{savePath}/{newQuestLineName}.asset";
                fullPath = UnityEditor.AssetDatabase.GenerateUniqueAssetPath(fullPath);

                UnityEditor.AssetDatabase.CreateAsset(newAsset, fullPath);
                UnityEditor.AssetDatabase.SaveAssets();
                
                if (questLines == null) questLines = new List<QuestLine>();
                questLines.Add(newAsset);

                Debug.Log($"Created: {fullPath}");
#else
                Debug.LogWarning("AssetDatabase is an Editor-only feature.");
#endif
            }
            
            // --- The Loom (Generator) ---
            [Title("Generate C# logic from a QuestLine asset", titleAlignment: TitleAlignments.Centered)]
            [BoxGroup("NonaOuter/Nona The Spinner/Nona's Loom")]
            [ValueDropdown("questLines")] // Creates a dropdown from your list of assets
            public QuestLine questLineToWeave;
            
            [BoxGroup("NonaOuter/Nona The Spinner/Nona's Loom")]
            [Button(ButtonSizes.Large, Name = "Weave Selected Quest Line")]
            [EnableIf("questLineToWeave")]
            [GUIColor(0.5f, 0.8f, 1f)]
            public void WeaveLogicFromSelection()
            {
                if (questLineToWeave == null) return;

                
                // Use Path.Combine to handle OS-specific slashes correctly
                // And ensure the directory exists before we try to write to it
                string folderPath = Path.Combine(Application.dataPath, "TheFates", "Nona", "QuestSystem" ,"QuestLines");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string fullPath = Path.Combine(Application.dataPath.Replace("Assets", ""), folderPath);
                string scriptFileName = "WovenFatesOf" + questLineToWeave.questLineName.Replace("_","") + ".cs"; 
                fullPath = Path.Combine(folderPath, scriptFileName);
                
                // Safety Check: Avoid Overwriting
                if (File.Exists(fullPath))
                {
                    Debug.LogError($"[FatesKeeper] A script already exists for {questLineToWeave.questLineName} at {folderPath}. Please delete it manually if you want to re-generate.");
                    return;
                }
                
                Loom.Instance.WeaveFullQuestLine(questLineToWeave);
            
            #if UNITY_EDITOR
                UnityEditor.AssetDatabase.Refresh();
            #endif
            }

            
            #endregion

            #region ChallengePrefab

            [HideInInspector] GameObject challengePrefab; // A prefab with the ChallengeObject script
            
            public void SpawnChallengeObject(Challenge challenge)
            {
#if UNITY_EDITOR
                if (challengePrefab == null)
                {
                    // Use the GUID or the exact Path
                    string path = "Assets/TheFates/QuestSystem/Challenge Object.prefab";
                    challengePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                    if (challengePrefab == null)
                    {
                        Debug.LogError($"Could not find prefab at {path}. Did you move it?");
                        return;
                    }
                }
#endif
                // 1. Determine the name of the QuestLine parent
                // We assume the challenge knows its QuestLine parent name, 
                // or you can pass it into the function.
                string parentName = "-- ] QL" + challenge.ParentQuestLine.questLineName + " [ --"; 

                // 2. Find or Create the QuestLine Parent
                Transform questLineParent = this.transform.Find(parentName);
                
                if (questLineParent == null)
                {
                    GameObject newParent = new GameObject(parentName);
                    newParent.transform.SetParent(this.transform);
                    questLineParent = newParent.transform;
           
          
                    // --- Search and Attach Logic ---
    
                    // 1. Try to find the script by its name (without extension)
                    // This looks for "MyScriptName" in the Project
                    string scriptName = "TheFates.ThreadsOf" + challenge.ParentQuestLine.questLineName.Replace("_","");
    
                    // 2. Locate the Type in the Assembly
                    System.Type scriptType = System.Type.GetType($"{scriptName}, Assembly-CSharp");
                    //Debug.Log(scriptType);
                    if (scriptType != null)
                    {
                        // 3. Add the component to the new parent
                        Component temp = newParent.AddComponent(scriptType);
                        // Cast it to your base type or interface
                        if (temp is UnchangingFate questScript)
                        {
                            questScript.InitPrefabs(challenge); 
                        }
                        // Debug.Log($"Successfully attached {scriptName} to {parentName}");
                    }
                    else
                    {
                        // This usually happens if the script hasn't compiled yet 
                        // or if there is a namespace mismatch.
                       // Debug.LogWarning($"Could not find Type for {scriptName}. Is it compiled?");
                    }
                }
                else
                {
                    // Grab all names safely
                    string qlName = challenge.ParentQuestLine?.questLineName ?? "NoLine";
                    string qName  = challenge.ParentQuest?.questName ?? "NoQuest";
                    string sName  = challenge.ParentStage?.stageName ?? "NoStage";
                    string cDesc  = challenge.challengeName;

                    // Result: [Main Story] Farmer Jon: Stage 1 | Collect Wool
                    string temp = $"[{qlName}] {qName}: {sName} | {cDesc}";

                    for (int i = 0; i < questLineParent.childCount; i++)
                    {
                        if (questLineParent.GetChild(i).name == temp)
                        {
                            Debug.LogError($"[FatesKeeper] A GameObject already exists for {temp}.");
                            return;
                        }
                    }
                }
                
                // 1. Create the physical object
                GameObject go = Instantiate(challengePrefab, questLineParent);
    
                // 2. Get the component and link the data
                ChallengeObject controller = go.GetComponent<ChallengeObject>();
                controller.Initialize(challenge);
                
            }

            #endregion
            

            

            

            
            // Helper to find Quest by name for the generated scripts
            public Quest GetQuest(string qName)
            {
                foreach (var ql in questLines)
                {
                    var found = ql.quests.Find(q => q.questName == qName);
                    if (found != null) return found;
                }
                return null;
            }

            
       
        }
        
        
}