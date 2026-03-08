using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TheFates.Nona
{
    // We removed the MenuItem here to stop the compilation errors
    [CreateAssetMenu(fileName = "SkillFactory", menuName = "The Fates/Factories/Skill Factory")]
    public class SkillFactory : SerializedScriptableObject 
    {
        [SerializeField, HideInInspector] 
        private string savePath = "TheFates/Nona/SkillsSystem/Skills";

        [Title("Skill Data Entry")]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden, Expanded = true)]
        [SerializeField] private Skill _creationBuffer;

        private void OnEnable()
        {
            if (_creationBuffer == null)
                _creationBuffer = ScriptableObject.CreateInstance<Skill>();
        }

        [Button(ButtonSizes.Large), GUIColor(0, 1, 0.5f)]
        public void BakeSkillToAsset()
        {
#if UNITY_EDITOR
            if (_creationBuffer == null || string.IsNullOrEmpty(_creationBuffer.Name)) return;

            string folderPath = Path.Combine(Application.dataPath, savePath);
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            string assetPath = UnityEditor.AssetDatabase.GenerateUniqueAssetPath($"Assets/{savePath}/{_creationBuffer.Name}.asset");

            Skill newSkillAsset = Instantiate(_creationBuffer);
            UnityEditor.AssetDatabase.CreateAsset(newSkillAsset, assetPath);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();

            _creationBuffer = ScriptableObject.CreateInstance<Skill>();
#endif
        }
    }
}