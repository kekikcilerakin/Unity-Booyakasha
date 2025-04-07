using UnityEngine;

public class EmotionManager : MonoBehaviour
{
  public static EmotionManager Instance { get; private set; }

  private EmotionData emotionData;

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);

    LoadEmotionData();
  }

  private void LoadEmotionData()
  {
    emotionData = Resources.Load<EmotionData>("EmotionData");
  }

  public EmotionData GetEmotionData()
  {
    return emotionData;
  }

  public Vector2 GetEmotionOffset(EmotionType emotionType)
  {
    return emotionData.GetUVOffset(emotionType);
  }
}