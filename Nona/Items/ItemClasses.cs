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
   

}