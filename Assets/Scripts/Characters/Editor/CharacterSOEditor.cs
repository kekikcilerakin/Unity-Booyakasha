using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(CharacterSO))]
public class CharacterSOEditor : Editor
{
  private SkinColorData skinColorData;

  private SerializedProperty characterNameProp;
  private SerializedProperty nameColorProp;
  private SerializedProperty skinColorIndexProp;

  private void OnEnable()
  {
    characterNameProp = serializedObject.FindProperty("characterName");
    nameColorProp = serializedObject.FindProperty("nameColor");
    skinColorIndexProp = serializedObject.FindProperty("skinColorIndex");

    // Find SkinColorData asset
    FindSkinColorData();
  }

  private void FindSkinColorData()
  {
    // Load SkinColorData from Resources folder
    skinColorData = Resources.Load<SkinColorData>("SkinColorData");

    if (skinColorData == null)
    {
      Debug.LogWarning("SkinColorData not found in Resources folder. Skin color preview won't be available.");
    }
  }

  public override void OnInspectorGUI()
  {
    serializedObject.Update();

    EditorGUILayout.PropertyField(characterNameProp);
    EditorGUILayout.PropertyField(nameColorProp);

    // Draw skin color index
    EditorGUILayout.PropertyField(skinColorIndexProp);

    // Get current skin color index
    int skinIndex = skinColorIndexProp.intValue;

    // Check if we have skin color data
    if (skinColorData != null)
    {
      // Draw color preview
      Rect colorRect = EditorGUILayout.GetControlRect(false, 20);
      EditorGUI.DrawRect(colorRect, skinColorData.GetColor(skinIndex));
    }
    else
    {
      EditorGUILayout.HelpBox("SkinColorData not found in Resources folder.", MessageType.Warning);
    }

    serializedObject.ApplyModifiedProperties();
  }
}