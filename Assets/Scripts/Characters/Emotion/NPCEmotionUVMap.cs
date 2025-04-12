using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ScriptableObject that maps NPC emotions to UV offsets for facial expressions
/// </summary>
[CreateAssetMenu(fileName = "NPCEmotionUVMap", menuName = "Characters/NPC Emotion UV Map")]
public class NPCEmotionUVMap : ScriptableObject
{
  [System.Serializable]
  public class EmotionUVPair
  {
    public NPCEmotionType emotionType;
    public Vector2 uvOffset;
  }

  [SerializeField] private EmotionUVPair[] emotionMappings;

  private void OnValidate()
  {
    ValidateEmotionData();
  }

  private void ValidateEmotionData()
  {
    if (emotionMappings == null || emotionMappings.Length == 0)
    {
      Debug.LogWarning($"No emotion mappings defined in {name}");
      return;
    }

    // Check for duplicates
    HashSet<NPCEmotionType> uniqueEmotions = new HashSet<NPCEmotionType>();
    foreach (var mapping in emotionMappings)
    {
      if (!uniqueEmotions.Add(mapping.emotionType))
      {
        Debug.LogWarning($"Duplicate emotion type {mapping.emotionType} in {name}. This might cause issues.");
      }
    }

    // Check if we have all enum values covered
    foreach (NPCEmotionType emotionType in System.Enum.GetValues(typeof(NPCEmotionType)))
    {
      bool found = false;
      foreach (var mapping in emotionMappings)
      {
        if (mapping.emotionType == emotionType)
        {
          found = true;
          break;
        }
      }

      if (!found)
      {
        Debug.LogWarning($"Missing UV offset for emotion {emotionType} in {name}");
      }
    }
  }

  /// <summary>
  /// Gets the UV offset for the specified emotion type
  /// </summary>
  public Vector2 GetUVOffset(NPCEmotionType emotionType)
  {
    if (emotionMappings == null)
    {
      Debug.LogWarning($"No emotion mappings defined in {name}");
      return Vector2.zero;
    }

    foreach (var mapping in emotionMappings)
    {
      if (mapping.emotionType == emotionType)
      {
        return mapping.uvOffset;
      }
    }

    Debug.LogWarning($"Emotion {emotionType} not found in {name}. Using default (0,0).");
    return Vector2.zero;
  }

  /// <summary>
  /// Returns all available emotion types in this mapping
  /// </summary>
  public NPCEmotionType[] GetAvailableEmotions()
  {
    if (emotionMappings == null || emotionMappings.Length == 0)
    {
      return new NPCEmotionType[0];
    }

    List<NPCEmotionType> emotions = new List<NPCEmotionType>();
    foreach (var mapping in emotionMappings)
    {
      emotions.Add(mapping.emotionType);
    }

    return emotions.ToArray();
  }

  /// <summary>
  /// Returns the number of defined emotion mappings
  /// </summary>
  public int EmotionCount => emotionMappings != null ? emotionMappings.Length : 0;
}