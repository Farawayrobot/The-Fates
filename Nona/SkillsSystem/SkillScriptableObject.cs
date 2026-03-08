using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TheFates.Nona
{
    public enum SkillType
    {
        Combat,
        OutOfCombat,
        Both
    }
    
    [Serializable]
    public class CharacterSkill
    {
        public Skill skillRef;
        public int skillValue;

    }
    
    [Serializable]
    [CreateAssetMenu(fileName = "New Skill", menuName = "Fates/Skills")]
    public class Skill : SerializedScriptableObject
    {
        public string Name;
        public string Description;
        public SkillType _skillType;
        
       
        [HorizontalGroup("Stats")]
        [ListDrawerSettings(
            DraggableItems = true, 
            Expanded = true, 
            ShowPaging = false, 
            CustomAddFunction = "AddNewStat"), HideLabel] // Optional: for custom logic
        public List<CharacterStatsEnum> statEnum = new List<CharacterStatsEnum>();

// If you want a specific default value when clicking the [+] button
        private void AddNewStat()
        {
            statEnum.Add(CharacterStatsEnum.Strength);
            

        }

    }


}