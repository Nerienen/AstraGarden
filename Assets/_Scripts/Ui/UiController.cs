using System;
using TMPro;
using UnityEngine;

public class UiController : MonoBehaviour
    {
        [SerializeField] private PauseMenuUI pauseMenu;

        private void Start()
        {
            Time.timeScale = 1;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Time.timeScale == 1)
                {
                    pauseMenu.PauseGame();
                }
                else pauseMenu.Resume();
            }
        }
}


