using UnityEngine;

public class Animal : MonoBehaviour
{
    public TrainManager TrainManager
    {
        get;
        set;
    }

    private enum State
    {
        IDLE,
        TURN
    }
    private State state;
    private float timer = 2.5f;

    private float multiplier = 1;

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (state == State.IDLE)
            {
                state = State.TURN;
                timer = 0.9f;
                multiplier = Random.Range(0, 4) < 2 ? -1 : 1;
            } else
            {
                state = State.IDLE;
                timer = 6.5f + Random.Range(-1f, 1.5f);
            }
        }
        switch (state)
        {
            case State.TURN:
                Vector3 rotation = transform.eulerAngles;
                float value = timer > 0.2 ? 0.15f : 0.1f;
                rotation.z += multiplier * value;
                transform.rotation = Quaternion.Euler(rotation);
                break;
        }
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
            transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            Destroy(this.gameObject, delay);
        }
    }
}
