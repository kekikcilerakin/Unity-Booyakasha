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
    foreach (var line in lines)
    {
      if (line.minDisplayTime < 1f)
      {
        line.minDisplayTime = 1f;
      }

      float timeBasedOnLength = line.text.Length * 0.05f;
      line.calculatedDisplayTime = Mathf.Clamp(timeBasedOnLength, line.minDisplayTime, 2.5f);
    }
  }
}