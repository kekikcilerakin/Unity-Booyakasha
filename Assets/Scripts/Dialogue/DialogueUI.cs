using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueUI : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI speakerNameText;
  [SerializeField] private TextMeshProUGUI dialogueText;

  [Header("Yazı Animasyonu")]
  [SerializeField] private float typingSpeed = 0.05f; // Harf başına saniye
  [SerializeField] private float punctuationPause = 0.2f; // Noktalama işaretlerinde ek bekleme
  [SerializeField] private AudioClip typingSound; // İsteğe bağlı yazma sesi
  [SerializeField] private float typingSoundVolume = 0.5f;

  private CanvasGroup canvasGroup;
  private Coroutine typingCoroutine;
  private bool isTyping = false;

  // Diyalog tamamlandığında DialogueManager'a bildirmek için
  public System.Action OnTypingCompleted;

  private void Awake()
  {
    canvasGroup = GetComponent<CanvasGroup>();
    Hide();
  }

  public void ShowDialogueLine(DialogueLine line)
  {
    speakerNameText.text = line.GetSpeakerName();
    speakerNameText.color = line.GetSpeakerColor();

    // Eğer önceki yazma animasyonu varsa durdur
    if (typingCoroutine != null)
    {
      StopCoroutine(typingCoroutine);
    }

    // Başlangıçta diyalog metnini temizle
    dialogueText.text = "";

    // Paneli görünür yap
    canvasGroup.alpha = 1f;
    canvasGroup.interactable = true;
    canvasGroup.blocksRaycasts = true;

    // Yazma animasyonunu başlat
    typingCoroutine = StartCoroutine(TypeText(line.text));
  }

  // Metni harf harf gösteren coroutine
  private IEnumerator TypeText(string text)
  {
    isTyping = true;
    dialogueText.text = "";

    // Yazma efekti için ses kaynağı (isteğe bağlı)
    AudioSource audioSource = null;
    if (typingSound != null)
    {
      audioSource = gameObject.AddComponent<AudioSource>();
      audioSource.clip = typingSound;
      audioSource.volume = typingSoundVolume;
      audioSource.loop = true;
      audioSource.Play();
    }

    // Metni harf harf görüntüle
    for (int i = 0; i < text.Length; i++)
    {
      dialogueText.text += text[i];

      // Noktalama işaretlerinde daha uzun bekle
      if (i < text.Length - 1 && IsPunctuation(text[i]))
      {
        yield return new WaitForSeconds(punctuationPause);
      }
      else
      {
        yield return new WaitForSeconds(typingSpeed);
      }
    }

    // Ses çalıyorsa durdur
    if (audioSource != null)
    {
      audioSource.Stop();
      Destroy(audioSource);
    }

    isTyping = false;

    // Yazma tamamlandığında event'i tetikle
    OnTypingCompleted?.Invoke();
  }

  // Noktalama işareti kontrolü
  private bool IsPunctuation(char character)
  {
    return character == '.' || character == ',' || character == '!' || character == '?' || character == ':' || character == ';';
  }

  // Yazı animasyonunu tamamla (oyuncu atlamak istediğinde)
  public void CompleteTyping()
  {
    if (isTyping)
    {
      StopCoroutine(typingCoroutine);
      dialogueText.text = dialogueText.text.Substring(0, dialogueText.text.Length); // Mevcut metni koru
      isTyping = false;

      // Yazma tamamlandığında event'i tetikle
      OnTypingCompleted?.Invoke();
    }
  }

  public void Hide()
  {
    if (typingCoroutine != null)
    {
      StopCoroutine(typingCoroutine);
      typingCoroutine = null;
    }

    canvasGroup.alpha = 0f;
    canvasGroup.interactable = false;
    canvasGroup.blocksRaycasts = false;
  }

  // Yazı gösterimi devam ediyor mu?
  public bool IsTyping()
  {
    return isTyping;
  }
}