using System.Collections.Generic;
using UnityEngine;

public class TrainManager : MonoBehaviour
{
    [SerializeField] 
    private float speed = 280;
    [SerializeField] 
    private float turnSpeed = 170;
    [SerializeField]
    private FuelBar fuelBar;
    [SerializeField]
    private GameObject collectedAnimalsUI;
    [SerializeField]
    private GameObject locomotive;

    private float cameraZoomsLeft = 0f;
    private float backwardDistance = 0f;
    private float fuelMultiplier = 1f;
    private float moveInputHorizontal;
    private int markersBetweenParts = 19;
    private List<Marker> markerList = new List<Marker>();
    private List<Marker> shadowMarkerList = new List<Marker>();
    private List<GameObject> trainCars = new List<GameObject>();
    private Camera mainCamera;
    private Inventory inventory;
    private Vector2 movement;
    private Vector3 camTargetPosition;
    private Vector3 velocity;

    private void Start()
    {
        markerList.Clear();
        mainCamera = Camera.main;
        inventory = new Inventory();
        markerList.Add(new Marker(locomotive.transform.position, locomotive.transform.rotation));
        fuelBar.SetMaxFuel(50);
    }

    private void FixedUpdate()
    {
        TrainMovement();
        if (cameraZoomsLeft > 0.01f) {
            mainCamera.orthographicSize += Time.deltaTime / 2;
            cameraZoomsLeft -= Time.deltaTime / 2;
        }

        if (backwardDistance <= 0.01f)
        {
            if (markerList.Count > (markersBetweenParts * trainCars.Count + markersBetweenParts))
            {
                shadowMarkerList.Add(markerList[0]);
                markerList.Remove(markerList[0]);
                if (shadowMarkerList.Count > markersBetweenParts * 4)
                {
                    shadowMarkerList.Remove(shadowMarkerList[0]);
                }
            }
            markerList.Add(new Marker(locomotive.transform.position, locomotive.transform.rotation));
        }
    }

    public void OnUIArrowPointerDown(string direction)
    {
        switch (direction.ToLower())
        {
            case "left":
                moveInputHorizontal = -0.5f;
                break;
            case "right":
                moveInputHorizontal = 0.5f;
                break;
        }
    }

    public void OnUIArrowPointerUp()
    {
        moveInputHorizontal = 0;
    }

    public void ClearAnimals()
    {
        for (int i = 0; i < trainCars.Count; i++)
        {
            Destroy(trainCars[i]);
        }
        
        markerList.RemoveRange(0, trainCars.Count * markersBetweenParts);
        trainCars.Clear();
        fuelMultiplier = 1f;
    }

    public void Grow(string collectedName, GameObject trainCarPrefab)
    {
        GameObject trainCar = Instantiate(trainCarPrefab, markerList[markerList.Count - 1].position, markerList[markerList.Count - 1].rotation, transform);
        trainCars.Add(trainCar);
        cameraZoomsLeft = 0.35f;
        inventory.UpdateAnimalCountsToUI(collectedName, collectedAnimalsUI);
        fuelMultiplier += 0.25f;
    }

    private void TrainMovement()
    {
        if (moveInputHorizontal != 0)
        {
            locomotive.transform.Rotate(new Vector3(0, 0, -turnSpeed * Time.deltaTime * moveInputHorizontal));
            mainCamera.transform.rotation = locomotive.transform.rotation;
        }
        bool isColliding = backwardDistance >= 0.01f;
        if (isColliding)
        {
            movement = -locomotive.transform.up * (speed * 0.65f) * Time.deltaTime;
            backwardDistance -= movement.normalized.magnitude / 2;
        } else
        {
            movement = locomotive.transform.up * speed * Time.deltaTime;
        }
        locomotive.GetComponent<Rigidbody2D>().velocity = movement;
        fuelBar.DecreaseFuel(movement.sqrMagnitude * Time.deltaTime / 40 * fuelMultiplier);
        
        camTargetPosition = locomotive.GetComponent<Transform>().TransformPoint(new Vector3(0, 2.25f, -10));
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, camTargetPosition, ref velocity, 0.2f);

        for (int i = 0; i < trainCars.Count; i++)
        {
            trainCars[i].transform.position = markerList[markersBetweenParts * (i + 1) - (isColliding ? 2 : 1)].position;
            trainCars[i].transform.rotation = markerList[markersBetweenParts * (i + 1) - (isColliding ? 2 : 1)].rotation;
        }
    }

    public void OnTrainCollision()
    {
        markerList.CopyTo(shadowMarkerList.ToArray(), 0);
        backwardDistance = 15f;
    }

    public class Marker
    {
        public Vector3 position;
        public Quaternion rotation;

        public Marker(Vector3 pos, Quaternion rot)
        {
            position = pos;
            rotation = rot;
        }
    }
}
