using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEditor;

namespace TheFates.Nona
{

    public class CharacterSheetFactory : MonoBehaviour
    {
        [Title("New Character Identity")] [LabelWidth(100)]
        public string CharacterName = "New Hero";
        
        public CharacterType characterType;
        
        [Title("Initial Stat Allotment")] [BoxGroup("Stats"), PropertyRange(-2, 8)]
        public int Strength = 0;

        [BoxGroup("Stats"), PropertyRange(-2, 8)]
        public int Dexterity = 0;

        [BoxGroup("Stats"), PropertyRange(-2, 8)]
        public int Constitution = 0;

        [BoxGroup("Stats"), PropertyRange(-2, 8)]
        public int Intelligence = 0;

        [BoxGroup("Stats"), PropertyRange(-2, 8)]
        public int Wisdom = 0;

        [BoxGroup("Stats"), PropertyRange(-2, 8)]
        public int Charisma = 0;

        [Button(ButtonSizes.Large), GUIColor(0, 1, 0.5f)]
        public void SpinNewCharacterSheet()
        {
#if UNITY_EDITOR
            // 1. Create the Instance
            CharacterSheet newSheet = ScriptableObject.CreateInstance<CharacterSheet>();
            newSheet.Name = CharacterName;

            // 2. Set the Stats immediately
            // This uses the method we built in CharacterStats earlier
            newSheet.abilities.SetAllAbilities(
                Strength, Dexterity, Constitution,
                Wisdom, Intelligence, Charisma
            );

            newSheet.SetCharacterType(characterType);
            
            // 1. Determine destination folder based on your existing Enum
            // Assuming 'Player' is one of your enum entries
            string subFolder = (characterType == CharacterType.Player) ? "Players" : "NPCs";
            string relativePath = $"Assets/TheFates/Nona/Characters/{subFolder}";
            
            
            // 3. Handle Folders via AssetDatabase
            string cleanPath = relativePath.Trim('/', '\\');
            string[] folders = cleanPath.Split('/');
            string currentPath = "Assets";

            for (int i = 1; i < folders.Length; i++) // Skip "Assets" if it's the first folder
            {
                if (!AssetDatabase.IsValidFolder(currentPath + "/" + folders[i]))
                {
                    AssetDatabase.CreateFolder(currentPath, folders[i]);
                }

                currentPath += "/" + folders[i];
            }

            // 4. Generate File Path
            string finalPath = $"{currentPath}/{CharacterName}.asset";
            finalPath = AssetDatabase.GenerateUniqueAssetPath(finalPath);

            // 5. Save and Refresh
            AssetDatabase.CreateAsset(newSheet, finalPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // // 6. Focus the result
            // EditorUtility.FocusProjectWindow();
            // Selection.activeObject = newSheet;

            Debug.Log($"<b>Nona:</b> A new thread has been spun for <b>{CharacterName}</b> at {finalPath}");
#endif
        }
    }
}
