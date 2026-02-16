/// Announcement System
/// 08/01/24
/// by Levi Scully
///
/// This script is part of the ScryVR Utilities and manages announcements that can be displayed
/// as text on the screen or played as audio. It is implemented as a MonoBehaviour
/// for use in Unity.


using System.Collections.Generic;
using TMPro; // Required for handling text in the user interface
using UnityEngine;
using System;

namespace TheFates.Utilities
{

    
    public class AnnouncementSystem : MonoBehaviour
    {
        // Serialized fields to be set in the Unity Editor.
        [SerializeField] private TMP_Text messageUI; // Text object to display messages
        [SerializeField] private AudioSource source; // AudioSource component for playing sounds
        [SerializeField] private List<InstructionMessageScene> storedSceneLists; // Stores message scenes, instantiated in Start

        // Unity Lifecycle function called when the script instance is being loaded.
        public void Start()
        {
            storedSceneLists = new List<InstructionMessageScene>(); // Initializes the list to store message scenes
        }

        // Unity Lifecycle functions could go here to manage updates, currently not in use.
        private void Update()
        {
            // Intentionally left empty - possible future updates or animations related to messages.
        }
        
        // Functions for managing audio and text messages
        
        // Method to display a simple text message on the UI
        public void DisplayTextMessage(string text)
        {
            messageUI.text = text;
        }
        
        // Method to clear the currently displayed text message from the UI
        public void ClearMessage()
        {
            messageUI.text = ""; // Resets the text content to an empty string
        }

        // Method to play an audio clip using the configured AudioSource
        public void PlaySound(AudioClip sound)
        {
            source.clip = sound; // Sets the current clip to play
            source.Play(); // Plays the clip
        }
        
        // Method to simultaneously display a text message and play an accompanying audio clip
        public void PlayMessage(string text, AudioClip voiceClip)
        {
            source.clip = voiceClip;
            source.Play();
            messageUI.text = text; // Displays the text while the audio plays
        }
    }
}


