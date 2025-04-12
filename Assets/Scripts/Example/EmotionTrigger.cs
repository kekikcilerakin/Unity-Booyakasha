using UnityEngine;

public class EmotionTrigger : MonoBehaviour
{
    [SerializeField] private NPCEmotionController characterEmotionController1;
    [SerializeField] private NPCEmotionController characterEmotionController2;

    public void TriggerEmotion1(EmotionType emotion)
    {
        characterEmotionController1.SetEmotion(emotion);
    }

    public void TriggerEmotion2(EmotionType emotion)
    {
        characterEmotionController2.SetEmotion(emotion);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            TriggerEmotion1(EmotionType.Angry);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            TriggerEmotion1(EmotionType.Happy);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            TriggerEmotion1(EmotionType.Sad);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            TriggerEmotion1(EmotionType.Disgusted);
        }   
        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            TriggerEmotion2(EmotionType.Angry);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6)) {
            TriggerEmotion2(EmotionType.Happy);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7)) {
            TriggerEmotion2(EmotionType.Sad);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8)) {
            TriggerEmotion2(EmotionType.Disgusted);
        }
        
        
        
    }
}
