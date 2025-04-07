using UnityEngine;

// Controls facial expressions for a character by manipulating texture offsets.
// Relies on CharacterEmotionManager to get emotion data.
public class EmotionController : MonoBehaviour
{
  [SerializeField] private MeshRenderer faceRenderer;

  // Configuration
  private const string textureSTProperty = "_BaseMap_ST";
  private const int materialIndex = 0;

  // State
  private EmotionType currentEmotion = EmotionType.Neutral;
  private Material faceMaterial;
  private int shaderPropertyID;

  private void Awake()
  {
    Initialize();
  }

  private void OnDestroy()
  {
    // Clean up the material instance
    if (faceMaterial != null)
    {
      Destroy(faceMaterial);
      faceMaterial = null;
    }
  }

  // Sets the character's facial expression by changing texture offset.
  public void SetEmotion(EmotionType emotion)
  {
    currentEmotion = emotion;
    Vector2 offset = EmotionManager.Instance.GetEmotionOffset(emotion);

    UpdateTextureOffset(offset);
  }

  // Returns the character's current emotional state.
  public EmotionType GetCurrentEmotion()
  {
    return currentEmotion;
  }

  // Initializes the controller by creating a material instance and caching the shader property.
  private void Initialize()
  {
    faceMaterial = new Material(faceRenderer.materials[materialIndex]);
    var materials = faceRenderer.materials;
    materials[materialIndex] = faceMaterial;
    faceRenderer.materials = materials;

    shaderPropertyID = Shader.PropertyToID(textureSTProperty);

    SetEmotion(EmotionType.Neutral);
  }

  // Updates the texture offset on the material.
  private void UpdateTextureOffset(Vector2 offset)
  {
    Vector4 currentST = faceMaterial.GetVector(shaderPropertyID);
    Vector4 newST = new Vector4(currentST.x, currentST.y, offset.x, offset.y);

    faceMaterial.SetVector(shaderPropertyID, newST);
  }
}