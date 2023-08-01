using System;
using System.Collections;
using System.Collections.Generic;
using ProjectUtils.Helpers;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    
    [Header("Bindings")]
    [SerializeField] private Transform objectives;
    [SerializeField] private GameObject objectiveUiPrefab;
    [SerializeField] private GameObject uiHolder;

    private void Start()
    {
        QuestManager.instance.onStartQuest += StartQuest;
        QuestManager.instance.onTryCompleteQuest += CheckObjectives;
    }

    private void CheckObjectives(Objective[] obj)
    {
        foreach (Transform objective in objectives)
        {
            objective.GetComponent<ObjectiveUI>().SetCheck(obj[objective.GetSiblingIndex()].completed);
        }
    }

    private void StartQuest(Quest quest)
    {
        uiHolder.SetActive(true);
        objectives.DeleteChildren();
        
        QuestData data = quest.questData;

        title.text = data.title;
        foreach (var objective in data.objectives)
        {
            GameObject temp = Instantiate(objectiveUiPrefab, objectives);
            temp.GetComponent<ObjectiveUI>().SetText(objective.description);
        }
    }
    
}
