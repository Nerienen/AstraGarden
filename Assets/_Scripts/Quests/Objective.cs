using System;

[Serializable]
public class Objective
{
    public string description;
    public QuestObjective questObjective;
    public bool completed;

    public Objective(string description, QuestObjective questObjective)
    {
        this.description = description;
        this.questObjective = questObjective;
    }
}
