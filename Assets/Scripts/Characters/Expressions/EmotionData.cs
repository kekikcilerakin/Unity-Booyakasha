using UnityEngine;
using System.Collections.Generic;

// ScriptableObject that stores mapping between emotions and UV offsets
// Used by EmotionManager to provide offset data to all characters
[CreateAssetMenu(fileName = "EmotionData", menuName = "Characters/Emotion Data")]
public class EmotionData : ScriptableObject
{
  // Serializable class for inspector editing of emotion-to-UV mappings
  [System.Serializable]
  public class EmotionUVOffset
  {
    public EmotionType emotionType;
    public Vector2 uvOffset;
  }

  // Array of emotion mappings, editable in the inspector
  [SerializeField] private EmotionUVOffset[] emotionOffsets;

  // Called when values change
  private void OnValidate()
  {
    ValidateEmotionData();
  }

  // Validates the emotion data to check for issues
  private void ValidateEmotionData()
  {
    if (emotionOffsets == null || emotionOffsets.Length == 0)
    {
      Debug.LogWarning($"No emotion offsets defined in {name}");
      return;
    }

    // Check for duplicates
    HashSet<EmotionType> uniqueEmotions = new HashSet<EmotionType>();
    foreach (var emotion in emotionOffsets)
    {
      if (!uniqueEmotions.Add(emotion.emotionType))
      {
        Debug.LogWarning($"Duplicate emotion type {emotion.emotionType} in {name}. This might cause issues.");
      }
    }

    // Check if we have all enum values covered
    foreach (EmotionType emotionType in System.Enum.GetValues(typeof(EmotionType)))
    {
      bool found = false;
      foreach (var emotion in emotionOffsets)
      {
        if (emotion.emotionType == emotionType)
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

  // Gets the UV offset for the specified emotion type
  public Vector2 GetUVOffset(EmotionType emotionType)
  {
    if (emotionOffsets == null)
    {
      Debug.LogWarning($"No emotion offsets defined in {name}");
      return Vector2.zero;
    }

    // Directly search the array for the matching emotion type
    foreach (var emotion in emotionOffsets)
    {
      if (emotion.emotionType == emotionType)
      {
        return emotion.uvOffset;
      }
    }

    // Return zero offset (neutral) if emotion not found
    Debug.LogWarning($"Emotion {emotionType} not found in {name}. Using default (0,0).");
    return Vector2.zero;
  }

  // Returns all available emotion types in this data
  public EmotionType[] GetAvailableEmotions()
  {
    if (emotionOffsets == null || emotionOffsets.Length == 0)
    {
      return new EmotionType[0];
    }

    List<EmotionType> emotions = new List<EmotionType>();
    foreach (var emotion in emotionOffsets)
    {
      emotions.Add(emotion.emotionType);
    }

    return emotions.ToArray();
  }

  // Returns the number of defined emotions
  public int EmotionCount => emotionOffsets != null ? emotionOffsets.Length : 0;
}