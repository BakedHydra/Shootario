using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Lever : MonoBehaviour
{

    public PlayableDirector Cutscene;
    private bool isActivated = false;

    private void OnMouseDown()
    {
        if (!isActivated)
        {
            Cutscene.Play();
            Cutscene.stopped += OnCutsceneFinished;
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().CanMove = false;
            GameObject.FindWithTag("GameController").GetComponent<GameManager>().InterfaceUI();
            isActivated = true;
        }
    }
    private void OnCutsceneFinished(PlayableDirector director)
    {
        Cutscene.stopped -= OnCutsceneFinished;
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().CanMove = true;
        GameObject.FindWithTag("GameController").GetComponent<GameManager>().InterfaceUI();
    }
}
