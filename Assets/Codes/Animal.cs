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

    public TrainManager TrainManager
    {
        get;
        set;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            TrainManager.Grow(this.gameObject.name);
            Destroy(this.gameObject);
        }
    }
}
