using UnityEngine;

public class Station : MonoBehaviour
{
    [SerializeField]
    TrainManager trainManager;

    [SerializeField]
	private float speed = 1;

    bool glow = false;
    private Color newColor;

    private void Start()
    {
        newColor = transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        newColor.a = 0;
    }

    private void FixedUpdate()
    {
        float alpha;
        if (glow)
        {
            alpha = Mathf.Clamp01(newColor.a + Time.deltaTime * speed);
            if (newColor.a >= 1)
            {
                glow = false;
            }
        } else
        {
            alpha = Mathf.Clamp01(newColor.a - Time.deltaTime * speed);
            if (newColor.a <= 0)
            {
                glow = true;
            }
        }
        newColor.a = alpha;
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = newColor;
        transform.GetChild(1).GetComponent<SpriteRenderer>().color = newColor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            trainManager.ClearAnimals();
        }
    }
}
