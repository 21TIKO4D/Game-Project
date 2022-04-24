using UnityEngine;

public class TrainCollision : MonoBehaviour
{
    private static float lastCollisionTime = 0;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obstacle") && lastCollisionTime > 0.4f)
        {
            lastCollisionTime = 0;
            this.GetComponentInParent<TrainManager>().OnTrainCollision();
            
            ParticleSystem effect = null;
            AudioSource audio = null;
            foreach (Transform child in other.gameObject.transform.parent)
            {
                if (child.name == "CollisionEffect")
                {
                    effect = child.GetComponent<ParticleSystem>();
                    audio = child.GetComponent<AudioSource>();
                    break;
                }
            }
            if (effect != null)
            {
                effect.gameObject.transform.position = this.gameObject.transform.position;
                audio.Play();
                effect.Play();
            }
        }
    }

    private void FixedUpdate()
    {
        lastCollisionTime += Time.fixedDeltaTime;
    }
}
