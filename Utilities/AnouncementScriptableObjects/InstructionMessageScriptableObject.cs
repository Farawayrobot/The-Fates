/// Instruction Message Scriptable Object
/// 08/01/24
/// by Levi Scully
/// 

using System;
using System.Collections;
using System.Collections.Generic;
using TheFates.Utilities;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "InstructionMessageSceneList", menuName = "UserStudyTemplate/SceneList")]
public class InstructionMessageScene : ScriptableObject
{
    [SerializeField] public List<InstructionMessage> InstructionMessageSceneList;
}