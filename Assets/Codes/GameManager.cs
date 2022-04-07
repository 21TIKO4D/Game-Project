using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameEndUI;

    [SerializeField]
    private GameObject pauseMenuUI;

    [SerializeField]
    public Grid tileMapGrid;

    public bool IsPaused { get; set; }

    public GameObject animals;
    public TrainManager TrainManager;
    
    private Dictionary<string, int> animalsCount = new Dictionary<string, int>();

    public void LevelStart()
    {
        pauseMenuUI.SetActive(false);
        foreach (Transform animal in animals.transform)
        {
            if (!animalsCount.ContainsKey(animal.name))
            {
                animalsCount.Add(animal.name, 0);
            }
            animalsCount[animal.name] = animalsCount[animal.name] + 1;
        }
    }
    
    public void BackToMainMenu()
    {
        LevelLoader.Current.LoadScene("Menu");
    }

    public void CheckAnimalCount(Dictionary<string, int> collectedAnimals)
    {
        bool completed = true;
        foreach (KeyValuePair<string, int> animal in animalsCount)
        {
            if (collectedAnimals.ContainsKey(animal.Key))
            {
                if (collectedAnimals[animal.Key] < animal.Value)
                {
                    completed = false;
                }
            }
            else
            {
                completed = false;
            }
        }
        if (completed)
        {
            GameEnd(true);
        }
    }

    public void GameEnd(bool completed)
    {
        IsPaused = true;
        if (completed)
        {
            gameEndUI.transform.GetChild(0).gameObject.SetActive(true);
            PlayerPrefs.SetInt("Level" + LevelLoader.Current.currentLevel, 1);
            PlayerPrefs.Save();
        }
        else
        {
            gameEndUI.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void NextLevel()
	{
        LevelLoader.Current.currentLevel++;
		LevelLoader.Current.LoadScene("LevelTemplate", LevelLoader.Current.currentLevel);
	}
    
    public void Pause()
    {
        if (IsPaused) return;

        pauseMenuUI.SetActive(true);
        IsPaused = true;
    }

    public void RestartLevel()
    {
        LevelLoader.Current.LoadScene("LevelTemplate", LevelLoader.Current.currentLevel);
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        IsPaused = false;
    }
}
