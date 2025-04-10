using UnityEngine;
using UnityEngine.InputSystem;

namespace ECM2.Walkthrough.Ex24
{
  /// <summary>
  /// This example shows how to make use of the new Input System,
  /// in particular, the PlayerInput component to control a First Person Character.
  /// </summary>
  public class FirstPersonInput : MonoBehaviour
  {
    [Header("Character")]
    /// <summary>
    /// Our controlled character.
    /// </summary>
    [Tooltip("Character to be controlled.\n" +
             "If not assigned, this will look into this GameObject.")]
    [SerializeField]
    private Character _character;

    [Header("Camera Settings")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow.")]
    public GameObject cameraTarget;
    [Tooltip("How far in degrees can you move the camera down.")]
    public float minPitch = -80.0f;
    [Tooltip("How far in degrees can you move the camera up.")]
    public float maxPitch = 80.0f;
    [Tooltip("Mouse look sensitivity")]
    public Vector2 sensitivity = new Vector2(1.5f, 1.25f);
    public bool invertLook = false;

    [Header("Crouch Camera Setup")]
    [Tooltip("Cinemachine Virtual Camera positioned at desired crouched height.")]
    public GameObject crouchedCamera;
    [Tooltip("Cinemachine Virtual Camera positioned at desired un-crouched height.")]
    public GameObject unCrouchedCamera;

    /// <summary>
    /// Current movement input values.
    /// </summary>
    private Vector2 _movementInput;

    /// <summary>
    /// Current look input values.
    /// </summary>
    private Vector2 _lookInput;

    private float _cameraTargetPitch;

    /// <summary>
    /// Movement InputAction event handler.
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
      _movementInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Look InputAction event handler.
    /// </summary>
    public void OnLook(InputAction.CallbackContext context)
    {
      _lookInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Jump InputAction event handler.
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
      if (context.started)
        _character.Jump();
      else if (context.canceled)
        _character.StopJumping();
    }

    /// <summary>
    /// Crouch InputAction event handler.
    /// </summary>
    public void OnCrouch(InputAction.CallbackContext context)
    {
      if (context.started)
        _character.Crouch();
      else if (context.canceled)
        _character.UnCrouch();
    }

    private void AddControlYawInput(float value)
    {
      _character.AddYawInput(value);
    }

    private void AddControlPitchInput(float value)
    {
      if (value == 0.0f)
        return;

      _cameraTargetPitch = MathLib.ClampAngle(_cameraTargetPitch + value, minPitch, maxPitch);
      cameraTarget.transform.localRotation = Quaternion.Euler(-_cameraTargetPitch, 0.0f, 0.0f);
    }

    private void OnCrouched()
    {
      crouchedCamera.SetActive(true);
      unCrouchedCamera.SetActive(false);
    }

    private void OnUnCrouched()
    {
      crouchedCamera.SetActive(false);
      unCrouchedCamera.SetActive(true);
    }

    /// <summary>
    /// Handle polled input here (ie: movement, look, etc.)
    /// </summary>
    protected virtual void HandleInput()
    {
      // Camera look
      Vector2 lookInput = _lookInput * sensitivity;
      AddControlYawInput(lookInput.x);
      AddControlPitchInput(invertLook ? -lookInput.y : lookInput.y);

      // Compose a movement direction vector relative to character's forward
      Vector3 movementDirection = Vector3.zero;

      movementDirection += _character.GetForwardVector() * _movementInput.y;
      movementDirection += _character.GetRightVector() * _movementInput.x;

      // Set character's movement direction vector
      _character.SetMovementDirection(movementDirection);
    }

    protected virtual void Awake()
    {
      // If character not assigned, attempts to cache from this current GameObject
      if (_character == null)
        _character = GetComponent<Character>();
    }

    private void OnEnable()
    {
      _character.Crouched += OnCrouched;
      _character.UnCrouched += OnUnCrouched;
    }

    private void OnDisable()
    {
      _character.Crouched -= OnCrouched;
      _character.UnCrouched -= OnUnCrouched;
    }

    protected void Start()
    {
      Cursor.lockState = CursorLockMode.Locked;
    }

    protected virtual void Update()
    {
      HandleInput();
    }
  }
}
