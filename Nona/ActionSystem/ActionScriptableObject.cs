using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace TheFates.Nona
{

    [CreateAssetMenu(fileName = "New Action", menuName = "Fates/Combat/Action")]
    public class ActionScriptableObject : SerializedScriptableObject
    {
        [HorizontalGroup("Top", width: 80)] // Fixed width for the icon slot
        [HideLabel, PreviewField(75, ObjectFieldAlignment.Left)]
        public Sprite icon;

        [VerticalGroup("Top/Right")]
        [BoxGroup("Top/Right/Identity", LabelText = "Action Identity")]
        [LabelWidth(100)]
        public string actionName;

        [VerticalGroup("Top/Right")]
        [BoxGroup("Top/Right/Identity")]
        [LabelWidth(100)]
        public ActionType type;

        [PropertySpace(10)]
        [Title("Description")]
        [MultiLineProperty(3), HideLabel]
        public string description;

        [Title("Execution Requirements")]
        [LabelWidth(60)] public int mpCost;
        [LabelWidth(80)] public TargetType targeting;

        [Title("Power & Scaling")]
        public int basePower;
        public List<Skill> AssociatedSkills;

        [Title("Status Effects")]
        public StatusEffectEnum statusEffect;
        [PropertyRange(0, 1)] public float applyChance = 1.0f;

        [Title("Visual & Audio Assets")]
      //  [AssetSelector(Filter = "t:Prefab")]
        public GameObject impactVFX;
       // public string animationTriggerName;
        public AudioClip sfxOnCast;


    }
}