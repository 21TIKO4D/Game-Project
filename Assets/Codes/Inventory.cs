using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory
{
    private Dictionary<string, int> animals = new Dictionary<string, int>();
    
    public void UpdateAnimalCountsToUI(string collectedName, GameObject collectedAnimalsUI)
    {
        if (!animals.ContainsKey(collectedName))
        {
            animals.Add(collectedName, 0);
        }
        animals[collectedName] = animals[collectedName] + 1;
        foreach(Transform child in collectedAnimalsUI.transform)
        {
            if (collectedName.Contains(child.gameObject.name))
            {
                foreach(Transform c in child.transform)
                {
                    if (c.name.Equals("CountText"))
                    {
                        c.GetComponent<TMP_Text>().text = animals[collectedName].ToString();
                    }
                }
            }
        }
    }
}
