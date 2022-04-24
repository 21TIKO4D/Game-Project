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

    [SerializeField]
    private List<GameObject> animalUIPrefabs;

    public bool IsPaused { get; set; }

    public GameObject animals;
    public TrainManager trainManager;
    
    public Dictionary<string, int> animalsCount = new Dictionary<string, int>();

    public void LevelStart(TrainManager trainManager)
    {
        this.trainManager = trainManager;
        pauseMenuUI.SetActive(false);
        foreach (Transform animal in animals.transform)
        {
            if (!animalsCount.ContainsKey(animal.name))
            {
                animalsCount.Add(animal.name, 0);
                switch (animal.name.Split('(')[0])
                {
                    case "Cow": Instantiate(animalUIPrefabs[0], trainManager.collectedAnimalsUI.transform); break;
                    case "Horse": Instantiate(animalUIPrefabs[1], trainManager.collectedAnimalsUI.transform); break;
                    case "Sheep": Instantiate(animalUIPrefabs[2], trainManager.collectedAnimalsUI.transform); break;
                    case "Chicken": Instantiate(animalUIPrefabs[3], trainManager.collectedAnimalsUI.transform); break;
                }
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
            int stars = 1;
            float fuelValue = trainManager.fuelBar.slider.normalizedValue;
            if (fuelValue >= 0.45)
            {
                stars = 3;
            } else if (fuelValue >= 0.15) 
            {
                stars = 2;
            }
            gameEndUI.transform.GetChild(0).gameObject.SetActive(true);
            gameEndUI.transform.GetChild(0).GetChild(stars).gameObject.SetActive(true);
            if (stars > PlayerPrefs.GetInt("Level" + LevelLoader.Current.currentLevel, 0))
            {
                PlayerPrefs.SetInt("Level" + LevelLoader.Current.currentLevel, stars);
                PlayerPrefs.Save();
            }
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
