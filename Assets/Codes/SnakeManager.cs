using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SnakeManager : MonoBehaviour
{

    [SerializeField] float distanceBetween = 0.2f;
    [SerializeField] float speed = 280;
    [SerializeField] float turnSpeed = 180;
    [SerializeField] List<GameObject> bodyParts = new List<GameObject>();
    List<GameObject> snakeBody = new List<GameObject>();

    private float moveInputHorizontal;

    float countUp = 0;

    // Start is called before the first frame update
    private void Start()
    {
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
    }

    private void OnMove(InputAction.CallbackContext callbackContext)
    {
        this.moveInputHorizontal = callbackContext.ReadValue<Vector2>().x;
    }

    private void SnakeMovement()
    {
        snakeBody[0].GetComponent<Rigidbody2D>().velocity = snakeBody[0].transform.right * speed * Time.deltaTime;
        if (moveInputHorizontal != 0)
        {
            snakeBody[0].transform.Rotate(new Vector3(0, 0, -turnSpeed * Time.deltaTime * moveInputHorizontal));
        }

        if (snakeBody.Count > 1)
        {
            for (int i = 1; i < snakeBody.Count; i++)
            {
                MarkerManager markM = snakeBody[i - 1].GetComponent<MarkerManager>();
                snakeBody[i].transform.position = markM.markerList[0].position;
                snakeBody[i].transform.rotation = markM.markerList[0].rotation;
                markM.markerList.RemoveAt(0);
            }
        }
    }

    void CreateBodyParts()
    {
        if (snakeBody.Count == 0)
        {
            GameObject temp1 = Instantiate(bodyParts[0], transform.position, transform.rotation, transform);
            snakeBody.Add(temp1);
            bodyParts.RemoveAt(0);
        }

        MarkerManager markM = snakeBody[snakeBody.Count -1].GetComponent<MarkerManager>();
        if (countUp == 0)
        {
            markM.ClearMarkerList();
        }
        countUp += Time.deltaTime;
        if (countUp >= distanceBetween)
        {
            GameObject temp = Instantiate(bodyParts[0], markM.markerList[0].position, markM.markerList[0].rotation, transform);
            snakeBody.Add(temp);
            bodyParts.RemoveAt(0);
            temp.GetComponent<MarkerManager>().ClearMarkerList();
            countUp = 0;
        }
    }

    public void Grow()
    {
        bodyParts.Add(snakeBody[snakeBody.Count - 1]);
    }
}
