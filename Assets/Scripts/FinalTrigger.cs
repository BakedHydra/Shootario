using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;

public class FinalTrigger : Trigger
{
    [Header("Final Trigger Settings")]
    [SerializeField] private PlayableDirector cutscene;
    public static event Action enableMove;
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().CanMove = false;
            GameObject.FindWithTag("GameController").GetComponent<GameManager>().InterfaceUI();
            cutscene.stopped += OnCutsceneStopped;
            cutscene.Play();
        }
    }
    private void OnCutsceneStopped(PlayableDirector obj)
    {
        cutscene.stopped -= OnCutsceneStopped;
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().CanMove = true;
        GameObject.FindWithTag("GameController").GetComponent<GameManager>().InterfaceUI();
        enableMove?.Invoke();
    }
}
