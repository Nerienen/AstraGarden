using System.Collections;
using UnityEngine;

public class TestNarrationDatabase : MonoBehaviour
{
    [SerializeField] string[] sentences;
    [SerializeField] LocalizedDynamicText localizedText;

    int _currentLineIndex = 0;

    private void Start()
    {
        StartCoroutine(SendText());
    }

    IEnumerator SendText()
    {
        while (_currentLineIndex < sentences.Length)
        {
            localizedText.DisplayLine(sentences[_currentLineIndex]);

            _currentLineIndex++;
            yield return new WaitForSeconds(3f);
        }
    }
}
