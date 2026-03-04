using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;


namespace TheFates.Nona
{
    [CreateAssetMenu(fileName = "New Character Template", menuName = "Fates/Character Template")]

    public class CharacterSheet : SerializedScriptableObject
    {
        [Title("Character")] [HideLabel] public string Name = "New Hero";
        [PreviewField(50, ObjectFieldAlignment.Left), HideLabel]
        public Sprite characterIcon;

        
        [SerializeField, ReadOnly] private CharacterType characterType;

        public void SetCharacterType(CharacterType characterType)
        {
            this.characterType = characterType;
        }

        [Title("Vitals")] [HideLabel] public Stat healthPoints = new Stat("Health", 100);

        [HideLabel] public Stat manaPoints = new Stat("Mana", 50);

        [FoldoutGroup("Character Stats"), HideLabel, ReadOnly] // Hides the "Abilities" label to keep the UI clean
        public CharacterAbilities abilities;

        private void OnEnable()
        {
            // This ensures they are named correctly in the Inspector 
            // the very first time you click the SO.
            if (string.IsNullOrEmpty(healthPoints.GetName()))
                healthPoints = new Stat("Health", 100);

            if (string.IsNullOrEmpty(manaPoints.GetName()))
                manaPoints = new Stat("Mana", 50);
        }
        
        
        
        #region Inventory
        
        [FoldoutGroup("Inventory"), TabGroup("Inventory/Main", "Equipment")]
        [TableList(AlwaysExpanded = true)]
        public List<ItemScriptableObject> equipment = new List<ItemScriptableObject>();

        [FoldoutGroup("Inventory") , TabGroup("Inventory/Main", "Consumables")]
        [TableList(AlwaysExpanded = true)]
        public List<ItemScriptableObject> consumables = new List<ItemScriptableObject>();
        
        [FoldoutGroup("Inventory"),TabGroup("Inventory/Main", "Quest Items")]
        [TableList(AlwaysExpanded = true)]
        public List<ItemScriptableObject> questItems = new List<ItemScriptableObject>();
       
        [PropertySpace(SpaceBefore = 5)]
        [Title("Add Item To Inventory")]
        [FoldoutGroup("Inventory/Tool")]
        [EnumPaging] // Nice horizontal buttons for selecting type
        public ItemType typeToAdd;

        [FoldoutGroup("Inventory/Tool")]
        [ValueDropdown("GetFilteredItems", IsUniqueList = true, ExpandAllMenuItems = true)]
        [LabelText("Select Item to Add")]
        public ItemScriptableObject selectedItem;

        [PropertySpace(SpaceBefore = 10)]
        [FoldoutGroup("Inventory/Tool")]
        [Button(ButtonSizes.Large, Name = "Add to Inventory")]
        [EnableIf("@selectedItem != null")]
        public void AddSelectedItem()
        {
            switch (typeToAdd)
            {
                case ItemType.Consumables:
                    consumables.Add(selectedItem);
                    break;

                case ItemType.Arms:
                    equipment.Add(selectedItem);
                    break;

                case ItemType.QuestItems:
                    questItems.Add(selectedItem);
                    break;

                default:
                    Debug.LogWarning($"No list assigned for ItemType: {typeToAdd}");
                    break;
            }
        }

        // --- LOGIC TO POPULATE THE DROPDOWN ---
        private IEnumerable<ValueDropdownItem> GetFilteredItems()
        {
    #if UNITY_EDITOR
            // Define your folder paths here
    // Use a switch expression to determine the folder path based on the enum
            string folder = typeToAdd switch
            {
                ItemType.Consumables => "Assets/TheFates/Nona/Items/Consumables",
                ItemType.Arms        => "Assets/TheFates/Nona/Items/Arms",
                ItemType.QuestItems  => "Assets/TheFates/Nona/Items/QuestItems",
                _                    => "Assets/TheFates/Nona/Items" // Default fallback
            };

            // Find all assets in that folder
            var guids = AssetDatabase.FindAssets($"t:{typeof(ItemScriptableObject).Name}", new[] { folder });
            
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<ItemScriptableObject>(path);
                
                if (asset != null)
                {
                    // Returns the item to the dropdown with its name
                    yield return new ValueDropdownItem(asset.itemName, asset);
                }
            }
    #else
            return null;
    #endif
        }
    }
        #endregion 
}
