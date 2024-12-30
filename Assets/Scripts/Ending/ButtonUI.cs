using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    public void TryAgain()
    {
        SceneManager.LoadScene("Game");
    }
}
