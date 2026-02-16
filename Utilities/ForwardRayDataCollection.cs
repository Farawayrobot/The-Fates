/// The Fates Narrative Event System
/// ForwardRayDataCollector
/// Performs raycasting to detect objects in the forward direction
/// and triggers events when objects are hit.
/// 08/01/24
/// by Levi Scully

using System;
using UnityEngine;

namespace TheFates.Utilities
{
    /// <summary>
    /// Attach this to the head to generate a low accuracy Gaze path
    /// that emits events on object detection through raycasting.
    /// </summary>
    public class ForwardRayDataCollector : MonoBehaviour
    {
        // Event triggered when an object is hit by the raycast
        public event Action OnRaycastHitEvent;
        [SerializeField] private EventTimer sampleTimer;
        private Transform _transform;
        [SerializeField] private LayerMask layerMask;

        private void Start()
        {
            // Cache the transform for optimization
            _transform = transform;
        }

        private void Awake()
        {
            // Subscribe to the timer event to conduct raycasting
            sampleTimer.OnTimerComplete += RayCast;
        }

        private void OnDestroy()
        {
            // Unsubscribe from the timer event to avoid memory leaks
            sampleTimer.OnTimerComplete -= RayCast;
        }

        /// <summary>
        /// Performs raycasting in the forward direction to detect objects.
        /// Triggers OnRaycastHitEvent if an object is hit.
        /// </summary>
        private void RayCast()
        {
            RaycastHit hit;
            if (Physics.Raycast(_transform.position, _transform.forward, out hit, Mathf.Infinity, layerMask))
            {
                // Notify subscribers about the hit event
                OnRaycastHitEvent?.Invoke();

            }
        }
    }
}
