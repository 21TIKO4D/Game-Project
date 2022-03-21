using UnityEngine;

public class Station : MonoBehaviour
{
    [SerializeField]
    TrainManager trainManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            trainManager.ClearAnimals();
        }
    }
}
