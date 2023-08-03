using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageSwitcher : MonoBehaviour
{
    /// <summary>
    /// Switch to a specific language using the index of the LocalizationSettings' language list
    /// </summary>
    /// <param name="languageIndex">Index of the LocalizationSettings' language list</param>
    public void SwitchLanguage(int languageIndex)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageIndex];
    }
}
