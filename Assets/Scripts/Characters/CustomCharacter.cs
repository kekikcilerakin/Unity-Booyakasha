using UnityEngine;

public class CustomCharacter : MonoBehaviour
{
  [SerializeField] private CharacterSO characterSO;
  [SerializeField] private SkinnedMeshRenderer skinRenderer;

  private void Start()
  {
    Color skinColor = CharacterCreationManager.Instance.SkinColorData.GetColor(characterSO.SkinColorIndex);
    if (skinRenderer != null)
    {
      skinRenderer.material.color = skinColor;
    }
  }
}
