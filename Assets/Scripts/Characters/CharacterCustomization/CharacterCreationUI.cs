using UnityEngine;
using UnityEngine.UI;

public class CharacterCreationUI : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private CharacterCreationManager characterCreationManager;

  [Header("UI Elements")]
  [SerializeField] private Button[] skinColorButtons;
  [SerializeField] private Button saveButton;
  [SerializeField] private float selectedButtonScale = 1.25f;

  private void Start()
  {
    if (characterCreationManager == null)
    {
      Debug.LogError("Character Creation Manager not assigned!");
      return;
    }

    // Subscribe to character update events
    characterCreationManager.OnCharacterUpdated += UpdateSelectedButton;

    // Setup UI components
    InitializeSkinColorButtons();

    saveButton.onClick.AddListener(OnSaveButtonClicked);
  }

  private void OnDestroy()
  {
    // Unsubscribe from events
    if (characterCreationManager != null)
    {
      characterCreationManager.OnCharacterUpdated -= UpdateSelectedButton;
    }
  }

  private void InitializeSkinColorButtons()
  {
    if (skinColorButtons == null || skinColorButtons.Length == 0)
    {
      Debug.LogError("Skin color buttons not assigned!");
      return;
    }

    var skinColorData = characterCreationManager.SkinColorData;

    // Make sure we have the right number of buttons
    if (skinColorButtons.Length != skinColorData.ColorCount)
    {
      Debug.LogWarning($"Number of buttons ({skinColorButtons.Length}) does not match number of skin colors ({skinColorData.ColorCount})");
    }

    // Set button colors and add listeners
    for (int i = 0; i < skinColorButtons.Length && i < skinColorData.ColorCount; i++)
    {
      int colorIndex = i; // Local variable for closure
      Button button = skinColorButtons[i];

      if (button != null)
      {
        // Set button color
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
          buttonImage.color = skinColorData.GetColor(colorIndex);
        }

        // Clear existing listeners and add new one
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnSkinColorButtonClicked(colorIndex));
      }
    }

    // Set initial selection
    UpdateSelectedButton();
  }

  private void OnSkinColorButtonClicked(int colorIndex)
  {
    characterCreationManager.SetSkinColor(colorIndex);
  }

  private void UpdateSelectedButton()
  {
    int currentColorIndex = characterCreationManager.CurrentSkinColorIndex;

    for (int i = 0; i < skinColorButtons.Length; i++)
    {
      if (skinColorButtons[i] != null)
      {
        // Visual indication of selection - scale the selected button
        skinColorButtons[i].transform.localScale = (i == currentColorIndex)
          ? new Vector3(selectedButtonScale, selectedButtonScale, 1f)
          : Vector3.one;
      }
    }
  }

  private void OnSaveButtonClicked()
  {
    characterCreationManager.SaveCharacter();
  }
}