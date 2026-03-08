using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TheFates.Nona
{
    [CreateAssetMenu(fileName = "StoreFactory", menuName = "Fates/Factories/Store Factory")]
    public class StoreFactory : SerializedScriptableObject
    {
        [Title("Store Creator")]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        [SerializeField] private StoreScriptableObject storePreview;

#if UNITY_EDITOR
        

        [SerializeField, HideInInspector] private string savePath = "Assets/TheFates/Nona/Items/Stores";

        private void OnEnable()
        {
            if (storePreview == null) CreateNewPreview();
        }

        private void CreateNewPreview()
        {
            storePreview = ScriptableObject.CreateInstance<StoreScriptableObject>();
            storePreview.storeName = "New Store";
            storePreview.currentGold = 1000;
        }

        [Button(ButtonSizes.Large, Name = "Finalize and Save Store")]
        [GUIColor(0.4f, 1f, 0.4f)]
        public void CreateStore()
        {
            if (string.IsNullOrEmpty(storePreview.storeName))
            {
                Debug.LogError("Store Name cannot be empty!");
                return;
            }

            if (!AssetDatabase.IsValidFolder(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            string fileName = storePreview.storeName.Replace(" ", "_") + ".asset";
            string fullPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(savePath, fileName));

            // Create a permanent copy of the preview
            StoreScriptableObject finalAsset = Instantiate(storePreview);
            AssetDatabase.CreateAsset(finalAsset, fullPath);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = finalAsset;

            CreateNewPreview();
        }
#endif
    }
}