using UnityEngine;

public class Animal : MonoBehaviour
{
    public TrainManager TrainManager
    {
        get;
        set;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            TrainManager.Grow(this.gameObject.name);

            float delay = 0;
            AudioSource audio = GetComponent<AudioSource>();
            if (audio != null) {
                audio.Play();
                delay = audio.clip.length;
            }
            this.GetComponent<CapsuleCollider2D>().enabled = false;
            this.GetComponent<Renderer>().enabled = false;
            Destroy(this.gameObject, delay);
        }
    }
}
