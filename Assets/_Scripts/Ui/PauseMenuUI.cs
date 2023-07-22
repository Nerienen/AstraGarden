using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
     [Header("Bindings")]
     [SerializeField] private GameObject pauseMenu;
     [SerializeField] private SettingsMenu settingsMenu;
     [SerializeField] private SettingsMenu settingsMenuWebGL;

     [Header("Buttons")]
     [SerializeField] private Button startButton;
     [SerializeField] private Button resumeButton;
     [SerializeField] private Button settingsButton;
     [SerializeField] private Button restartButton;
     [SerializeField] private Button quitButton;
     
     private void Start()
     {
         if (resumeButton != null) resumeButton.onClick.AddListener(Resume);
         if (settingsButton != null) settingsButton.onClick.AddListener(Settings);
         if (quitButton != null) quitButton.onClick.AddListener(HomeButtonAction);
         if (restartButton != null) restartButton.onClick.AddListener(RestartButtonAction);
         if (startButton != null) startButton.onClick.AddListener(StartGame);
     }

     public void PauseGame()
     {
         Cursor.visible = true;
         Cursor.lockState = CursorLockMode.None;
         
          FindObjectOfType<EventSystem>().SetSelectedGameObject(null);
         Time.timeScale = 0;
         pauseMenu.SetActive(true);
     }

     public void StartGame()
     {
         SceneManager.LoadScene(1);
     }
     
     public void Resume()
     {
         Time.timeScale = 1;
         Cursor.visible = false;
         Cursor.lockState = CursorLockMode.Locked;
         
         settingsMenu.DisableSettings();
         settingsMenuWebGL.DisableSettings();
         pauseMenu.SetActive(false);
     }

     private void HomeButtonAction()
     {
         quitButton.enabled = false;
         SceneManager.LoadScene(0);
     }

     private void RestartButtonAction()
     {
         restartButton.enabled = false;
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
