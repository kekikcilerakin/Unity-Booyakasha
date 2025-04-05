using UnityEngine;

public class CharacterCustomization : MonoBehaviour
{
  [SerializeField] private SkinnedMeshRenderer characterRenderer;
  [SerializeField] private int skinMaterialIndex = 0; // Index of skin material in the renderer

  // Default skin color options - you can expand these
  [SerializeField]
  private Color[] skinColorPresets = new Color[] {
    // Lightest to darkest
    new Color(0.98f, 0.87f, 0.74f, 1f),  // Very light (Caucasian/Asian)
    new Color(0.94f, 0.78f, 0.67f, 1f),  // Light (Caucasian)
    new Color(0.89f, 0.67f, 0.47f, 1f),  // Light olive/tan (Latino/Asian)
    new Color(0.79f, 0.59f, 0.41f, 1f),  // Medium olive (Latino/Hispanic)
    new Color(0.65f, 0.48f, 0.35f, 1f),  // Medium-dark olive/brown
    new Color(0.60f, 0.42f, 0.29f, 1f),  // Light brown (African-American)
    new Color(0.52f, 0.37f, 0.26f, 1f),  // Medium brown (African-American)
    new Color(0.38f, 0.25f, 0.13f, 1f),  // Dark brown (African-American)
  };

  private Material skinMaterial;
  private static readonly int SkinColorProperty = Shader.PropertyToID("_BaseColor"); // For URP/Standard shader

  private void Awake()
  {
    // Create instance of the material to avoid changing the shared material
    skinMaterial = new Material(characterRenderer.materials[skinMaterialIndex]);

    // Apply the material to the renderer
    Material[] materials = characterRenderer.materials;
    materials[skinMaterialIndex] = skinMaterial;
    characterRenderer.materials = materials;
  }

  public void SetSkinColor(Color color)
  {
    if (skinMaterial != null)
    {
      skinMaterial.SetColor(SkinColorProperty, color);
    }
  }

  public void SetSkinColorPreset(int presetIndex)
  {
    if (presetIndex >= 0 && presetIndex < skinColorPresets.Length)
    {
      SetSkinColor(skinColorPresets[presetIndex]);
    }
  }

  // Optional: Method for custom color (e.g., from a color picker)
  public void SetCustomSkinColor(float r, float g, float b)
  {
    SetSkinColor(new Color(r, g, b, 1f));
  }

  // Clean up the instantiated material when destroyed
  private void OnDestroy()
  {
    if (skinMaterial != null)
    {
      Destroy(skinMaterial);
    }
  }

  public Color[] SkinColorPresets => skinColorPresets;
}