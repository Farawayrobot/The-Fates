using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine.Serialization;


namespace TheFates.Nona
{
    [CreateAssetMenu(fileName = "New Character Template", menuName = "Fates/Character Template")]

    public class CharacterSheet : SerializedScriptableObject
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
        
        [SerializeField] public GameObject characterPrefab;
        

        public void SetCharacterType(CharacterType characterType)
        {
            this.characterType = characterType;
        }

        [Title("Vitals")] [HideLabel] public Stat healthPoints = new Stat("Health", 100);

        [HideLabel] public Stat manaPoints = new Stat("Mana", 50);

        [FormerlySerializedAs("abilities")] [FoldoutGroup("Character Stats"), HideLabel, ReadOnly] // Hides the "Abilities" label to keep the UI clean
        public CharacterStats stats;

        private void OnEnable()
        {
            // This ensures they are named correctly in the Inspector 
            // the very first time you click the SO.
            if (string.IsNullOrEmpty(healthPoints.GetName()))
                healthPoints = new Stat("Health", 100);

            if (string.IsNullOrEmpty(manaPoints.GetName()))
                manaPoints = new Stat("Mana", 50);
        }
        
        
        public List<CharacterSkill> skills = new List<CharacterSkill>();
        
        #region Inventory
        
        [FoldoutGroup("Inventory"), TabGroup("Inventory/Main", "Equipment")]
        [TableList(AlwaysExpanded = true)]
        public List<InventoryItem> equipment = new List<InventoryItem>();

        [FoldoutGroup("Inventory") , TabGroup("Inventory/Main", "Consumables")]
        [TableList(AlwaysExpanded = true)]
        public List<InventoryItem> consumables = new List<InventoryItem>();
        
        [FoldoutGroup("Inventory"),TabGroup("Inventory/Main", "Quest Items")]
        [TableList(AlwaysExpanded = true)]
        public List<InventoryItem> questItems = new List<InventoryItem>();
       
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
                    foreach (var invItem in consumables)
                    {
                        if (invItem.data == selectedItem)
                        {
                            invItem.currentStack++;
                            return;
                        }
                    }
                    consumables.Add(new InventoryItem(selectedItem));
                    break;

                case ItemType.Arms:
                    foreach (var invItem in equipment)
                    {
                        if (invItem.data == selectedItem)
                        {
                           // return;
                        }
                    }
                    equipment.Add(new InventoryItem(selectedItem));
                    break;

                case ItemType.QuestItems:
                    foreach (var invItem in questItems)
                    {
                        if (invItem.data == selectedItem)
                        {
                            invItem.currentStack++;
                            return;
                        }
                    }
                    questItems.Add(new InventoryItem(selectedItem));
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
