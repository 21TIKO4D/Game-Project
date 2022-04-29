using System.Collections.Generic;
using UnityEngine;

public class TrainManager : MonoBehaviour
{
    [SerializeField] 
    private float speed = 280;
    [SerializeField] 
    private float turnSpeed = 165;
    public FuelBar fuelBar;
    public GameObject collectedAnimalsUI;
    [SerializeField]
    public GameManager gameManager;
    
    private GameObject locomotive;
    [SerializeField]
    private GameObject trainCarPrefab;

    [SerializeField]
    private GameObject station;

    [SerializeField]
    private Sprite locomotiveTurningLeft;

    [SerializeField]
    private Sprite locomotiveSprite;

    [SerializeField]
    private Sprite locomotiveTurningRight;

    private float cameraZoomsLeft = 0f;
    private float backwardDistance = 0f;
    private float fuelMultiplier = 1.15f;
    private float moveInputHorizontal;
    private int markersBetweenParts = 19;
    private Dictionary<string, int> collectedAnimals = new Dictionary<string, int>();
    private List<Marker> markerList = new List<Marker>();
    private List<GameObject> trainCars = new List<GameObject>();
    private Camera mainCamera;
    private Inventory inventory;
    private Rigidbody2D locomotiveRb;
    private SpriteRenderer locomotiveSpriteRenderer;
    private Vector2 movement;
    private Vector3 camTargetPosition;
    private Vector3 velocity;
    private bool goingBackwards = false;

    private void Start()
    {
        locomotive = this.gameObject;
        locomotiveSpriteRenderer = locomotive.transform.GetChild(0).GetComponent<SpriteRenderer>();
        locomotiveRb = locomotive.GetComponent<Rigidbody2D>();
        markerList.Clear();
        mainCamera = Camera.main;
        markerList.Add(new Marker(locomotive.transform));
        int startFuel = 50;
        if (LevelLoader.Current.currentLevel > 9)
        {
            startFuel = 68;
        } else if (LevelLoader.Current.currentLevel > 8)
        {
            startFuel = 65;
        } else if (LevelLoader.Current.currentLevel > 7)
        {
            startFuel = 56;
        }
        fuelBar.SetMaxFuel(startFuel);

        LevelData levelData = LevelLoader.Current.transform.GetChild(0).GetComponent<LevelData>();
        levelData.LoadData(this);

        locomotive.transform.position = levelData.LocomotivePosition.position;
        locomotive.transform.rotation = levelData.LocomotivePosition.rotation;

        station.transform.position = levelData.Station.transform.position;
        station.transform.rotation = levelData.Station.transform.rotation;
        station.GetComponent<SpriteRenderer>().sprite = levelData.Station.GetComponent<SpriteRenderer>().sprite;

        gameManager.LevelStart(this);
        mainCamera.transform.rotation = locomotive.transform.rotation;
        locomotiveSpriteRenderer.sprite = locomotiveSprite;
        inventory = new Inventory(collectedAnimalsUI, gameManager);
    }

    private void FixedUpdate()
    {
        if (gameManager.IsPaused)
        {
            locomotiveRb.velocity = Vector3.zero;
            return;
        }
        TrainMovement();
        if (cameraZoomsLeft > 0.01f) {
            mainCamera.orthographicSize += Time.deltaTime / 2;
            cameraZoomsLeft -= Time.deltaTime / 2;
        }

        if (backwardDistance <= 0.01f)
        {
            if (markerList.Count > (markersBetweenParts * trainCars.Count + markersBetweenParts))
            {
                markerList.Remove(markerList[0]);
            }
            markerList.Add(new Marker(locomotive.transform));
        }
    }

    public void OnUIArrowPointerClick(float horizontal)
    {
        moveInputHorizontal += horizontal;
        UpdateLocomotiveSprite();
    }

    private void UpdateLocomotiveSprite()
    {
        if (moveInputHorizontal > 0)
        {
            locomotiveSpriteRenderer.sprite = backwardDistance > 0.01f ? locomotiveTurningLeft : locomotiveTurningRight;
        }
        else if (moveInputHorizontal < 0)
        {
            locomotiveSpriteRenderer.sprite = backwardDistance > 0.01f ? locomotiveTurningRight : locomotiveTurningLeft;
        }
        else
        {
            locomotiveSpriteRenderer.sprite = locomotiveSprite;
        }
    }

    public void ClearAnimals()
    {
        if (trainCars.Count > 0)
        {
            for (int i = 0; i < trainCars.Count; i++)
            {
                GameObject trainCar = trainCars[i];
                string animalName = trainCar.name;
                inventory.UpdateAnimalCountsToUI(animalName);
                if (!collectedAnimals.ContainsKey(animalName))
                {
                    collectedAnimals.Add(animalName, 1);
                }
                else
                {
                    collectedAnimals[animalName]++;
                }
                Destroy(trainCar);
            }
            
            gameManager.CheckAnimalCount(collectedAnimals);
            EmptyTrain();
            trainCars.Clear();
            gameManager.transform.GetChild(0).GetComponent<AudioSource>().Play();
        }
    }

    public void Grow(string collectedName)
    {
        GameObject trainCar = Instantiate(trainCarPrefab, markerList[markerList.Count - 1].position, markerList[markerList.Count - 1].rotation, transform);
        trainCar.name = collectedName;
        trainCars.Add(trainCar);
        cameraZoomsLeft = 0.35f;
        fuelMultiplier += 0.2f;
    }

    public void PauseLocomotiveSound()
    {
        locomotive.transform.GetChild(0).GetComponent<AudioSource>().Pause();
    }

    public void ResumeLocomotiveSound()
    {
        locomotive.transform.GetChild(0).GetComponent<AudioSource>().UnPause();
    }

    private void EmptyTrain()
    {
        markerList.RemoveRange(0, trainCars.Count * markersBetweenParts);
        fuelMultiplier = 1f;
    }

    private void TrainMovement()
    {
        if (moveInputHorizontal != 0)
        {
            locomotive.transform.Rotate(new Vector3(0, 0, -turnSpeed * Time.deltaTime * moveInputHorizontal));
            mainCamera.transform.rotation = locomotive.transform.rotation;
        }
        if (backwardDistance >= 0.01f)
        {
            goingBackwards = true;
            movement = -locomotive.transform.up * (speed * 0.65f) * Time.deltaTime;
            backwardDistance -= movement.normalized.magnitude / 2;
        } else
        {
            if (goingBackwards) 
            {
                UpdateLocomotiveSprite();
                goingBackwards = false;
            }
            movement = locomotive.transform.up * speed * Time.deltaTime;
        }
        locomotiveRb.velocity = movement;
        fuelBar.DecreaseFuel(movement.sqrMagnitude * Time.deltaTime / 40 * fuelMultiplier);
        if (fuelBar.Value <= 0)
        {
            gameManager.GameEnd(false);
        }
        
        camTargetPosition = locomotive.transform.TransformPoint(new Vector3(0, 2f, -10));
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, camTargetPosition, ref velocity, Time.deltaTime);

        for (int i = 0; i < trainCars.Count; i++)
        {
            int nextMarker = markersBetweenParts * (i + 1) - 1;
            trainCars[i].transform.position = markerList[nextMarker].position;
            trainCars[i].transform.rotation = markerList[nextMarker].rotation;
        }
    }

    public void OnTrainCollision()
    {
        backwardDistance = 15f;
        SummonAnimalAndDestroyCar();
        UpdateLocomotiveSprite();
    }

    private void SummonAnimalAndDestroyCar()
	{
        EmptyTrain();
		while (trainCars.Count > 0)
        {
            GameObject trainCar = trainCars[0];
            Transform obj = FindNewAnimalObj(trainCar);

            if (obj != null)
            {
                Vector2 spawnPoint;
                do
                {
                    float x = Random.Range(-29.0f, 25.5f);
                    float y = Random.Range(-16.0f, 17.0f);
                    spawnPoint = new Vector2(x, y);
                } while (Physics2D.OverlapCircle(spawnPoint, 2.5f, LayerMask.GetMask("Obstacle")));

                trainCars.RemoveAt(0);

                GameObject animal = Instantiate(obj.gameObject, spawnPoint, Quaternion.identity, gameManager.animals.transform);
                animal.name = obj.name + "(Clone)";
                animal.gameObject.GetComponent<Animal>().TrainManager = this;
                Destroy(trainCar);
            }
        }
	}

    private Transform FindNewAnimalObj(GameObject trainCar)
    {
        foreach (Transform levelObj in LevelLoader.Current.transform)
        {
            if (levelObj.name.Equals("Level_" + LevelLoader.Current.currentLevel))
            {
                foreach (Transform obj in levelObj.transform)
                {
                    if (trainCar.name.StartsWith(obj.name))
                    {
                        return obj;
                    }
                }
            }
        }
        return null;
    }

    public class Marker
    {
        public Vector3 position;
        public Quaternion rotation;

        public Marker(Transform transform)
        {
            position = transform.position;
            rotation = transform.rotation;
        }
    }
}
