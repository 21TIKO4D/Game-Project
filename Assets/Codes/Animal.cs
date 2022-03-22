using UnityEngine;


public class Animal : MonoBehaviour
{
    public enum AnimalType
    {
        Cow,
        Sheep
    }
    
    [SerializeField]
    private AnimalType animalType;

    public AnimalType Type
    {
        get
        {
            return animalType;
        }
    }

    [SerializeField]
    public TrainManager trainManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            trainManager.Grow(this.gameObject.name);
            Destroy(this.gameObject);
        }
    }
}
