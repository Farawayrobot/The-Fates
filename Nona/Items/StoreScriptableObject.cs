using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TheFates.Nona
{
    [CreateAssetMenu(fileName = "New Store", menuName = "Fates/Store")]
    public class StoreScriptableObject : SerializedScriptableObject
    {
        [BoxGroup("Store Identity")]
        [HorizontalGroup("Store Identity/Top", 75)]
        [PreviewField(75, ObjectFieldAlignment.Left), HideLabel]
        public Sprite storeIcon;

        [VerticalGroup("Store Identity/Top/Right")]
        [LabelText("Store Name")] public string storeName = "New Store";
        
        [VerticalGroup("Store Identity/Top/Right")]
        public StoreType storeType;

        [BoxGroup("Finances")]
        [GUIColor(1, 0.8f, 0)]
        public int currentGold = 1000;

        [BoxGroup("Personnel")]
        public CharacterSheet shopkeep;

        [Title("Current Inventory")]
        [TableList(AlwaysExpanded = true, DrawScrollView = false)]
        public List<ShopInventoryItem> storeStock = new List<ShopInventoryItem>();

        #region Inventory Management Tool (Mirrored from CharacterSheet)

        [Title("Stocking Tool")]
        [FoldoutGroup("Inventory Management")]
        [EnumPaging]
        public ItemType typeToAdd;

        [FoldoutGroup("Inventory Management")]
        [ValueDropdown("GetFilteredItems", IsUniqueList = true, ExpandAllMenuItems = true)]
        [LabelText("Select Item to Stock")]
        public ItemScriptableObject selectedItem;

        [FoldoutGroup("Inventory Management")]
        public int priceForNewItem = 1;
        
        [FoldoutGroup("Inventory Management")]
        public int amountToStock = 1;

        [FoldoutGroup("Inventory Management")]
        [Button(ButtonSizes.Large, Name = "Add to Store Stock")]
        [EnableIf("@selectedItem != null")]
        public void AddToStoreStock()
        {
            var existing = storeStock.Find(x => x.item == selectedItem);
            if (existing != null)
            {
                existing.stock += amountToStock;
            }
            else
            {
                storeStock.Add(new ShopInventoryItem(selectedItem, priceForNewItem, amountToStock));
            }
            
            // Optional: reset selection after adding
            selectedItem = null;
        }

        private IEnumerable<ValueDropdownItem> GetFilteredItems()
        {
#if UNITY_EDITOR
            string folder = typeToAdd switch
            {
                ItemType.Consumables => "Assets/TheFates/Nona/Items/Consumables",
                ItemType.Arms        => "Assets/TheFates/Nona/Items/Arms",
                ItemType.QuestItems  => "Assets/TheFates/Nona/Items/QuestItems",
                _                    => "Assets/TheFates/Nona/Items"
            };

            var guids = AssetDatabase.FindAssets($"t:{typeof(ItemScriptableObject).Name}", new[] { folder });
            
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<ItemScriptableObject>(path);
                if (asset != null)
                {
                    yield return new ValueDropdownItem(asset.itemName, asset);
                }
            }
#else
            yield break;
#endif
        }
        #endregion
    }
}