using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private Quest[] quests;
    private int _currentQuestIndex;

    private readonly HashSet<QuestObjective> _objectives = new();
    private Objective[] _objectivesData;

    public event Action<Quest> onStartQuest;
    public event Action<Objective[]> onTryCompleteQuest;

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
        _objectives.Clear();
        quest.StartQuest();
        onStartQuest?.Invoke(quest);
    }
    
    public void TryCompleteCurrentQuest(QuestObjective objective)
    {
        if(_currentQuestIndex >= quests.Length) return;

        _objectives.Add(objective);

        bool questCompleted = quests[_currentQuestIndex].CompleteQuest(_objectives.ToArray(), out _objectivesData);
        onTryCompleteQuest?.Invoke(_objectivesData);
        if (questCompleted)
        {
            ContinueQuestLine(_currentQuestIndex++);
        }
        
    }
    private void ContinueQuestLine(int index)
    {
        if(index is 0 or 2 or 3) StartCurrentQuest();
    }
}
