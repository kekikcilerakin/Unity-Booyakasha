using UnityEngine;
using System;

[System.Serializable]
public class DialogueChoice
{
  public string choiceText;
  public DialogueSequence nextSequence;
  public Action onChosen; // Seçildiğinde çalıştırılacak event
}