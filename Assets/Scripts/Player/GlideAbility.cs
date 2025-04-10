using ECM2;
using UnityEngine;

public class GlideAbility : MonoBehaviour
{
  [Header("Glide Settings")]
  [SerializeField] private bool _hasGliderEquipped;
  [SerializeField] private float maxFallSpeedGliding = 1.0f;

  private Character _character;
  private bool _isGliding;
  private bool _glideInputPressed;

  public bool IsGliding => _isGliding;
  public bool HasGliderEquipped { get => _hasGliderEquipped; set => _hasGliderEquipped = value; }

  private void Awake() => _character = GetComponent<Character>();

  private void OnEnable() => _character.BeforeSimulationUpdated += OnBeforeSimulationUpdated;

  private void OnDisable() => _character.BeforeSimulationUpdated -= OnBeforeSimulationUpdated;

  private void OnBeforeSimulationUpdated(float deltaTime) => CheckGlideInput();

  public void Glide() => _glideInputPressed = true;

  public void StopGliding() => _glideInputPressed = false;

  private bool IsGlideAllowed()
  {
    if (!_hasGliderEquipped || !_character.IsFalling())
      return false;

    Vector3 worldUp = -_character.GetGravityDirection();
    float verticalSpeed = Vector3.Dot(_character.GetVelocity(), worldUp);

    return verticalSpeed < 0.0f;
  }

  private void CheckGlideInput()
  {
    if (!_isGliding && _glideInputPressed && IsGlideAllowed())
    {
      StartGliding();
    }
    else if (_isGliding && (!_glideInputPressed || !IsGlideAllowed()))
    {
      StopGlidingInternal();
    }
  }

  private void StartGliding()
  {
    _isGliding = true;
    _character.maxFallSpeed = maxFallSpeedGliding;
  }

  private void StopGlidingInternal()
  {
    _isGliding = false;
    _character.maxFallSpeed = 40.0f;
  }

}