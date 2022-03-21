using UnityEngine;

public class Animal : MonoBehaviour
{
    public TrainManager trainManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            trainManager.Grow();
            Destroy(this.gameObject);
        }
    }
}
