using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TheFates.Nona
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Fates/Item")]
    public class ItemScriptableObject : SerializedScriptableObject
    {
        [TableColumnWidth(60, Resizable = false)]
        [PreviewField(50, ObjectFieldAlignment.Left), HideLabel]
        public Sprite itemIcon;

        [BoxGroup("Core Identity")]
        [LabelWidth(100)]
        public string itemName;

        [BoxGroup("Core Identity")]        
        [LabelWidth(100)]
        public ItemType type;

        [BoxGroup("Core Identity")]
        [LabelWidth(100)]
        public ItemRarity rarity;
        
        [BoxGroup("Core Identity"), ShowIf("IsArms")]
        public ArmsType armsType;
        
        [BoxGroup("Core Identity"), ShowIf("IsConsumable")]
        public int amount = 1;       
        [BoxGroup("Core Identity"), ShowIf("IsArmRanged")]
        public int ammo = 1;
        

        
        
        
        [FoldoutGroup("Core Identity/Item Attributes"), TextArea(5, 12)]
        public string description = "Enter item flavor text here...";
        
        [FoldoutGroup("Core Identity/Item Attributes"), HideLabel]
        [SerializeField] public CharacterAbilities itemAbilities;

        private bool IsConsumable => type == ItemType.Consumables;
        private bool IsArms => type == ItemType.Arms;
        
        private bool IsArmRanged => armsType == ArmsType.Ranged && type == ItemType.Arms;
        
        private bool IsQuestItem => type == ItemType.QuestItems;
        
        
    

        
    }
    }

