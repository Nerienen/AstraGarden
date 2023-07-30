using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct Quest 
{
    [Header("Narration")]
    [SerializeField] private QuestNarrations narration;

    [field:Header("UI text")] 
    [field:SerializeReference] public string title { get; private set; }
    [field:SerializeReference] public string description { get; private set; }

    [Header("Conditions")] 
    public Condition[] conditions;
    
    [Header("Events")] 
    [SerializeField] private UnityEvent onStartQuest;
    [SerializeField] private UnityEvent onCompleteQuest;

    public void StartQuest()
    {
        onStartQuest?.Invoke();
        NarrationManager.instance.StartNarration(narration.narrations);
    }

    public bool CompleteQuest(QuestCondition[] conditions)
    {
        foreach (var condition in this.conditions)
        {
            if (!condition.completed && conditions.Contains(condition.questCondition)) condition.completed = true;
        }
        if (this.conditions.Any(condition => !condition.completed)) return false;
        
        onCompleteQuest?.Invoke();
        return true;
    }
}

[Serializable]
public class Condition
{
    public string description;
    public QuestCondition questCondition;
    public bool completed;
}
