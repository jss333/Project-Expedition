using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    // Tag of the object you want to detect collision with
    public string targetTag;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object's tag matches the target tag
        if (collision.gameObject.tag == targetTag)
        {
            // Load the next scene in the build settings
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
