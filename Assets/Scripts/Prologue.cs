using UnityEngine;
using UnityEngine.InputSystem;

public class Prologue : MonoBehaviour
{
  [SerializeField] private PlayerInput _playerInput;
  [SerializeField] private DialogueSequence dialogueSequence;

  [SerializeField] private GameObject playerCamera;
  [SerializeField] private GameObject fatherCamera;
  [SerializeField] private GameObject mariaCamera;
  [SerializeField] private GameObject carlosCamera;

  private void Start()
  {
    DialogueManager.Instance.PlayDialogueSequence(dialogueSequence);
  }

  public void DisablePlayerInput()
  {
    _playerInput.enabled = false;
  }

  public void EnablePlayerInput()
  {
    _playerInput.enabled = true;
  }

  public void SwitchToFatherCamera()
  {
    playerCamera.SetActive(false);
    fatherCamera.SetActive(true);
    mariaCamera.SetActive(false);
    carlosCamera.SetActive(false);
  }

  public void SwitchToMariaCamera()
  {
    playerCamera.SetActive(false);
    fatherCamera.SetActive(false);
    mariaCamera.SetActive(true);
    carlosCamera.SetActive(false);
  }

  public void SwitchToCarlosCamera()
  {
    playerCamera.SetActive(false);
    fatherCamera.SetActive(false);
    mariaCamera.SetActive(false);
    carlosCamera.SetActive(true);
  }

  public void SwitchToPlayerCamera()
  {
    fatherCamera.SetActive(false);
    mariaCamera.SetActive(false);
    carlosCamera.SetActive(false);
    playerCamera.SetActive(true);
  }
}
