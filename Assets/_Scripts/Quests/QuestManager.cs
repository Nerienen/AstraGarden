using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private Quest[] quests;
    private int _currentQuestIndex;

    private readonly HashSet<QuestObjective> _conditions = new();

    public static QuestManager instance { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        StartCurrentQuest();
    }

    public void StartCurrentQuest()
    {
        if(_currentQuestIndex >= quests.Length) return;
        
        StartQuest(quests[_currentQuestIndex]);
    }

    public void StartQuest(Quest quest)
    {
        _conditions.Clear();
        quest.StartQuest();
    }

    public void TryCompleteCurrentQuest()
    {
        if (quests[_currentQuestIndex].CompleteQuest(_conditions.ToArray()))
        {
            ContinueQuestLine(_currentQuestIndex++);
        }    
    }
    
    public void AddCondition(QuestObjective objective)
    {
        _conditions.Add(objective);
    }

    private void ContinueQuestLine(int index)
    {
        if(index is 0 or 2 or 3) StartCurrentQuest();
    }
}
