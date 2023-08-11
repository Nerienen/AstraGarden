using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Bindings")] 
    [SerializeField] private SettingsMenu settingsMenu;
    [SerializeField] private SettingsMenu settingsMenuWebGL;
    
    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    [Header("Selectables")]
    [SerializeField] private Selectable firstSettingsSelectable;
    [SerializeField] private Selectable firstSettingsWebGLSelectable;

    private void Start()
    {
        if (startButton != null) startButton.onClick.AddListener(StartGame);
        if (quitButton != null) quitButton.onClick.AddListener(QuitGame);
        if (settingsButton != null) settingsButton.onClick.AddListener(Settings);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    
    private void StartGame()
    {
        startButton.enabled = false;
        SceneManager.LoadScene(1);
    }
    private void QuitGame()
    {
#if UNITY_WEBGL
           return; 
#elif UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void Settings()
    {
#if UNITY_WEBGL
        settingsMenuWebGL.EnableSettings();
        firstSettingsWebGLSelectable.Select();
        return; 
#else
        settingsMenu.EnableSettings();
        firstSettingsSelectable.Select();
#endif
    }
    
}
