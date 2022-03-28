using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameEndUI;

    [SerializeField]
    private GameObject nextLevelButton;

    [SerializeField]
    private GameObject PauseMenuUI;

    public bool IsPaused { get; set; }

    public void Start()
    {
        PauseMenuUI.SetActive(false);
    }
    
    public void BackToMainMenu()
    {
        LevelLoader.Current.LoadLevel("Menu");
    }
    
    public void GameEnd(bool completed)
    {
        IsPaused = true;
        gameEndUI.SetActive(true);
        if (completed)
        {
            nextLevelButton.SetActive(true);
        }
        else
        {
            nextLevelButton.SetActive(false);
        }
    }
    
    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        IsPaused = true;
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        IsPaused = false;
    }
}
