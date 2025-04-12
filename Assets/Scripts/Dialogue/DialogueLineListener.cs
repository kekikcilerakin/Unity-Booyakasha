using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

[Serializable]
public class LineEvent
{
  [Tooltip("The dialogue sequence containing the line")]
  public DialogueSequence sequence;

  [Tooltip("The ID of the line to trigger events for")]
  public string lineId;

  [Header("Events")]
  public UnityEvent onLineDisplayed = new UnityEvent();
  public UnityEvent onLineTypingComplete = new UnityEvent();
  public UnityEvent onLineFinished = new UnityEvent();
}

public class DialogueLineListener : MonoBehaviour
{
  [SerializeField] private List<LineEvent> lineEvents = new List<LineEvent>();
  private bool isInitialized = false;

  private void Start()
  {
    InitializeIfNeeded();
  }

  private void OnEnable()
  {
    InitializeIfNeeded();
  }

  private void OnDisable()
  {
    UnsubscribeFromEvents();
  }

  private void InitializeIfNeeded()
  {
    if (isInitialized) return;

    if (DialogueManager.Instance != null)
    {
      SubscribeToEvents();
      isInitialized = true;
    }
    else
    {
      // If DialogueManager is not ready yet, try again in the next frame
      StartCoroutine(WaitForDialogueManager());
    }
  }

  private System.Collections.IEnumerator WaitForDialogueManager()
  {
    while (DialogueManager.Instance == null)
    {
      yield return null;
    }
    SubscribeToEvents();
    isInitialized = true;
  }

  private void SubscribeToEvents()
  {
    DialogueManager.Instance.OnLineDisplayed += HandleLineDisplayed;
    DialogueManager.Instance.OnLineTypingComplete += HandleLineTypingComplete;
    DialogueManager.Instance.OnLineFinished += HandleLineFinished;
  }

  private void UnsubscribeFromEvents()
  {
    if (DialogueManager.Instance != null)
    {
      DialogueManager.Instance.OnLineDisplayed -= HandleLineDisplayed;
      DialogueManager.Instance.OnLineTypingComplete -= HandleLineTypingComplete;
      DialogueManager.Instance.OnLineFinished -= HandleLineFinished;
    }
    isInitialized = false;
  }

  private void HandleLineDisplayed(DialogueSequence sequence, int lineIndex)
  {
    if (sequence.lines[lineIndex].lineId == null) return;

    foreach (var lineEvent in lineEvents)
    {
      if (lineEvent.sequence == sequence && lineEvent.lineId == sequence.lines[lineIndex].lineId)
      {
        lineEvent.onLineDisplayed?.Invoke();
      }
    }
  }

  private void HandleLineTypingComplete(DialogueSequence sequence, int lineIndex)
  {
    if (sequence.lines[lineIndex].lineId == null) return;

    foreach (var lineEvent in lineEvents)
    {
      if (lineEvent.sequence == sequence && lineEvent.lineId == sequence.lines[lineIndex].lineId)
      {
        lineEvent.onLineTypingComplete?.Invoke();
      }
    }
  }

  private void HandleLineFinished(DialogueSequence sequence, int lineIndex)
  {
    if (sequence.lines[lineIndex].lineId == null) return;

    foreach (var lineEvent in lineEvents)
    {
      if (lineEvent.sequence == sequence && lineEvent.lineId == sequence.lines[lineIndex].lineId)
      {
        lineEvent.onLineFinished?.Invoke();
      }
    }
  }
}