using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TheFates.Nona
{
    [CreateAssetMenu(fileName = "CharacterFactory", menuName = "The Fates/Factories/Character Factory")]
    public class CharacterFactory : SerializedScriptableObject
    {
        // Define the main horizontal split
        [HorizontalGroup("Split", width: 0.25f)] // This is the Left Column for the Icon
        [HideLabel, PreviewField(90, ObjectFieldAlignment.Left)]
        public Sprite characterIcon;

        [VerticalGroup("Split/Right")] // This is the Right Column for the Text
        [BoxGroup("Split/Right/Identity", LabelText = "Character Identity")]
        [LabelWidth(100)]
        public string CharacterName = "New NPC";

        [VerticalGroup("Split/Right")]
        [BoxGroup("Split/Right/Identity")]
        public CharacterType characterType;
       
        [BoxGroup("Stats", LabelText = "Initial Allotment")]
        [HideLabel] // This hides the "Character Stats" header so the internal fields breathe
        public CharacterStats characterStats;
        
        [Button(ButtonSizes.Large), GUIColor(0, 1, 0.5f)]
        public void SpinNewCharacterSheet()
        {
#if UNITY_EDITOR
            // 1. Determine destination folder
            string subFolder = (characterType == CharacterType.Player) ? "Players" : "NPCs";
            string relativePath = $"Assets/TheFates/Nona/Characters/{subFolder}";
            
            // 2. Ensure Folders exist
            EnsureFolders(relativePath);

            // 3. Create and Populate Instance
            CharacterSheet newSheet = ScriptableObject.CreateInstance<CharacterSheet>();
            newSheet.name = CharacterName;
       
            newSheet.stats = new CharacterStats(characterStats);
            newSheet.SetCharacterType(characterType);
            newSheet.characterIcon = characterIcon;

            // 4. Save
            string finalPath = AssetDatabase.GenerateUniqueAssetPath($"{relativePath}/{CharacterName}.asset");
            AssetDatabase.CreateAsset(newSheet, finalPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"<b>Nona:</b> A new thread has been spun for <b>{CharacterName}</b> at {finalPath}");
#endif
        }

        private void EnsureFolders(string path)
        {
#if UNITY_EDITOR
            string[] folders = path.Split('/');
            string currentPath = "Assets";
            for (int i = 1; i < folders.Length; i++)
            {
                if (!AssetDatabase.IsValidFolder(currentPath + "/" + folders[i]))
                    AssetDatabase.CreateFolder(currentPath, folders[i]);
                currentPath += "/" + folders[i];
            }
#endif
        }
    }
}