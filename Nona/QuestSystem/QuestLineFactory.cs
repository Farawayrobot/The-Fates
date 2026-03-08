using Sirenix.OdinInspector;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Linq;
using TheFates.Nona.QuestSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TheFates.Nona
{
    [CreateAssetMenu(fileName = "QuestFactory", menuName = "The Fates/Factories/Quest Factory")]
    public class QuestFactory : SerializedScriptableObject
    {
        // --- SECTION 1: THE WORKBENCH ---
        [BoxGroup("Weave Quest Line")]
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden, Expanded = true)]
        [SerializeField] private QuestLine _creationBuffer;

        [SerializeField, HideInInspector] private string savePath = "TheFates/Nona/QuestSystem/QuestLines";

        private void OnEnable()
        {
            if (_creationBuffer == null)
                _creationBuffer = ScriptableObject.CreateInstance<QuestLine>();
        }

        [BoxGroup("Weave Quest Line")]
        [Button(ButtonSizes.Large), GUIColor(0, 1, 0.5f)]
        public void ForgeQuestLineAsset()
        {
#if UNITY_EDITOR
            if (_creationBuffer == null || string.IsNullOrEmpty(_creationBuffer.questLineName)) return;

            string folderPath = Path.Combine(Application.dataPath, savePath);
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            QuestLine finalAsset = Instantiate(_creationBuffer);
            string fileName = _creationBuffer.questLineName.Replace(" ", "_");
            string assetPath = AssetDatabase.GenerateUniqueAssetPath($"Assets/{savePath}/{fileName}.asset");

            AssetDatabase.CreateAsset(finalAsset, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            _creationBuffer = ScriptableObject.CreateInstance<QuestLine>();
#endif
        }

        // --- SECTION 2: THE LOOM ---
        [Space(10)]
        [BoxGroup("Loom Quest Line")]
        [ValueDropdown("GetQuestLineAssets")]
        public QuestLine questLineToWeave;

        [BoxGroup("Loom Quest Line")]
        [Button(ButtonSizes.Large, Name = "Weave Logic"), GUIColor(0.5f, 0.8f, 1f)]
        [EnableIf("questLineToWeave")]
        public void WeaveLogic()
        {
#if UNITY_EDITOR
            if (questLineToWeave == null) return;

            string folderPath = Path.Combine(Application.dataPath, "TheFates", "Nona", "QuestSystem", "QuestLines");
            string scriptFileName = "WovenFatesOf" + questLineToWeave.questLineName.Replace(" ", "") + ".cs"; 
            string fullPath = Path.Combine(folderPath, scriptFileName);

            if (File.Exists(fullPath)) return;

            Loom.Instance.WeaveFullQuestLine(questLineToWeave);
            AssetDatabase.Refresh();
#endif
        }

        private IEnumerable GetQuestLineAssets()
        {
#if UNITY_EDITOR
            return AssetDatabase.FindAssets("t:QuestLine")
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Select(path => new ValueDropdownItem(
                    Path.GetFileNameWithoutExtension(path), 
                    AssetDatabase.LoadAssetAtPath<QuestLine>(path)
                ));
#else
            return null;
#endif
        }
    }
}