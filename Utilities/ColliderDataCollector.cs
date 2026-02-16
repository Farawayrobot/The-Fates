/// The Fates Narrative Event System
/// ColliderDataCollector
/// Handles and collects data related to collision events,
/// raising events to notify subscribers when collisions occur.
/// 08/01/24
/// by Levi Scully


using System;
using UnityEngine;

namespace TheFates.Utilities
{
    public class ColliderDataCollector : MonoBehaviour
    {
        // Define events for collision
        public event Action OnCollisionEnterEvent;
        public event Action OnCollisionExitEvent;

        
        private void Start()
        {
            // Optionally: Initialize or preload any data needed
        }

        private void FixedUpdate()
        {
            // Optionally: Update any necessary data each frame
        }

        private void OnCollisionEnter(Collision other)
        {
            // Invoke the collision enter event
            OnCollisionEnterEvent?.Invoke();
        }

        private void OnCollisionExit(Collision other)
        {
            // Invoke the collision exit event
            OnCollisionExitEvent?.Invoke();
        }
    }
}
