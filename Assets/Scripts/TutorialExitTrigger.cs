using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialExitTrigger : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("Boss01");
        }
    }
}
