using UnityEngine;


namespace PeliExample
{
    public class Animal : MonoBehaviour
    {
        public SnakeManager snakeManager;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player")) {
                snakeManager.Grow();
                Destroy(this.gameObject);
            }
        }
    }
}
