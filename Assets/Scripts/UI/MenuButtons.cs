using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public Main main;

    public void LoadScene(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void ContinueButton()
    {
        gameObject.SetActive(false);
        Time.timeScale = main.timeScale;
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
