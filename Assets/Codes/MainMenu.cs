using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject levelMenu;

    public void Start()
    {
        if (PlayerPrefs.GetString("currentUI") == "LevelMenu")
        {
            this.gameObject.SetActive(false);
            levelMenu.SetActive(true);
        } else if (PlayerPrefs.GetString("currentUI") == "MainMenu")
        {
            levelMenu.SetActive(false);
            this.gameObject.SetActive(true);
        }
    }

    public void OpenLevelMenu()
    {
        this.gameObject.SetActive(false);
        levelMenu.SetActive(true);
        PlayerPrefs.SetString("currentUI", "LevelMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartLevel(GameObject levelData)
    {
        Instantiate(levelData, LevelLoader.Current.transform);
        LevelLoader.Current.LoadScene("LevelTemplate");
    }
}