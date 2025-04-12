using UnityEngine;
using UnityEngine.InputSystem;

public class Prologue : MonoBehaviour
{
  [SerializeField] private PlayerInput _playerInput;
  [SerializeField] private DialogueSequence dialogueSequence;

  private void Start()
  {
    _playerInput.enabled = false;
    DialogueManager.Instance.PlayDialogueSequence(dialogueSequence);
  }
}
