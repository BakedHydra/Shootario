using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController: MonoBehaviour
{
    public void ToGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ToExit()
    {
        Application.Quit();
    }
}
