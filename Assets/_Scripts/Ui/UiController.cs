using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UiController : MonoBehaviour
    {
        [SerializeField] private PauseMenuUI pauseMenu;
        [SerializeField] private GameObject displayTab;
        [SerializeField] private GameObject displayE;
        public static UiController instance;
        public bool eDisplayed;

        public Action<bool> onSetPaused; 

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
        }

        private void Start()
        {
            Time.timeScale = 1;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !CutsceneController.Instance.isPlayingCutscene)
            {
                if (Time.timeScale == 1) pauseMenu.PauseGame();
                else pauseMenu.Resume();
            }
        }

        public void DisplayInspectPlant()
        {
            displayTab.SetActive(true);
            StartCoroutine(DisableTab());
        }

        private IEnumerator DisableTab()
        {
            yield return new WaitForSeconds(3.5F);
            displayTab.SetActive(false);
        }
        
        public void DisplayInteract()
        {
            displayE.SetActive(true);
            StartCoroutine(DisableE());
        }

        private IEnumerator DisableE()
        {
            yield return new WaitForSeconds(3.5f);
            displayE.SetActive(false);
            yield return new WaitForSeconds(2);
            eDisplayed = true;
        }
    }


