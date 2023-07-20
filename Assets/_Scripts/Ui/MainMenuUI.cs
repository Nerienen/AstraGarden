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
    private void Start()
    {
        if (startButton != null) startButton.onClick.AddListener(StarGame);
        if (quitButton != null) quitButton.onClick.AddListener(QuitGame);
        if (settingsButton != null) settingsButton.onClick.AddListener(Settings);
    }
    
    private void StarGame()
    {
        startButton.enabled = false;
        SceneManager.LoadScene(1);
    }
    private void QuitGame()
    {
       #if UNITY_WEBGL
           return; 
       #endif
        Application.Quit();
    }
    private void Settings()
    {
        #if UNITY_WEBGL
           settingsMenuWebGL.EnableSettings();
           return; 
        #endif
        settingsMenu.EnableSettings();
    }
    
}
