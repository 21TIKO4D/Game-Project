using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            this.GetComponentInParent<TrainManager>().OnTrainCollision();
            
            ParticleSystem effect = other.gameObject.GetComponentInChildren<ParticleSystem>();
            if (effect != null)
            {
                effect.gameObject.transform.position = this.gameObject.transform.position;
                effect.Play();
            }
        }
    }
}
