using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Dialogue/Character")]
public class Character : ScriptableObject
{
  [Header("Basic Info")]
  public string characterName;
  public Color nameColor = Color.white;

  [Tooltip("Character audio settings")]
  [Range(0.8f, 1.2f)]
  public float pitchModifier = 1.0f;

  // Diğer potansiyel özellikler:
  // public AnimationClip talkAnimation;
  // public string characterID; // Localization için
  // public AudioClip[] voiceSamples;
  // public Faction faction;
}