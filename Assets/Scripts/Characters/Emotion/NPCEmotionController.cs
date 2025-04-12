using UnityEngine;

/// <summary>
/// Controls facial expressions for NPCs by applying UV offsets to the face material
/// </summary>
public class NPCEmotionController : MonoBehaviour
{
  [SerializeField] private SkinnedMeshRenderer faceRenderer;
  [SerializeField] private int faceTextureIndex = 0;
  [Tooltip("Property name for the texture UV scale/offset")]
  [SerializeField] private string textureSTProperty = "_BaseMap_ST";

  private NPCEmotionType currentEmotion = NPCEmotionType.Neutral;
  private Material faceMaterial;
  private int uvPropertyID;

  private void Awake()
  {
    Initialize();
  }

  private void Initialize()
  {
    if (faceRenderer == null)
    {
      Debug.LogError($"No face renderer assigned to {gameObject.name}'s NPCEmotionController");
      return;
    }

    if (faceRenderer.materials.Length <= faceTextureIndex)
    {
      Debug.LogError($"Material index {faceTextureIndex} is out of range for {gameObject.name}'s face renderer");
      return;
    }

    // Create a material instance to avoid modifying shared materials
    faceMaterial = new Material(faceRenderer.materials[faceTextureIndex]);
    var materials = faceRenderer.materials;
    materials[faceTextureIndex] = faceMaterial;
    faceRenderer.materials = materials;

    uvPropertyID = Shader.PropertyToID(textureSTProperty);
  }

  private void OnDestroy()
  {
    if (faceMaterial == null) return;

    Destroy(faceMaterial);
    faceMaterial = null;
  }

  /// <summary>
  /// Sets the NPC's facial expression by applying the appropriate UV offset
  /// </summary>
  public void SetEmotion(NPCEmotionType emotion)
  {
    if (faceMaterial == null) return;

    currentEmotion = emotion;
    Vector2 offset = NPCEmotionService.Instance.GetEmotionUVOffset(emotion);

    ApplyUVOffset(offset);
  }

  private void ApplyUVOffset(Vector2 offset)
  {
    Vector4 currentST = faceMaterial.GetVector(uvPropertyID);
    // Keep scale (x,y) and update offset (z,w)
    Vector4 newST = new Vector4(currentST.x, currentST.y, offset.x, offset.y);

    faceMaterial.SetVector(uvPropertyID, newST);
  }

  // Convenience methods for setting common emotions
  public void SetEmotionNeutral() => SetEmotion(NPCEmotionType.Neutral);
  public void SetEmotionHappy() => SetEmotion(NPCEmotionType.Happy);
  public void SetEmotionSad() => SetEmotion(NPCEmotionType.Sad);
  public void SetEmotionAngry() => SetEmotion(NPCEmotionType.Angry);
  public void SetEmotionSurprised() => SetEmotion(NPCEmotionType.Surprised);
  public void SetEmotionScared() => SetEmotion(NPCEmotionType.Scared);
  public void SetEmotionDisgusted() => SetEmotion(NPCEmotionType.Disgusted);

  /// <summary>
  /// Gets the current emotion of the NPC
  /// </summary>
  public NPCEmotionType GetCurrentEmotion() => currentEmotion;
}