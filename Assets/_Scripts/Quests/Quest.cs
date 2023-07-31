using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct Quest 
{
    [field: Header("Data")]
    [field: SerializeReference] public QuestData questData { get; private set; }
    
    [Header("Events")] 
    [SerializeField] private UnityEvent onStartQuest;
    [SerializeField] private UnityEvent onCompleteQuest;

    public void StartQuest()
    {
        onStartQuest?.Invoke();
        NarrationManager.instance.StartNarration(questData.narration.narrations);
    }

    public bool CompleteQuest(QuestObjective[] objectives)
    {
        foreach (var objective in questData.objectives)
        {
            if (!objective.completed && objectives.Contains(objective.questObjective)) objective.completed = true;
        }
        if (questData.objectives.Any(condition => !condition.completed)) return false;
        
        onCompleteQuest?.Invoke();
        return true;
    }
}


