using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class QuestData : ScriptableObject
{
    [Header("Narration")]
    public QuestNarrations narration;

    [field: Header("UI text")] 
    public string title;
    public string description;

    [Header("Conditions")] 
    public Objective[] objectives;

    public QuestData(QuestNarrations narration, string title, string description, Objective[] objectives)
    {
        this.narration = narration;
        this.title = title;
        this.description = description;
        this.objectives = objectives;
    }
}
