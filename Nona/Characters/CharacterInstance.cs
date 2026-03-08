using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace TheFates.Nona
{
    public class CharacterInstance : MonoBehaviour
    {
        [Title("Data Source")]
        [Required] public CharacterSheet characterSheet;

        [Title("Live Stats")]
        [ReadOnly] public int currentHP;
        [ReadOnly] public int currentMP;
        
        [PropertySpace(10)]
        public List<StatusEffectEnum> activeStatuses = new List<StatusEffectEnum>();

        [Title("Scene Reference")]
        [ReadOnly, SceneObjectsOnly]
        public GameObject spawnedObject;

        [Button(ButtonSizes.Large, Name = "Spawn / Reset Object")]
        [GUIColor(0, 1, 0.5f)]
        public void SpawnObject()
        {
#if UNITY_EDITOR
            if (characterSheet == null || characterSheet.characterPrefab == null)
            {
                Debug.LogError("Assign a CharacterSheet with a valid prefab first!");
                return;
            }

            // Cleanup old instance if it exists
            if (spawnedObject != null)
                DestroyImmediate(spawnedObject);

            // Instantiate from the sheet's prefab
            spawnedObject = UnityEditor.PrefabUtility.InstantiatePrefab(characterSheet.characterPrefab) as GameObject;
            
            // Parent to this locator for spatial management
            spawnedObject.transform.SetParent(this.transform);
            spawnedObject.transform.localPosition = Vector3.zero;
            spawnedObject.transform.localRotation = Quaternion.identity;
            
            // Set initial runtime values from the sheet
            SyncWithSheet();
            
            // Keep it disabled so the Questline can activate it when ready
            spawnedObject.SetActive(false);
#endif
        }

        public void SyncWithSheet()
        {
            if (characterSheet == null) return;
            
            // Pulling from your Stat class in the CharacterSheet
            currentHP = (int)characterSheet.healthPoints.GetBaseValue();
            currentMP = (int)characterSheet.manaPoints.GetBaseValue();
        }

        public void Activate()
        {
            if (spawnedObject != null)
            {
                spawnedObject.SetActive(true);
            }
        }
    }
}