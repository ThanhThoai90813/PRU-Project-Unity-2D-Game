﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MenuController))]
public class MenuEditor : Editor {

    int selectedOption = 0;
    private MenuController menu;

    public override void OnInspectorGUI()
    {
        menu = target as MenuController;
        //base.OnInspectorGUI();
        string[] labels = new string[] { "Normal Backgrounds", "Parallax Backgrounds"};
        selectedOption = GUILayout.SelectionGrid(selectedOption, labels, 2);
        switch (selectedOption)
        {
            case 0:
                menu.useParallax = false;
                menu.useKeys = EditorGUILayout.Toggle("Use keyboard keys",menu.useKeys);
                EditorGUILayout.HelpBox("If deactivated the menu will only use the ingame arrows.", MessageType.Info);
                serializedObject.Update();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("mainBackground"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("backgrounds"),true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("options"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Select"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("SceneSelect"), true);
                EditorGUILayout.HelpBox("The audio that will be played in the menu.", MessageType.Info);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Events"), true);
                EditorGUILayout.LabelField("🎨 UI Elements", EditorStyles.boldLabel);
                menu.menuText = (UnityEngine.UI.Text)EditorGUILayout.ObjectField("Menu Text", menu.menuText, typeof(UnityEngine.UI.Text), true);
                menu.menuBar = (GameObject)EditorGUILayout.ObjectField("Menu Bar", menu.menuBar, typeof(GameObject), true);
                menu.exitMenu = (GameObject)EditorGUILayout.ObjectField("Exit Menu", menu.exitMenu, typeof(GameObject), true);
                menu.OptionsMenu = (GameObject)EditorGUILayout.ObjectField("Options Menu", menu.OptionsMenu, typeof(GameObject), true);
                serializedObject.ApplyModifiedProperties();
                break;

            case 1:
                menu.useParallax = true;
                menu.useKeys = EditorGUILayout.Toggle("Use keyboard keys", menu.useKeys);
                EditorGUILayout.HelpBox("If deactivated the menu will only use the ingame arrows.", MessageType.Info);
                serializedObject.Update();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("mainBackgroundParallax"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("backgroundsParallax"), true);            
                EditorGUILayout.PropertyField(serializedObject.FindProperty("options"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Select"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("SceneSelect"), true);
                EditorGUILayout.HelpBox("The audio that will be played in the menu.", MessageType.Info);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Events"), true);
                EditorGUILayout.LabelField("🎨 UI Elements", EditorStyles.boldLabel);
                menu.menuText = (UnityEngine.UI.Text)EditorGUILayout.ObjectField("Menu Text", menu.menuText, typeof(UnityEngine.UI.Text), true);
                menu.menuBar = (GameObject)EditorGUILayout.ObjectField("Menu Bar", menu.menuBar, typeof(GameObject), true);
                menu.exitMenu = (GameObject)EditorGUILayout.ObjectField("Exit Menu", menu.exitMenu, typeof(GameObject), true);
                menu.OptionsMenu = (GameObject)EditorGUILayout.ObjectField("Options Menu", menu.OptionsMenu, typeof(GameObject), true);

                serializedObject.ApplyModifiedProperties();
                break;

        }
    }


}



