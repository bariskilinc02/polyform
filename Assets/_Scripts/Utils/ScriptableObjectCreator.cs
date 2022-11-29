using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public static class ScriptableObjectCreator
{
    public static void CreateLevelScriptableObject(Level toCreate, int levelIndex)
    {
        AssetDatabase.CreateAsset(toCreate, "Assets/Resources/RecipeObject/" + "Level_" + levelIndex + ".asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        //Selection.activeObject = ;
    }
}
