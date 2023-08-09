using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class LocalizedDropdown : MonoBehaviour
{
    [SerializeField] string stringTableName = "Menu";

    TMP_Dropdown _dropdown;
    bool _previousIsExpanded;

    private void Start()
    {
        _dropdown = GetComponent<TMP_Dropdown>();
    }

    private void Update()
    {
        if (_previousIsExpanded == false && _dropdown.IsExpanded)
        {
            ProcessExpandedDropdown();
        }
        _previousIsExpanded = _dropdown.IsExpanded;
    }

    private void ProcessExpandedDropdown()
    {
        GameObject[] itemLabels = transform.GetComponentsInChildren<Transform>()
            .Where(t => t.gameObject.name == "Item Label")
            .Select(t => t.gameObject)
            .ToArray();

        foreach (var itemLabel in itemLabels)
        {
            LocalizeStringEvent localizeStringEvent = itemLabel.AddComponent<LocalizeStringEvent>();
            TextMeshProUGUI text = itemLabel.GetComponent<TextMeshProUGUI>();

            localizeStringEvent.SetTable(stringTableName);
            LocalizedString localizedString = new(stringTableName, text.text);
            localizeStringEvent.StringReference = localizedString;
            localizeStringEvent.OnUpdateString.AddListener((string value) =>
            {
                text.text = value;
            });
        }
    }
}
