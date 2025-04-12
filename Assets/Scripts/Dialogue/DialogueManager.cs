using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

public class DialogueManager : MonoBehaviour
{
  public static DialogueManager Instance { get; private set; }

  [SerializeField] private DialogueUI dialogueUI;
  [SerializeField] private DialogueChoiceUI choiceUI;

  [Header("Sequence Events")]
  [SerializeField] private UnityEvent onAnySequenceStarted = new UnityEvent();
  [SerializeField] private UnityEvent onAnySequenceEnded = new UnityEvent();

  // Events that can be subscribed to for specific sequences
  public event Action<DialogueSequence> OnSequenceStarted;
  public event Action<DialogueSequence> OnSequenceEnded;

  // Events that can be subscribed to for specific lines
  public event Action<DialogueSequence, int> OnLineDisplayed;
  public event Action<DialogueSequence, int> OnLineTypingComplete;
  public event Action<DialogueSequence, int> OnLineFinished;

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

    onAnySequenceStarted?.Invoke();
    OnSequenceStarted?.Invoke(sequence);

    playingCoroutine = StartCoroutine(PlaySequenceCoroutine());
  }

  public void StopDialogueSequence()
  {
    if (playingCoroutine != null)
    {
      StopCoroutine(playingCoroutine);
      playingCoroutine = null;
    }

    StopDialogueAudio();

    if (currentSequence != null)
    {
      var sequence = currentSequence;
      onAnySequenceEnded?.Invoke();
      OnSequenceEnded?.Invoke(sequence);
      currentSequence = null;
    }

    isPlaying = false;
    dialogueUI.Hide();
  }

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

      // Trigger line displayed events
      OnLineDisplayed?.Invoke(currentSequence, currentLineIndex);

      dialogueUI.ShowDialogueLine(line);

      StopDialogueAudio();

      if (line.audioClip != null)
      {
        dialogueAudioSource.clip = line.audioClip;
        dialogueAudioSource.Play();
      }

      bool typingComplete = false;
      dialogueUI.OnTypingCompleted = () => 
      {
        typingComplete = true;
        // Trigger typing complete events
        OnLineTypingComplete?.Invoke(currentSequence, currentLineIndex);
      };

      yield return new WaitUntil(() => typingComplete);

      yield return new WaitForSeconds(line.calculatedDisplayTime);

      // Trigger line finished events
      OnLineFinished?.Invoke(currentSequence, currentLineIndex);

      currentLineIndex++;
    }

    if (currentSequence.isInteractive && currentSequence.choices != null && currentSequence.choices.Count > 0)
    {
      choiceUI.ShowChoices(currentSequence.choices);
    }
    else
    {
      var sequence = currentSequence;
      onAnySequenceEnded?.Invoke();
      OnSequenceEnded?.Invoke(sequence);
      isPlaying = false;
      dialogueUI.Hide();
    }
  }
}