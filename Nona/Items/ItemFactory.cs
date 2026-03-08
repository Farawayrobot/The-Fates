using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TheFates.Nona
{
    [CreateAssetMenu(fileName = "ItemFactory", menuName = "The Fates/Factories/Item Factory")]
    public class ItemFactory : SerializedScriptableObject
    {
        [HorizontalGroup("Top", width: 80)] // Fixed width for the icon slot
        [HideLabel, PreviewField(75, ObjectFieldAlignment.Left)]
        [SerializeField] public Sprite itemIcon;

        [VerticalGroup("Top/Right")]
        [BoxGroup("Top/Right/Identity", LabelText = "Item Identity")]
        [LabelWidth(120)] // Slightly wider to accommodate longer display names
        [SerializeField] public string ItemDisplayName = "New Item";

        [VerticalGroup("Top/Right")]
        [BoxGroup("Top/Right/Identity")]
        [MultiLineProperty(3), HideLabel] // Multi-line is much better for descriptions
        [SerializeField] string ItemDisplayDescription = "New Item Description";
        [VerticalGroup("Top/Right")]
        [BoxGroup("Top/Right/Identity")]
        public ItemType type;
        [VerticalGroup("Top/Right")]
        [BoxGroup("Top/Right/Identity")]
        public ItemRarity rarity;
        
        [Button(ButtonSizes.Large), GUIColor(0, 1, 0.5f)]
        public void ForgeItem()
        {
#if UNITY_EDITOR
            string folderName = GetFolderForItemType(type);
            string relativePath = $"Assets/TheFates/Nona/Items/{folderName}";

            EnsureFolders(relativePath);

            ItemScriptableObject newItem = ScriptableObject.CreateInstance<ItemScriptableObject>();
            newItem.itemName = ItemDisplayName;
            newItem.description = ItemDisplayDescription;
            newItem.type = type;
            newItem.rarity = rarity;

            string finalPath = AssetDatabase.GenerateUniqueAssetPath($"{relativePath}/{ItemDisplayName}.asset");
            AssetDatabase.CreateAsset(newItem, finalPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"<b>Nona:</b> Forged <b>{ItemDisplayName}</b> in <b>{folderName}</b>.");
#endif
        }

        private string GetFolderForItemType(ItemType type)
        {
            switch (type)
            {
                case ItemType.Arms: return "Arms";
                case ItemType.Consumables: return "Consumables";
                case ItemType.QuestItems: return "QuestItems";
                default: return "Misc";
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