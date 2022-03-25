using UnityEngine;


public class Animal : MonoBehaviour
{
    public enum AnimalType
    {
        Cow,
        Sheep
    }

    public AnimalType Type
    {
        get;
    }

    [SerializeField]
    private GameObject trainCarPrefab;

    [SerializeField]
    public TrainManager trainManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            trainManager.Grow(this.gameObject.name, trainCarPrefab);
            Destroy(this.gameObject);
        }
    }
}
