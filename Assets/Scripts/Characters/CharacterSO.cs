using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Dialogue/Character")]
public class CharacterSO : ScriptableObject
{
  [Header("Basic Info")]
  public string characterName;
  public Color nameColor = Color.white;

  [Header("Appearance")]
  [SerializeField, Range(0, 7)] private int skinColorIndex;
  public int SkinColorIndex 
  { 
    get => skinColorIndex; 
    set => skinColorIndex = value;
  }

  // Diğer potansiyel özellikler:
  // public AnimationClip talkAnimation;
  // public string characterID; // Localization için
  // public AudioClip[] voiceSamples;
  // public Faction faction;
}