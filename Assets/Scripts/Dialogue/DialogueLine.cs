using UnityEngine;

[System.Serializable]
public class DialogueLine
{
  [Tooltip("Konuşan karakter")]
  public Character character;

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

  // Localization için daha sonra eklenecek
  // public string textID;

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