using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject levelMenu;

    [SerializeField]
    private GameObject locked;

    [SerializeField]
    private GameObject stars0;

    [SerializeField]
    private GameObject stars1;

    [SerializeField]
    private GameObject stars2;

    [SerializeField]
    private GameObject stars3;

    public void Start()
    {
        int i = 1;
        foreach (RectTransform levelIcon in levelMenu.transform.GetChild(1).transform)
        {
            Destroy(levelIcon.transform.GetChild(1).gameObject);
            if (i == 1 || PlayerPrefs.HasKey("Level" + (i-1)))
            {
                levelIcon.GetComponent<Button>().interactable = true;
                switch (PlayerPrefs.GetInt("Level" + i, 0))
                {
                    case 0: Instantiate(stars0, levelIcon.transform); break;
                    case 1: Instantiate(stars1, levelIcon.transform); break;
                    case 2: Instantiate(stars2, levelIcon.transform); break;
                    case 3: Instantiate(stars3, levelIcon.transform); break;
                };
            }
            else
            {
                levelIcon.GetComponent<Button>().interactable = false;
                Instantiate(locked, levelIcon.transform);
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