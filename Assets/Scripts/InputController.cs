using ECM2;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
  [Header("Look Settings")]
  public Vector2 sensitivity = new Vector2(1f, 1f);
  public bool invertLook = false;

  [Header("Input Actions")]
  private PlayerInput _playerInput;
  private const string ACTION_MAP_PLAYER = "Player";
  private const string ACTION_MAP_UI = "CUSTOM_UI";

  private bool isInInventory = false;

  private Vector2 _movementInput;
  private Vector2 _lookInput;
  private Character _character;

  private SprintAbility _sprintAbility;

  public bool IsMoving => _movementInput.sqrMagnitude > 0.01f;

  protected virtual void Awake()
  {
    _character = GetComponent<Character>();
    _playerInput = GetComponent<PlayerInput>();

    _sprintAbility = GetComponent<SprintAbility>();
  }

  private void OnEnable()
  {
    SwitchActionMapAndCursor(ACTION_MAP_PLAYER, false);
  }

  private void OnDisable()
  {
    SwitchActionMapAndCursor(ACTION_MAP_PLAYER, false);
  }

  public void OnMove(InputAction.CallbackContext context)
  {
    _movementInput = context.ReadValue<Vector2>();
  }

  public void OnLook(InputAction.CallbackContext context)
  {
    _lookInput = context.ReadValue<Vector2>();
  }

  public void OnJump(InputAction.CallbackContext context)
  {
    if (context.started) _character.Jump();
    else if (context.canceled) _character.StopJumping();
  }

  public void OnCrouch(InputAction.CallbackContext context)
  {
    if (context.started) _character.Crouch();
    else if (context.canceled) _character.UnCrouch();
  }

  public void OnSprint(InputAction.CallbackContext context)
  {
    if (context.started) _sprintAbility.Sprint();
    else if (context.canceled) _sprintAbility.StopSprinting();
  }

  public void OnToggleInventory(InputAction.CallbackContext context)
  {
    if (!context.performed) return;

    isInInventory = !isInInventory;
    SwitchActionMapAndCursor(isInInventory ? ACTION_MAP_UI : ACTION_MAP_PLAYER, isInInventory);
  }

  public void SwitchActionMapAndCursor(string actionMap, bool showCursor)
  {
    _playerInput.SwitchCurrentActionMap(actionMap);
    Cursor.lockState = showCursor ? CursorLockMode.None : CursorLockMode.Locked;
    Cursor.visible = showCursor;
  }

  protected virtual void Update()
  {
    HandleInput();
  }

  protected virtual void HandleInput()
  {
    // Camera look
    Vector2 lookInput = _lookInput * sensitivity;
    _character.AddYawInput(lookInput.x);
    _character.AddControlPitchInput(invertLook ? -lookInput.y : lookInput.y);

    // Movement
    Vector3 movementDirection = Vector3.zero;
    movementDirection += _character.GetForwardVector() * _movementInput.y;
    movementDirection += _character.GetRightVector() * _movementInput.x;
    _character.SetMovementDirection(movementDirection);
  }

}