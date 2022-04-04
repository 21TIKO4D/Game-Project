using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject levelMenu;

    public void Start()
    {
        int i = 0;
        foreach (RectTransform levelIcon in levelMenu.transform.GetChild(1).transform)
        {
            if (PlayerPrefs.HasKey("Level" + i))
            {
                levelIcon.GetComponent<Button>().interactable = true;
            }
            else
            {
                levelIcon.GetComponent<Button>().interactable = false;
            }
            i++;
        }
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

    public void StartLevel(int level)
    {
        LevelLoader.Current.LoadScene("LevelTemplate", level);
    }
}