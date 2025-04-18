using UnityEngine;
using System;

[Serializable]
public class DialogueLine
{
  [Tooltip("Optional unique identifier for this line (used for triggering events)")]
  public string lineId;

  [Tooltip("Konuşan karakter")]
  public CharacterSO character;

  [Tooltip("Karakterin bu satırdaki özel adı (boş bırakılırsa karakter nesnesindeki isim kullanılır)")]
  public string overrideSpeakerName;

  [Tooltip("Karakterin bu satırdaki özel rengi (etkinse karakter nesnesindeki renk yerine bu kullanılır)")]
  public bool useCustomColor = false;
  public Color32 customColor = Color.white;

  [TextArea(3, 10)]
  public string text;
  public AudioClip audioClip;
  public float minDisplayTime = 1f; // Minimum 1 saniye
  public float calculatedDisplayTime; // Hesaplanacak süre

  // Konuşmacı adını al (override veya karakter adı)
  public string GetSpeakerName()
  {
    if (!string.IsNullOrEmpty(overrideSpeakerName))
      return overrideSpeakerName;

    return character != null ? character.characterName : "Unknown";
  }

  // Konuşmacı rengini al (override veya karakter rengi)
  public Color32 GetSpeakerColor()
  {
    if (useCustomColor)
      return customColor;

    return character != null ? character.nameColor : Color.white;
  }

  public void OnValidate()
  {
    // Minimum sürenin 1 saniyeden az olmamasını sağla
    if (minDisplayTime < 1f)
    {
      minDisplayTime = 1f;
    }
  }
}