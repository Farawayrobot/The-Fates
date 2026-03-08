using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace TheFates.Nona
{
    public class NonasSpinningWheel : OdinMenuEditorWindow
    {
        [MenuItem("The Fates/Nona's Spinning Wheel")]
        public static void OpenWindow() => GetWindow<NonasSpinningWheel>().Show();

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();

            // Find Skill Factory
            var skillFactory = AssetDatabase.FindAssets("t:SkillFactory")
                .Select(guid => AssetDatabase.LoadAssetAtPath<SkillFactory>(AssetDatabase.GUIDToAssetPath(guid)))
                .FirstOrDefault();

            // Find Item Factory
            var itemFactory = AssetDatabase.FindAssets("t:ItemFactory")
                .Select(guid => AssetDatabase.LoadAssetAtPath<ItemFactory>(AssetDatabase.GUIDToAssetPath(guid)))
                .FirstOrDefault();

            // Find Character Factory
            var charFactory = AssetDatabase.FindAssets("t:CharacterFactory")
                .Select(guid => AssetDatabase.LoadAssetAtPath<CharacterFactory>(AssetDatabase.GUIDToAssetPath(guid)))
                .FirstOrDefault();
            
            // Find Quest Factory
            var questFactory = AssetDatabase.FindAssets("t:QuestFactory")
                .Select(guid => AssetDatabase.LoadAssetAtPath<QuestFactory>(AssetDatabase.GUIDToAssetPath(guid)))
                .FirstOrDefault();
            
            // Find Store Factory
            var storeFactory = AssetDatabase.FindAssets("t:StoreFactory")
                .Select(guid => AssetDatabase.LoadAssetAtPath<StoreFactory>(AssetDatabase.GUIDToAssetPath(guid)))
                .FirstOrDefault();

            // New: Find Action Factory
            var actionFactory = AssetDatabase.FindAssets("t:ActionFactory")
                .Select(guid => AssetDatabase.LoadAssetAtPath<ActionFactory>(AssetDatabase.GUIDToAssetPath(guid)))
                .FirstOrDefault();

            // Adding to the tree

            if (questFactory != null) tree.Add("Quests", questFactory);
            if (charFactory != null) tree.Add("Characters", charFactory);
            if (skillFactory != null) tree.Add("Skills", skillFactory);
            if (actionFactory != null) tree.Add("Actions", actionFactory);
            if (storeFactory != null) tree.Add("Stores", storeFactory);
            if (itemFactory != null) tree.Add("Items", itemFactory);
            
            return tree;
        }
    }
}