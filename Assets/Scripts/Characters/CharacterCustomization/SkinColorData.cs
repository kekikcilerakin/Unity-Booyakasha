using UnityEngine;

[CreateAssetMenu(fileName = "SkinColorData", menuName = "Character Creation/Skin Color Data")]
public class SkinColorData : ScriptableObject
{
  [SerializeField]
  private Color[] skinColors = new Color[]
  {
    new Color(0.98f, 0.87f, 0.74f, 1f),  // Very light (Caucasian/Asian)
    new Color(0.94f, 0.78f, 0.67f, 1f),  // Light (Caucasian)
    new Color(0.89f, 0.67f, 0.47f, 1f),  // Light olive/tan (Latino/Asian)
    new Color(0.79f, 0.59f, 0.41f, 1f),  // Medium olive (Latino/Hispanic)
    new Color(0.65f, 0.48f, 0.35f, 1f),  // Medium-dark olive/brown
    new Color(0.60f, 0.42f, 0.29f, 1f),  // Light brown (African-American)
    new Color(0.52f, 0.37f, 0.26f, 1f),  // Medium brown (African-American)
    new Color(0.38f, 0.25f, 0.13f, 1f),  // Dark brown (African-American)
  };

  public Color[] SkinColors => skinColors;
  public int ColorCount => skinColors.Length;

  public Color GetColor(int index)
  {
    if (index < 0 || index >= skinColors.Length)
    {
      Debug.LogWarning($"Invalid skin color index: {index}. Returning default color.");
      return skinColors[0];
    }
    return skinColors[index];
  }
}