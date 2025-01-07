using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;

    public Animator animator;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator LoadLevel(int level)
    {
        animator.SetBool("start", true);

        yield return new WaitForSeconds(1);

        animator.SetBool("start", false);

        SceneManager.LoadScene(level);
    }
}