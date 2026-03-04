using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace TheFates.Nona
{
    public class ItemFactory : MonoBehaviour
    {
        [Title("Identity")] [HideLabel]
        [SerializeField] public string ItemDisplayName = "New Item";
        [SerializeField] string ItemDisplayDescription = "New Item Description";
        public ItemType type;
        public ItemRarity rarity;
        
        [Button(ButtonSizes.Large), GUIColor(0, 1, 0.5f)]
        public void ForgeItem()
        {
#if UNITY_EDITOR
            // 1. Determine Folder based on Type
            string folderName = GetFolderForItemType(type);
            string relativePath = $"Assets/TheFates/Nona/Items/{folderName}";

            // 2. Build the folders
            EnsureFolders(relativePath);

            // 3. Create the SO
            ItemScriptableObject newItem = ScriptableObject.CreateInstance<ItemScriptableObject>();
            newItem.itemName = ItemDisplayName; // This is your string field
            newItem.type = type;
            newItem.rarity = rarity;
            

            // 4. Save with a unique name
            string finalPath = AssetDatabase.GenerateUniqueAssetPath($"{relativePath}/{ItemDisplayName}.asset");
            AssetDatabase.CreateAsset(newItem, finalPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            //Selection.activeObject = newItem;
            Debug.Log($"<b>Nona:</b> Forged <b>{ItemDisplayName}</b> in <b>{folderName}</b>.");
#endif
        }

        private string GetFolderForItemType(ItemType t)
        {
            // This maps your Enum values to the specific folder names you want
            switch (t)
            {
                case ItemType.Arms:
                    return "Arms";
                case ItemType.Consumables:
                    return "Consumables";
                case ItemType.QuestItems:
                    return "QuestItems";
                default:
                    return "Misc";
            }
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