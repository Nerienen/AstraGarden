using ProjectUtils.Helpers;
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
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    [Header("Selectables")]
    [SerializeField] private Selectable firstPauseSelectable;
    [SerializeField] private Selectable firstSettingsSelectable;
    [SerializeField] private Selectable firstSettingsWebGLSelectable;

    private void Start()
    {
        if (resumeButton != null) resumeButton.onClick.AddListener(Resume);
        if (settingsButton != null) settingsButton.onClick.AddListener(Settings);
        if (quitButton != null) quitButton.onClick.AddListener(HomeButtonAction);
        if (restartButton != null) restartButton.onClick.AddListener(RestartButtonAction);
    }

    public void PauseGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        FindObjectOfType<EventSystem>().SetSelectedGameObject(null);
        Time.timeScale = 0;

        if (pauseMenu.TryGetComponent(out CanvasGroup canvasGroup))
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            firstPauseSelectable.Select();
        }

        UiController.instance.onSetPaused?.Invoke(true);
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
        
        if (pauseMenu.TryGetComponent(out CanvasGroup canvasGroup))
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            EventSystem.current.SetSelectedGameObject(null);
        }

        UiController.instance.onSetPaused?.Invoke(false);
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
           firstSettingsWebGLSelectable.Select();

           return; 
#else
        settingsMenu.EnableSettings();
        firstSettingsSelectable.Select();
#endif
    }
}
