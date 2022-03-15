using System.Collections.Generic;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    public enum DirectionState
    {
        Left,
        Forward,
        Right
    }
    public DirectionState currentDirection = DirectionState.Forward;

    [SerializeField] float distanceBetween = 0.2f;

    [SerializeField] float smoothTime = 0.2f;
    [SerializeField] float speed = 280;
    [SerializeField] float turnSpeed = 180;
    [SerializeField] GameObject animalCarriage;
    [SerializeField] List<GameObject> bodyParts = new List<GameObject>();
    List<GameObject> trainParts = new List<GameObject>();

    private float moveInputHorizontal;
    private float cameraZoomsLeft = 0f;
    private float countUp = 0;
    private Camera mainCamera;
    
    private Vector3 velocity;

    // Start is called before the first frame update
    private void Start()
    {
        mainCamera = Camera.main;
        CreateBodyParts();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (bodyParts.Count > 0)
        {
            CreateBodyParts();
        }
        SnakeMovement();
        if (cameraZoomsLeft > 0.01) {
            mainCamera.orthographicSize += Time.deltaTime / 2;
            cameraZoomsLeft -= Time.deltaTime / 2;
        }
        Debug.Log(currentDirection);
    }

    public void OnUIArrowPointerDown(string direction)
    {
        switch (direction.ToLower())
        {
            case "left":
                currentDirection = DirectionState.Left;
                moveInputHorizontal = -0.5f;
                break;
            case "right":
                currentDirection = DirectionState.Right;
                moveInputHorizontal = 0.5f;
                break;
        }
    }

    public void OnUIArrowPointerUp()
    {
        currentDirection = DirectionState.Forward;
        moveInputHorizontal = 0;
    }

    private void SnakeMovement()
    {
        Vector2 headMove = trainParts[0].transform.up * speed * Time.deltaTime;
        trainParts[0].GetComponent<Rigidbody2D>().velocity = headMove;
        
        Vector3 targetPosition = trainParts[0].GetComponent<Transform>().TransformPoint(new Vector3(0, 0, -10));
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, targetPosition, ref velocity, smoothTime);

        if (moveInputHorizontal != 0)
        {
            trainParts[0].transform.Rotate(new Vector3(0, 0, -turnSpeed * Time.deltaTime * moveInputHorizontal));
            mainCamera.transform.rotation = trainParts[0].transform.rotation;
        }

        if (trainParts.Count > 1)
        {
            for (int i = 1; i < trainParts.Count; i++)
            {
                MarkerManager markM = trainParts[i - 1].GetComponent<MarkerManager>();
                trainParts[i].transform.position = markM.markerList[0].position;
                trainParts[i].transform.rotation = markM.markerList[0].rotation;
                markM.markerList.RemoveAt(0);
            }
        }
    }

    void CreateBodyParts()
    {
        if (trainParts.Count == 0)
        {
            GameObject temp1 = Instantiate(bodyParts[0], transform.position, transform.rotation, transform);
            trainParts.Add(temp1);
            bodyParts.RemoveAt(0);
        }

        MarkerManager markM = trainParts[trainParts.Count -1].GetComponent<MarkerManager>();
        if (countUp == 0)
        {
            markM.ClearMarkerList();
        }
        countUp += Time.deltaTime;
        if (countUp >= distanceBetween)
        {
            GameObject temp = Instantiate(animalCarriage, markM.markerList[0].position, markM.markerList[0].rotation, transform);
            trainParts.Add(temp);
            bodyParts.RemoveAt(0);
            temp.GetComponent<MarkerManager>().ClearMarkerList();
            countUp = 0;
        }
    }

    public void ClearAnimals()
    {
        for (int i = 1; i < trainParts.Count; i++)
        {
            Destroy(trainParts[i]);
        }
        trainParts.RemoveRange(1, trainParts.Count - 1);
    }

    public void Grow()
    {
        bodyParts.Add(trainParts[trainParts.Count - 1]);
        cameraZoomsLeft = 0.2f;
    }
}
