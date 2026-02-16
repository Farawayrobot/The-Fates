/// The Fates Narrative Event System
/// Various data classes for TheFates.Utilities
/// 08/01/24
/// by Levi Scully


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFates.Utilities
{
    /// InstructionMessage
    /// This class represents an instruction message containing text,
    /// an associated audio clip, and the duration for which the message should be displayed.
     
    /// InstructionMessage
    /// This class represents an instruction message containing text,
    /// an associated audio clip, and the duration for which the message should be displayed.
    /// It is used for scripting interactive narratives and guides within the system.
    [Serializable]
    public class InstructionMessage
    {
        public string text;
        public AudioClip audioClip;
        public float messageDuration;
    }
}
