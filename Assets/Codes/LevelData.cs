using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelData : MonoBehaviour
{
    public Grid tileMap;
    public Transform locomotivePosition;

    public Transform stationPosition;

    public void LoadData(TrainManager trainManager)
    {
        GameObject animalsObj = null;
        GameObject[] rootObjects = SceneManager.GetSceneByName("LevelTemplate").GetRootGameObjects();
        foreach (GameObject item in rootObjects)
        {
            if (item.name == "Animals")
            {
                animalsObj = item;
                break;
            }
        }
        if (animalsObj != null)
        {
            foreach (Transform levelObj in LevelLoader.Current.transform)
            {
                if (levelObj.name.Equals("Level_" + LevelLoader.Current.currentLevel))
                {
                    foreach (Transform obj in levelObj.transform)
                    {
                        if (obj.name == "Grid")
                        {
                            Instantiate(obj.transform.GetChild(0), Vector3.zero, Quaternion.identity, trainManager.gameManager.tileMapGrid.transform);
                        } else
                        {
                            GameObject animal = Instantiate(obj.gameObject, obj.position, obj.rotation, animalsObj.transform);
                            animal.name = animal.name;
                            animal.gameObject.GetComponent<Animal>().TrainManager = trainManager;
                        }
                    }
                    break;
                }
            }
        }
    }

}
