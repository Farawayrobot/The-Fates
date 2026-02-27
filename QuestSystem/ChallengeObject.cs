using Sirenix.OdinInspector;
using UnityEngine;
using TheFates; // Use your namespace

public class ChallengeObject : MonoBehaviour
{
    [Sirenix.OdinInspector.ReadOnly]
    [HideInInspector] public Challenge data; // The reference to your serializable class
    [Button(ButtonSizes.Small), FoldoutGroup("Debug Events")]
    public void ManualStarted() => data.ChallengeStartedEventBroadcast();
    
    [Button(ButtonSizes.Small), FoldoutGroup("Debug Events")]
    public void ManualComplete() => data.ChallengeCompletedEventBroadcast();

    [Button(ButtonSizes.Small), FoldoutGroup("Debug Events")]
    public void ManualEventSubscribe() => SubscribeToChallengeDebugEvents();
    public void Initialize(Challenge challengeData)
    {
        this.data = challengeData;
        
        // Grab all names safely
        string qlName = data.ParentQuestLine?.questLineName ?? "NoLine";
        string qName  = data.ParentQuest?.questName ?? "NoQuest";
        string sName  = data.ParentStage?.stageName ?? "NoStage";
        string cDesc  = data.challengeName;

        // Result: [Main Story] Farmer Jon: Stage 1 | Collect Wool
        this.gameObject.name = $"-- ] {qlName} {qName} : {sName} | {cDesc} [ --";
        SubscribeToChallengeDebugEvents();

    }

    private void SubscribeToChallengeDebugEvents()
    {
        // Subscribe to the events you wrote!
        data.ChallengeHasStarted += OnChallengeStarted;
        data.ChallengeHasCompleted += OnChallengeCompleted;
    }

    private void OnChallengeStarted(Challenge c)
    {
        Debug.Log($"GO {name} reacted to Start!");
        
        // Play particles, enable a trigger, etc.
    }

    private void OnChallengeCompleted(Challenge c)
    {
        Debug.Log($"GO {name} reacted to Complete!");
        // Disable the object, play a sound, etc.
    }

    private void OnDestroy()
    {
        if (data != null)
        {
            data.ChallengeHasStarted -= OnChallengeStarted;
            data.ChallengeHasCompleted -= OnChallengeCompleted;
        }
    }
}