using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory
{
    private Dictionary<string, int> animals = new Dictionary<string, int>();

    private GameObject collectedAnimalsUI;
    private GameManager gameManager;
    
    public Inventory(GameObject collectedAnimalsUI, GameManager gameManager)
    {
        this.collectedAnimalsUI = collectedAnimalsUI;
        this.gameManager = gameManager;
        foreach(Transform child in collectedAnimalsUI.transform)
        {
            animals.Add(child.name, 0);
        }
        UpdateUI();
    }

    public void UpdateAnimalCountsToUI(string collectedName)
    {
        animals[collectedName] += 1;
        UpdateUI();
    }

    public int GetMaxCount(string name)
    {
        return gameManager.animalsCount[name];
    }

    private void UpdateUI()
    {
        foreach(Transform child in collectedAnimalsUI.transform)
        {
            string name = child.name;
            child.transform.GetChild(1).GetComponent<TMP_Text>().text = animals[name].ToString() + "/" + GetMaxCount(name);
        }
    }
}
