using UnityEngine;

public class CharacterCreationManager : MonoBehaviour
{
  public static CharacterCreationManager Instance;

  [Header("Data References")]
  [SerializeField] private CharacterSO characterData;
  [SerializeField] private SkinColorData skinColorData;

  [Header("Character Renderers")]
  [SerializeField] private SkinnedMeshRenderer skinRenderer;

  // Event that UI can subscribe to
  public delegate void OnCharacterUpdatedDelegate();
  public event OnCharacterUpdatedDelegate OnCharacterUpdated;

  // Temporary selections that aren't committed to the character data yet
  private int selectedSkinColorIndex;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  private void Start()
  {
    // Initialize selected color from character data
    selectedSkinColorIndex = characterData.SkinColorIndex;
    UpdateCharacterAppearance();
  }

  public CharacterSO CharacterData => characterData;
  public SkinColorData SkinColorData => skinColorData;
  public int CurrentSkinColorIndex => selectedSkinColorIndex;

  public void SetSkinColor(int colorIndex)
  {
    if (colorIndex < 0 || colorIndex >= skinColorData.ColorCount)
    {
      Debug.LogWarning($"Invalid skin color index: {colorIndex}");
      return;
    }

    // Only store in temporary selection, not in character data yet
    selectedSkinColorIndex = colorIndex;
    UpdateCharacterAppearance();

    // Notify subscribers that character has been updated
    OnCharacterUpdated?.Invoke();
  }

  public void UpdateCharacterAppearance()
  {
    if (characterData == null || skinColorData == null)
    {
      Debug.LogError("Character data or skin color data not assigned!");
      return;
    }

    // Apply skin color from temporary selection
    Color skinColor = skinColorData.GetColor(selectedSkinColorIndex);
    if (skinRenderer != null)
    {
      skinRenderer.material.color = skinColor;
    }
  }

  public void SaveCharacter()
  {
    // Now we save the temporary selections to the CharacterSO
    characterData.SkinColorIndex = selectedSkinColorIndex;

    // Here you would save any additional character creation options
    Debug.Log($"Character saved with skin color index: {characterData.SkinColorIndex}");

    // Additional code to save the character or proceed to game
  }
}