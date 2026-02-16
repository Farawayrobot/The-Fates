/// The Fates Narrative Event System
/// EventTimer
/// Manages and triggers events based on timed conditions.
/// This class allows subscription to specific time-related events,
/// supporting gameplay mechanics that require timed execution.
/// 08/01/24 - Major edits finished 2/15/26
/// by Levi Scully

using System;
using UnityEngine;

namespace TheFates.Utilities
{
    public enum TimerAccuray
    {
        Low,
        Medium,
        High
    };
    
    public class EventTimer : MonoBehaviour
    {
        // Event triggered when the timer reaches a specific duration
        public event Action OnTimerComplete;

        // Duration for the timer in seconds
        [SerializeField] private float timerDuration = 10.0f;

        // Internal tracking of time elapsed
        private float targetTime = 0.0f;

        // Indicates if the timer is running
        private bool timerRunning = false;
        
        //
        [SerializeField] private bool isTimerOnRepeat = false;
        [Header("Timer accuracy in Seconds: low (0.50), medium (0.10), high (0.02) ")]
        [SerializeField] private TimerAccuray timerAccuracy = TimerAccuray.Low;

        private int frameSkipCount = 0;
        private int frameCount = 0;
            
        private void Start()
        {                
            switch (timerAccuracy)
            {
                case TimerAccuray.Low:
                    frameSkipCount = 25; //checks every 0.50 seconds
                    break; 
                case TimerAccuray.Medium: // checks every 0.10 seconds
                    frameSkipCount = 5;
                    break;
                case TimerAccuray.High: //checks every 0.02 seconds
                    frameSkipCount = 0;
                    break;
            }
            // Optionally: Initialize timer conditions
            // StartTimer(); // Uncomment if the timer should start automatically
        }

        private void FixedUpdate()
        {
            frameCount++;
            if (timerRunning && frameSkipCount < frameCount)
            {
                frameCount = 0;
                // Check if the timer has reached the specified duration
                if (targetTime <= Time.time)
                {
                    TimerComplete();
                }
            }
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void StartTimer()
        {
            if (!timerRunning)
            {
                targetTime = Time.time + timerDuration;
                timerRunning = true;
            }
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void StopTimer()
        {
            timerRunning = false;
        }     
        
        /// <summary>
        /// Sets the timer duration
        /// </summary>
        public void SetTimerDuration(float newDuration)
        {
            timerDuration = newDuration;
        }
        
        /// <summary>
        /// Invoked when the timer completes its duration.
        /// Triggers the OnTimerComplete event.
        /// </summary>
        private void TimerComplete()
        {
            StopTimer(); // Stop the timer
            // Invoke the timer complete event
            OnTimerComplete?.Invoke();
            if (isTimerOnRepeat)
            {
                StartTimer();
            }
            // Additional logic upon timer completion can be added here
        }
    }
}
