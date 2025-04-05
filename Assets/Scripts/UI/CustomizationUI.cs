using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CustomizationUI : MonoBehaviour
{
  [SerializeField] private CharacterCustomization characterCustomization;
  [SerializeField] private Button[] skinColorButtons;

  // Optional: Add images to show colors on buttons
  [SerializeField] private bool setButtonColors = true;

  private void Start()
  {
    // Set up preset buttons
    for (int i = 0; i < skinColorButtons.Length; i++)
    {
      int index = i; // Need to capture the index for the lambda
      skinColorButtons[i].onClick.AddListener(() => characterCustomization.SetSkinColorPreset(index));

      // Optional: Set button color to match the skin tone it represents
      if (setButtonColors && characterCustomization != null && i < characterCustomization.SkinColorPresets.Length)
      {
        Image buttonImage = skinColorButtons[i].GetComponent<Image>();
        if (buttonImage != null)
        {
          buttonImage.color = characterCustomization.SkinColorPresets[i];
        }
      }
    }
  }
}