/// The Fates Narrative Event System
/// ObjectMovementThreshold
/// Monitors an object's position and rotation to trigger events
/// when movement or rotation exceeds a defined threshold.
/// 08/01/24
/// by Levi Scully

using System;
using UnityEngine;

namespace TheFates.Utilities
{

    public class ObjectMovementThreshold : MonoBehaviour
    {
        // The threshold for triggering movement events
        [SerializeField] private float movementThreshold = 0.1f;

        // Event triggered when object movement exceeds the threshold
        public event Action OnObjectMovedFurtherThanThreshold;

        // Stores the last known position and rotation
        private Vector3 lastPosition;
        private Vector3 lastRotation;
        [SerializeField] private EventTimer timer;

        private void Start()
        {
            // Initialize last position and rotation with current values
            lastPosition = transform.position;
            lastRotation = transform.eulerAngles;

            // Ensure the threshold is non-negative
            if (movementThreshold < 0) 
                movementThreshold = 0.1f; // Default to a minimal threshold if invalid
        }

        private void OnEnable()
        {
            timer.OnTimerComplete += CheckMovementThreshold;
        }        
        
        private void OnDisable()
        {
            timer.OnTimerComplete -= CheckMovementThreshold;
        }

        private void FixedUpdate()
        {
            // check the object's movement every fixed frame
            //CheckMovementThreshold();
        }

        /// <summary>
        /// Checks if the object has moved or rotated beyond the set threshold.
        /// Triggers an event if the threshold is exceeded.
        /// </summary>
       
        private void CheckMovementThreshold()
        {
            float distanceMoved = Vector3.Distance(transform.position, lastPosition);
            float distanceRotated = Vector3.Distance(transform.eulerAngles, lastRotation);

            // Check if movement or rotation exceeds the threshold
            if (distanceMoved > movementThreshold || distanceRotated > movementThreshold)
            {
                // Update the last known position and rotation
                lastPosition = transform.position;
                lastRotation = transform.eulerAngles;

                // Invoke the movement threshold event
                OnObjectMovedFurtherThanThreshold?.Invoke();
            }
        }

        /// <summary>
        /// Resets the tracking positions and angle.
        /// Useful if the object is teleported or manually repositioned.
        /// </summary>
        public void ResetTracking()
        {
            lastPosition = transform.position;
            lastRotation = transform.eulerAngles;
        }
    }
}