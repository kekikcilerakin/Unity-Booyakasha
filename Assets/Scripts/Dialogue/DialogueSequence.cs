using UnityEngine;
using System.Collections.Generic;
using VInspector;

[CreateAssetMenu(fileName = "New Dialogue Sequence", menuName = "Dialogue/Sequence")]
public class DialogueSequence : ScriptableObject
{
  public List<DialogueLine> lines = new List<DialogueLine>();
  public bool isInteractive = false; // Oyuncunun seçim yapabileceği bir diyalog mu?

  [ShowIf("isInteractive")] public List<DialogueChoice> choices = new List<DialogueChoice>(); // Seçenekleri doğrudan DialogueSequence'a ekleyelim

  public void OnValidate()
  {
    // Her satır için gösterim süresini hesapla
    foreach (var line in lines)
    {
      // Minimum gösterim süresinin en az 1 saniye olmasını sağla
      if (line.minDisplayTime < 1f)
      {
        line.minDisplayTime = 1f;
      }
      
      // Her karaktere 0.05 saniye ayır (okuma hızı)
      float timeBasedOnLength = line.text.Length * 0.05f;
      // En az 1 saniye, en çok 2.5 saniye
      line.calculatedDisplayTime = Mathf.Clamp(timeBasedOnLength, line.minDisplayTime, 2.5f);
    }
  }
}