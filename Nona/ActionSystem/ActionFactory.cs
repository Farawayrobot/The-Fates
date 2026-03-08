using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TheFates.Nona
{
    [CreateAssetMenu(fileName = "ActionFactory", menuName = "The Fates/Factories/Action Factory")]
    public class ActionFactory : SerializedScriptableObject 
    {
        [SerializeField, HideInInspector] 
        private string savePath = "TheFates/Nona/ActionSystem/Actions";

        [Title("Action Data Entry")]
        [InfoBox("Fill out the details below and click Bake to create the permanent Action asset.")]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden, Expanded = true)]
        [SerializeField] private ActionScriptableObject _creationBuffer;

        private void OnEnable()
        {
            // Initializes the buffer so you have a clean slate in the inspector
            if (_creationBuffer == null)
                _creationBuffer = ScriptableObject.CreateInstance<ActionScriptableObject>();
        }

        [Button(ButtonSizes.Large), GUIColor(0, 1, 0.5f)]
        public void BakeActionToAsset()
        {
#if UNITY_EDITOR
            if (_creationBuffer == null || string.IsNullOrEmpty(_creationBuffer.actionName))
            {
                Debug.LogWarning("Action Name is empty! Cannot bake asset.");
                return;
            }

            // Ensure the directory exists
            string folderPath = Path.Combine(Application.dataPath, savePath);
            if (!Directory.Exists(folderPath)) 
                Directory.CreateDirectory(folderPath);

            // Create a unique path based on the action name
            string fileName = _creationBuffer.actionName.Replace(" ", "_");
            string assetPath = UnityEditor.AssetDatabase.GenerateUniqueAssetPath($"Assets/{savePath}/{fileName}.asset");

            // Clone the buffer data into a new permanent asset
            ActionScriptableObject newActionAsset = Instantiate(_creationBuffer);
            
            UnityEditor.AssetDatabase.CreateAsset(newActionAsset, assetPath);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();

            // Wipe the buffer for the next entry
            _creationBuffer = ScriptableObject.CreateInstance<ActionScriptableObject>();
            Debug.Log($"Successfully baked Action: {fileName} to {assetPath}");
#endif
        }
    }
}