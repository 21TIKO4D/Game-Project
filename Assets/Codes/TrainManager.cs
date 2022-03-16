using System.Collections.Generic;
using UnityEngine;

public class TrainManager : MonoBehaviour
{
    [SerializeField] 
    private float speed = 280;
    [SerializeField] 
    private float turnSpeed = 170;

    [SerializeField]
    private GameObject locomotivePrefab;
    [SerializeField]
    private GameObject trainCarPrefab;

    private float cameraZoomsLeft = 0f;
    private float moveInputHorizontal;
    private Camera mainCamera;
    private GameObject locomotive;
    private Vector3 velocity;

    private void Start()
    {
        mainCamera = Camera.main;
        locomotive = Instantiate(locomotivePrefab, transform.position, transform.rotation, transform);
    }

    private void FixedUpdate()
    {
        TrainMovement();
        Debug.Log("update: " + cameraZoomsLeft + " - " + this.GetInstanceID().ToString());
        if (cameraZoomsLeft > 0.01f) {
            mainCamera.orthographicSize += Time.deltaTime / 2;
            cameraZoomsLeft -= Time.deltaTime / 2;
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
    }

    public void Grow()
    {
        cameraZoomsLeft = 0.3f;
        Debug.Log("Grow " + cameraZoomsLeft + " - " + this.GetInstanceID().ToString());
    }

    private void TrainMovement()
    {
        Vector2 movement = locomotive.transform.up * speed * Time.deltaTime;
        locomotive.GetComponent<Rigidbody2D>().velocity = movement;
        
        Vector3 targetPosition = locomotive.GetComponent<Transform>().TransformPoint(new Vector3(0, 0, -10));
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, targetPosition, ref velocity, 0.2f);

        if (moveInputHorizontal != 0)
        {
            locomotive.transform.Rotate(new Vector3(0, 0, -turnSpeed * Time.deltaTime * moveInputHorizontal));
            mainCamera.transform.rotation = locomotive.transform.rotation;
        }
    }
}
