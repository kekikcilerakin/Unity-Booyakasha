using UnityEngine;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
  public static DialogueManager Instance { get; private set; }

  [SerializeField] private DialogueUI dialogueUI;
  [SerializeField] private DialogueChoiceUI choiceUI;

  // Ses kontrolü için AudioSource ekleyelim
  private AudioSource dialogueAudioSource;

  private DialogueSequence currentSequence;
  private int currentLineIndex = 0;
  private Coroutine playingCoroutine;
  private bool isPlaying = false;

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

    // AudioSource komponentini oluşturalım veya alalım
    dialogueAudioSource = GetComponent<AudioSource>();
    if (dialogueAudioSource == null)
    {
      dialogueAudioSource = gameObject.AddComponent<AudioSource>();
    }
  }

  public void PlayDialogueSequence(DialogueSequence sequence)
  {
    if (sequence == null)
    {
      Debug.LogError("Null diyalog dizisi oynatılamaz!");
      return;
    }

    if (isPlaying)
    {
      StopDialogueSequence();
    }

    currentSequence = sequence;
    currentLineIndex = 0;
    isPlaying = true;

    playingCoroutine = StartCoroutine(PlaySequenceCoroutine());
  }

  public void StopDialogueSequence()
  {
    if (playingCoroutine != null)
    {
      StopCoroutine(playingCoroutine);
      playingCoroutine = null;
    }

    // Ses çalıyorsa durdur
    StopDialogueAudio();

    isPlaying = false;
    dialogueUI.Hide();
  }

  // Diyalog sesini durdur
  private void StopDialogueAudio()
  {
    if (dialogueAudioSource != null && dialogueAudioSource.isPlaying)
    {
      dialogueAudioSource.Stop();
    }
  }

  private IEnumerator PlaySequenceCoroutine()
  {
    while (currentLineIndex < currentSequence.lines.Count)
    {
      DialogueLine line = currentSequence.lines[currentLineIndex];

      // UI'yi göster ve yazı animasyonunu başlat
      dialogueUI.ShowDialogueLine(line);

      // Önceki sesi durdur ve yeni sesi çal
      StopDialogueAudio();

      // Ses efektini oynat
      if (line.audioClip != null)
      {
        dialogueAudioSource.clip = line.audioClip;

        dialogueAudioSource.Play();
      }

      // Yazı animasyonu bitene kadar bekleyelim
      bool typingComplete = false;
      dialogueUI.OnTypingCompleted = () => typingComplete = true;

      // Yazı tamamlanana kadar bekle
      yield return new WaitUntil(() => typingComplete);

      // Yazı tamamlandıktan sonra, konfigüre edilmiş süre kadar bekle
      yield return new WaitForSeconds(line.calculatedDisplayTime);

      currentLineIndex++;
    }

    // Diyalog bittiğinde, eğer interaktif bir diyalogsa seçenekleri göster
    if (currentSequence.isInteractive && currentSequence.choices != null && currentSequence.choices.Count > 0)
    {
      choiceUI.ShowChoices(currentSequence.choices);
    }
    else
    {
      isPlaying = false;
      dialogueUI.Hide();
    }
  }

  // Oyuncu Input'u ile ilerleme
  public void AdvanceDialogue()
  {
    if (!isPlaying) return;

    // Eğer yazı hala yazılıyorsa, yazıyı tamamla
    if (dialogueUI.IsTyping())
    {
      dialogueUI.CompleteTyping();
      return;
    }

    // Eğer bir satır gösteriliyorsa, bir sonraki satıra geç
    if (currentLineIndex < currentSequence.lines.Count)
    {
      StopCoroutine(playingCoroutine);

      // Ses çalıyorsa durdur
      StopDialogueAudio();

      currentLineIndex++;

      if (currentLineIndex < currentSequence.lines.Count)
      {
        playingCoroutine = StartCoroutine(PlaySequenceCoroutine());
      }
      else
      {
        // Son satırın sonunda, interaktif diyalog kontrolü yapın
        if (currentSequence.isInteractive && currentSequence.choices != null && currentSequence.choices.Count > 0)
        {
          choiceUI.ShowChoices(currentSequence.choices);
        }
        else
        {
          isPlaying = false;
          dialogueUI.Hide();
        }
      }
    }
  }
}