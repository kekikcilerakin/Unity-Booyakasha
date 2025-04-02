using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class DialogueChoiceUI : MonoBehaviour
{
  [SerializeField] private Transform choiceContainer;
  [SerializeField] private GameObject choiceButtonPrefab;

  private List<GameObject> activeChoiceButtons = new List<GameObject>();

  private void Awake()
  {
    if (choiceContainer == null)
    {
      Debug.LogError("Choice Container atanmamış! Lütfen inspector'dan atayın.");
    }

    if (choiceButtonPrefab == null)
    {
      Debug.LogError("Choice Button Prefab atanmamış! Lütfen inspector'dan atayın.");
    }

    if (choiceContainer != null)
    {
      choiceContainer.gameObject.SetActive(false);
    }
  }

  public void ShowChoices(List<DialogueChoice> choices)
  {
    if (choiceContainer == null || choiceButtonPrefab == null)
    {
      Debug.LogError("DialogueChoiceUI: Gerekli referanslar atanmamış!");
      return;
    }

    if (choices == null || choices.Count == 0)
    {
      Debug.LogWarning("DialogueChoiceUI: Gösterilecek seçenek yok!");
      return;
    }

    ClearChoices();

    foreach (var choice in choices)
    {
      if (choice == null) continue;

      GameObject buttonObj = Instantiate(choiceButtonPrefab, choiceContainer);

      Button button = buttonObj.GetComponent<Button>();
      if (button == null)
      {
        Debug.LogError("Choice Button Prefab'ında Button komponenti yok!");
        Destroy(buttonObj);
        continue;
      }

      TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
      if (buttonText == null)
      {
        Debug.LogError("Choice Button Prefab'ında TextMeshProUGUI komponenti yok!");
        Destroy(buttonObj);
        continue;
      }

      buttonText.text = choice.choiceText;

      button.onClick.AddListener(() =>
      {
        OnChoiceSelected(choice);
      });

      activeChoiceButtons.Add(buttonObj);
    }

    choiceContainer.gameObject.SetActive(true);
  }

  private void OnChoiceSelected(DialogueChoice choice)
  {
    // Seçim yapıldıktan sonra UI'yi gizle
    choiceContainer.gameObject.SetActive(false);

    // Eğer varsa, choice.onChosen event'ini çağır
    choice.onChosen?.Invoke();

    // Yeni diyalog akışını başlat
    if (choice.nextSequence != null)
    {
      DialogueManager.Instance.PlayDialogueSequence(choice.nextSequence);
    }

    ClearChoices();
  }

  private void ClearChoices()
  {
    foreach (var button in activeChoiceButtons)
    {
      Destroy(button);
    }

    activeChoiceButtons.Clear();
  }
}