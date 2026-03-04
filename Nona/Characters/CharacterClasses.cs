using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TheFates.Nona
{
    [System.Serializable]
    public enum CharacterType
    {
        Player,
        Npc
    }
    
    
    [System.Serializable]
    public class StatModifier
    {
        [SerializeField, HorizontalGroup, HideLabel]
        private int amount;

        [ShowInInspector, ReadOnly, HorizontalGroup]
        private object source; 

        [SerializeField, HorizontalGroup, ReadOnly]
        private string description;

        // --- Properties (Read-Only Access) ---
        public int Amount => amount;
        public object Source => source;
        public string Description => description;

        // --- Constructor ---
        public StatModifier(int amount, object source, string description = "")
        {
            this.amount = amount;
            this.source = source;
            this.description = description;
        }
    }
    
    [System.Serializable]
    public class Stat
    {
        [SerializeField, HorizontalGroup("Stat", Width = 0.4f), HideLabel]
        private string name;

        public string GetName()
        {
            return name;
        }

        [SerializeField, HorizontalGroup("Stat", Width = 0.3f), HideLabel]
        [OnValueChanged("UpdateValue")]
        private int baseValue;

        public int GetBaseValue()
        {
            return baseValue;
        }

        [ShowInInspector, HorizontalGroup("Stat", Width = 0.3f), HideLabel, ReadOnly]
        [GUIColor(0.7f, 1f, 0.7f)] // Light green to show it's the "Final" value
        public int totalValue;

        private List<StatModifier> modifiers = new List<StatModifier>();

        public Stat(string inputName, int inputBaseValue)
        {
            name = inputName;
            baseValue = inputBaseValue;
            UpdateValue();
        }
        

        public void SetName(string inputName)
        {
            name = inputName;
        }

        // Manual setter for your SO button logic
        public void SetBaseValue(int value)
        {
            baseValue = value;
            UpdateValue();
        }
        
        public void AddModifier(StatModifier modifier)
        {
            modifiers.Add(modifier);
            UpdateValue();
        }

        public void UpdateValue() 
        {
            totalValue = baseValue;
            foreach (var statModifier in modifiers)
            {
                totalValue += statModifier.Amount;
            }
        }
    }

    [System.Serializable]
    public class CharacterAbilities
    {
        // 1. Mark as SerializeField so the SO actually saves the data
        [SerializeField] private Stat strength = new Stat("Strength", 0);
        [SerializeField] private Stat dexterity = new Stat("Dexterity", 0);
        [SerializeField] private Stat constitution = new Stat("Constitution", 0);
        [SerializeField] private Stat wisdom = new Stat("Wisdom", 0);
        [SerializeField] private Stat intelligence = new Stat("Intelligence", 0);
        [SerializeField] private Stat charisma = new Stat("Charisma", 0);

        // 2. Use a List that references the actual fields for the Inspector
        // We use [ShowInInspector] because the list itself isn't what we save; we save the fields above.
        [Title("Ability Scores")]
        [ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true)]
        public List<Stat> AllAbilities => new List<Stat> 
        { 
            strength, dexterity, constitution, wisdom, intelligence, charisma 
        };
    
        public void SetAllAbilities(int str, int dex, int con, int wis, int intel, int cha)
        {
            strength.SetBaseValue(str);
            dexterity.SetBaseValue(dex);
            constitution.SetBaseValue(con);
            wisdom.SetBaseValue(wis);
            intelligence.SetBaseValue(intel);
            charisma.SetBaseValue(cha);
        }

        // Constructor for deep copying
        public CharacterAbilities(CharacterAbilities source)
        {
            if (source == null) return;
            SetAllAbilities(
                source.strength.GetBaseValue(), 
                source.dexterity.GetBaseValue(), 
                source.constitution.GetBaseValue(),
                source.wisdom.GetBaseValue(),
                source.intelligence.GetBaseValue(),
                source.charisma.GetBaseValue()
            );
        }

        // Empty constructor for Unity's initial creation
        public CharacterAbilities() { }
    }
    
    public class Fibre
    {
        
    }

    public class Thread
    {
        
    }
    
    
}