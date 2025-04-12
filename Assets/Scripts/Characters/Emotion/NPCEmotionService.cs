using UnityEngine;

/// <summary>
/// Singleton service that provides access to NPC emotion UV mapping data
/// </summary>
public class NPCEmotionService : MonoBehaviour
{
  public static NPCEmotionService Instance { get; private set; }

  private const string emotionMapResourcePath = "NPCEmotionUVMap";
  private NPCEmotionUVMap emotionMap;

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);

    LoadEmotionMap();
  }

  private void LoadEmotionMap()
  {
    emotionMap = Resources.Load<NPCEmotionUVMap>(emotionMapResourcePath);

    if (emotionMap == null)
    {
      Debug.LogError($"Failed to load NPC emotion map from Resources/{emotionMapResourcePath}");
    }
  }

  /// <summary>
  /// Gets the NPCEmotionUVMap containing all emotion mappings
  /// </summary>
  public NPCEmotionUVMap GetEmotionMap()
  {
    return emotionMap;
  }

  /// <summary>
  /// Gets the UV offset for the specified emotion type
  /// </summary>
  public Vector2 GetEmotionUVOffset(NPCEmotionType emotionType)
  {
    if (emotionMap == null)
    {
      Debug.LogError("Emotion map not loaded. Returning default UV offset.");
      return Vector2.zero;
    }

    return emotionMap.GetUVOffset(emotionType);
  }
}