using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
  [SerializeField] private DialogueSequence dialogueSequence;
  private bool dialogueStarted = false;

  public void TriggerDialogue()
  {
    DialogueManager.Instance.PlayDialogueSequence(dialogueSequence);
    dialogueStarted = true;
  }

  private void Update()
  {
    // Diyaloğu başlatma tuşu
    if (Input.GetKeyDown(KeyCode.Space) && !dialogueStarted)
    {
      TriggerDialogue();
    }

    // Diyaloğu ilerleme/atlama tuşu (zaten başladıysa)
    else if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && dialogueStarted)
    {
      // DialogueManager.Instance.AdvanceDialogue();
    }
  }
}