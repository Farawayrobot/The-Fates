using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace TheFates.Nona
{
   
   [Serializable]
   public enum ItemType
   {
      Arms,
      Consumables,
      QuestItems
   }

   public enum ArmsType
   {
      Melee,
      Ranged,
      Defense
   }
   
   [Serializable]
   public enum ItemRarity
   {
      Common,
      Uncommon,
      Rare,
      Legendary,
      Mythic
   }

   [Serializable]
   public enum StorageLocations
   {
      Self,
      Backpack,
      Stash
   }

   [Serializable]
   public class StatData
   {
      public string name;
      public int value;
      
      public StatData(string name, int value)
      {
         this.name = name;
         this.value = value;
      }
   }
   
   [Serializable]
   public class InventoryItem
   {
      [HideLabel]
      [DisplayAsString(Alignment = TextAlignment.Left)]
      [GUIColor(0.8f, 0.8f, 1f, 1f)] // Light blue tint for the name
      [PropertyOrder(-10)]
      [BoxGroup("$itemName"), HideInInspector]
      public string itemName;
        
      [HideInInspector] 
      [Required]
      public ItemScriptableObject data;
        
      [BoxGroup("$itemName")]
      public StorageLocations storageLocation;

      [BoxGroup("$itemName")]
      [ShowIf("IsStackable")]
      public int currentStack;
    
      public InventoryItem(ItemScriptableObject source, int stackCount = 1)
      {
         this.data = source;
         this.itemName = source.name;
         this.currentStack = stackCount;
            
      }

      #region Odin Helpers
      private bool IsStackable => data != null && (data.type == ItemType.Consumables || data.type == ItemType.QuestItems);
    
        
      #endregion
   }
   
   public enum StoreType
   {
      General,
      Blacksmith,
      Alchemist,
      ShadyDealer
   }

   [Serializable]
   public class ShopInventoryItem
   {
      [HorizontalGroup("Item"), HideLabel, ReadOnly]
      public ItemScriptableObject item;
    
      [HorizontalGroup("Item", 80), LabelWidth(40)]
      public int price;
    
      [HorizontalGroup("Item", 80), LabelWidth(40)]
      public int stock;

      public ShopInventoryItem(ItemScriptableObject item, int price, int stock = 1)
      {
         this.item = item;
         this.price = price;
         this.stock = stock;
      }
   }
   

}