using UnityEngine;
using UnityEngine.Playables;

public class GlassRoomDirector : MonoBehaviour
{
    [SerializeField] private GameObject pressurePad1;
    [SerializeField] private GameObject pressurePad2;
    [SerializeField] private PlayableDirector Cutscene;
    private bool isActivated = false;
    void Update()
    {
        if(pressurePad1.GetComponent<PressurePad>().isPressed && pressurePad2.GetComponent<PressurePad>().isPressed)
        {
            CutsceneActivate();
        }
    }
    private void CutsceneActivate()
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
