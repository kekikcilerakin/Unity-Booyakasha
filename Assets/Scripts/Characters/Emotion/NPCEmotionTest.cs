using UnityEngine;

public class NPCEmotionTest : MonoBehaviour
{
    [SerializeField] private NPCEmotionController emotionController;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            emotionController.SetEmotion(NPCEmotionType.Happy);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            emotionController.SetEmotion(NPCEmotionType.Sad);
        }
    }
}
