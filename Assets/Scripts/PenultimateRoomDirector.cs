using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

public class PenultimateRoomDirector : MonoBehaviour
{
    public PlayableDirector director;
    private int EnemyCount;
    private bool isActivated = false;

    private void OnEnable()
    {
        EnemySpawner.OnSpawn += AddEnemyToCounter;
        Enemy.OnDeath += SubstractEnemyFromCounter;
    }
    private void OnDisable()
    {
        EnemySpawner.OnSpawn -= AddEnemyToCounter;
        Enemy.OnDeath -= SubstractEnemyFromCounter;
    }
    private void AddEnemyToCounter()
    {
        EnemyCount++;
    }
    private void SubstractEnemyFromCounter()
    {
        EnemyCount--;
        if(EnemyCount <= 0)
        {
            PlayCutscene();
        }
    }
    private void PlayCutscene()
    {
        if (!isActivated)
        {
            director.Play();
            director.stopped += OnCutsceneFinished;
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().CanMove = true;
            GameObject.FindWithTag("GameController").GetComponent<GameManager>().InterfaceUI();
            isActivated = true;

        }
    }

    private void OnCutsceneFinished(PlayableDirector obj)
    {
        director.stopped -= OnCutsceneFinished;
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().CanMove = true;
        GameObject.FindWithTag("GameController").GetComponent<GameManager>().InterfaceUI();
        gameObject.SetActive(false);
    }
}
