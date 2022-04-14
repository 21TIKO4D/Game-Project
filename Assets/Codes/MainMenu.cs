using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject levelMenu;

    [SerializeField]
    private GameObject optionsMenu;

    [SerializeField]
    private GameObject mainMenu;

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

    [SerializeField] private AudioMixer mixer;
	[SerializeField] private AudioControl musicControl;
	[SerializeField] private AudioControl sfxControl;
	[SerializeField] private string musicVolName;
	[SerializeField] private string sfxVolName;

    public void Start()
    {
        LoadLevels();

        if (PlayerPrefs.GetString("currentUI") == "LevelMenu")
        {
            mainMenu.gameObject.SetActive(false);
            levelMenu.SetActive(true);
        } else if (PlayerPrefs.GetString("currentUI") == "MainMenu")
        {
            levelMenu.SetActive(false);
            mainMenu.gameObject.SetActive(true);
        }

        musicControl.Setup(mixer, musicVolName);
		sfxControl.Setup(mixer, sfxVolName);
    }

    public void CloseOptions()
    {
        mainMenu.gameObject.SetActive(true);
        optionsMenu.SetActive(false);
        musicControl.Save();
		sfxControl.Save();
    }

    public void OpenLevelMenu()
    {
        mainMenu.gameObject.SetActive(false);
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

    private void LoadLevels()
    {
        int i = 1;
        foreach (RectTransform levelIcon in levelMenu.transform.GetChild(0).GetChild(1).transform)
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
    }
}