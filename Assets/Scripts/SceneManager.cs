using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject deathUI;
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject interfaceUI;


    private int enemyCounter;
    public void OnEnable()
    {
        EnemySpawner.OnSpawn += OnSpawn;
        Enemy.OnDeath += On_Death;
    }

    private void On_Death()
    {
        enemyCounter--;
        if (enemyCounter <= 0)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().canRegenerate = true;
        }
    }

    private void OnSpawn()
    {
        enemyCounter++;
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().canRegenerate = false;
    }

    public void Reload()
    {
        if (menuUI.activeSelf)
        {
            MenuUI();
        }
        if (deathUI.activeSelf)
        {
            DeathUI();
        }
        if (winUI.activeSelf)
        {
            WinUI();
        }
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    public void ToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuUI();
        }
    }
    public void MenuUI()
    {
        if (menuUI.activeSelf)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().CanMove = true;
            interfaceUI.SetActive(true);
            menuUI.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().CanMove = false;
            interfaceUI.SetActive(false);
            menuUI.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void DeathUI()
    {
        if (deathUI.activeSelf)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().CanMove = true;
            interfaceUI.SetActive(true);
            deathUI.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().CanMove = false;
            interfaceUI.SetActive(false);
            deathUI.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void WinUI()
    {
        if (deathUI.activeSelf)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().CanMove = true;
            interfaceUI.SetActive(true);
            winUI.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().CanMove = false;
            interfaceUI.SetActive(false);
            winUI.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
