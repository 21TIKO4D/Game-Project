using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelData : MonoBehaviour
{
    public Grid tileMap;

    private GameObject station;
    private Transform locomotivePosition;

    public Transform LocomotivePosition {
        get {
            return locomotivePosition;
        }
        private set {
            locomotivePosition = value;
        }
    }

    public GameObject Station {
        get {
            return station;
        }
        private set {
            station = value;
        }
    }

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
                            Transform obs = Instantiate(obj.transform.GetChild(0), Vector3.zero, Quaternion.identity, trainManager.gameManager.tileMapGrid.transform);
                            obs.transform.position = obj.transform.GetChild(0).transform.position;
                            LocomotivePosition = obj.transform.GetChild(1);
                            Station = obj.transform.GetChild(2).gameObject;
                        } else
                        {
                            GameObject animal = Instantiate(obj.gameObject, obj.position, obj.rotation, animalsObj.transform);
                            animal.gameObject.GetComponent<Animal>().TrainManager = trainManager;
                        }
                    }
                    break;
                }
            }
        }
    }

}
