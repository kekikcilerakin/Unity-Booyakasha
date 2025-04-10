using ECM2;
using UnityEngine;

public class SprintAbility : MonoBehaviour
{
  [Header("Sprint Settings")]
  [SerializeField] private bool canEverSprint = true;
  [SerializeField] private float maxSprintSpeed = 10.0f;

  private Character _character;
  private bool _isSprinting;
  private bool _sprintInputPressed;
  private float _cachedMaxWalkSpeed;

  public bool CanEverSprint => canEverSprint;
  public bool IsSprinting => _isSprinting;

  public delegate void SprintStartedEventHandler();
  public delegate void SprintEndedEventHandler();

  public event SprintStartedEventHandler SprintStarted;
  public event SprintEndedEventHandler SprintEnded;

  private void Awake() => _character = GetComponent<Character>();

  private void OnEnable() => _character.BeforeSimulationUpdated += OnBeforeSimulationUpdated;

  private void OnDisable() => _character.BeforeSimulationUpdated -= OnBeforeSimulationUpdated;

  private void OnBeforeSimulationUpdated(float deltaTime) => CheckSprintInput();

  public void Sprint() => _sprintInputPressed = true;

  public void StopSprinting() => _sprintInputPressed = false;

  private bool IsSprintAllowed() => canEverSprint && _character.IsWalking() && !_character.IsCrouched();

  private void CheckSprintInput()
  {
    if (!_isSprinting && _sprintInputPressed && IsSprintAllowed())
    {
      StartSprinting();
    }
    else if (_isSprinting && (!_sprintInputPressed || !IsSprintAllowed()))
    {
      StopSprintingInternal();
    }
  }

  private void StartSprinting()
  {
    _isSprinting = true;
    _cachedMaxWalkSpeed = _character.maxWalkSpeed;
    _character.maxWalkSpeed = maxSprintSpeed;
    SprintStarted?.Invoke();
  }

  private void StopSprintingInternal()
  {
    _isSprinting = false;
    _character.maxWalkSpeed = _cachedMaxWalkSpeed;
    SprintEnded?.Invoke();
  }

}