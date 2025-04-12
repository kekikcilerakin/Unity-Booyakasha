using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

[Serializable]
public class SequenceEvent
{
  public DialogueSequence sequence;
  public UnityEvent onStarted = new UnityEvent();
  public UnityEvent onEnded = new UnityEvent();
}

public class DialogueSequenceListener : MonoBehaviour
{
  [SerializeField] private List<SequenceEvent> sequenceEvents = new List<SequenceEvent>();
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
    DialogueManager.Instance.OnSequenceStarted += HandleSequenceStarted;
    DialogueManager.Instance.OnSequenceEnded += HandleSequenceEnded;
  }

  private void UnsubscribeFromEvents()
  {
    if (DialogueManager.Instance != null)
    {
      DialogueManager.Instance.OnSequenceStarted -= HandleSequenceStarted;
      DialogueManager.Instance.OnSequenceEnded -= HandleSequenceEnded;
    }
    isInitialized = false;
  }

  private void HandleSequenceStarted(DialogueSequence sequence)
  {
    foreach (var sequenceEvent in sequenceEvents)
    {
      if (sequenceEvent.sequence == sequence)
      {
        sequenceEvent.onStarted?.Invoke();
      }
    }
  }

  private void HandleSequenceEnded(DialogueSequence sequence)
  {
    foreach (var sequenceEvent in sequenceEvents)
    {
      if (sequenceEvent.sequence == sequence)
      {
        sequenceEvent.onEnded?.Invoke();
      }
    }
  }
}