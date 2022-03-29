using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject animals;

    [SerializeField]
    private GameObject gameEndUI;

    [SerializeField]
    private GameObject nextLevelButton;

    [SerializeField]
    private GameObject pauseMenuUI;

    private Dictionary<string, int> animalsCount = new Dictionary<string, int>();

    public bool IsPaused { get; set; }

    public void Start()
    {
        pauseMenuUI.SetActive(false);
        foreach (Transform animalType in animals.transform)
        {
            foreach (Transform animal in animalType.transform)
            {
                if (!animalsCount.ContainsKey(animal.name))
                {
                    animalsCount.Add(animal.name, 0);
                }
                animalsCount[animal.name] = animalsCount[animal.name] + 1;
            }
        }
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
    
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        IsPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        IsPaused = false;
    }
}
