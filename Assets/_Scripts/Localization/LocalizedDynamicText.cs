using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

[RequireComponent(typeof(LocalizeStringEvent))]
public class LocalizedDynamicText : MonoBehaviour
{
    [SerializeField] string stringTableCollectionName = "AssistantLines";

    LocalizeStringEvent _localizeStringEvent;

    private void Awake()
    {
        _localizeStringEvent = GetComponent<LocalizeStringEvent>();
    }

    LocalizedString TranslateLine(string line)
    {
        LocalizedString localizedLine = new LocalizedString(stringTableCollectionName, line);
        return localizedLine;
    }

    void LocalizeLine(string line)
    {
        LocalizedString localizedLine = TranslateLine(line);
        _localizeStringEvent.StringReference = localizedLine;
    }

    /// <summary>
    /// Makes the Localize String Event to translate and update a text element
    /// </summary>
    /// <param name="line">Key to the line to translate</param>
    public void DisplayLine(string line)
    {
        LocalizeLine(line);
    }

    /// <summary>
    /// Change the String Table Collection to use and makes the Localize String Event
    /// to translate and update a text element
    /// </summary>
    /// <param name="stringTableCollectionName">Name of the String Table Collection to use</param>
    /// <param name="line">Key to the line to translate</param>
    public void DisplayLine(string stringTableCollectionName, string line)
    {
        this.stringTableCollectionName = stringTableCollectionName;
        LocalizeLine(line);
    }
}
