using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
  [SerializeField] private DialogueSequence dialogueSequence;
  // private bool dialogueStarted = false;

  private void Start()
  {
    TriggerDialogue();
  }

  public void TriggerDialogue()
  {
    DialogueManager.Instance.PlayDialogueSequence(dialogueSequence);
    // dialogueStarted = true;
  }

  private void Update()
  {
    // Diyaloğu ilerleme/atlama tuşu (zaten başladıysa)
    // if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && dialogueStarted)
    // {
    //   DialogueManager.Instance.AdvanceDialogue();
    // }
  }
}