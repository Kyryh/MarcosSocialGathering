﻿#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(NetworkSelectable), true)]
[CanEditMultipleObjects]
internal class NetworkSelectableEditor : SelectableEditor {

    SerializedProperty serverOnlyProperty;
    protected override void OnEnable() {
        base.OnEnable();
        serverOnlyProperty = serializedObject.FindProperty("serverOnly");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serverOnlyProperty);
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Space();

        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(NetworkButton), true)]
[CanEditMultipleObjects]
internal class NetworkButtonEditor : ButtonEditor {

    SerializedProperty serverOnlyProperty;
    protected override void OnEnable() {
        base.OnEnable();
        serverOnlyProperty = serializedObject.FindProperty("serverOnly");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serverOnlyProperty);
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Space();

        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(NetworkDropdown), true)]
[CanEditMultipleObjects]
internal class NetworkDropdownEditor : TMPro.EditorUtilities.DropdownEditor {

    SerializedProperty serverOnlyProperty;
    protected override void OnEnable() {
        base.OnEnable();
        serverOnlyProperty = serializedObject.FindProperty("serverOnly");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serverOnlyProperty);
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Space();

        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(NetworkInputField), true)]
[CanEditMultipleObjects]
internal class NetworkInputFieldEditor : TMPro.EditorUtilities.TMP_InputFieldEditor {

    SerializedProperty serverOnlyProperty;
    protected override void OnEnable() {
        base.OnEnable();
        serverOnlyProperty = serializedObject.FindProperty("serverOnly");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serverOnlyProperty);
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Space();

        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(NetworkScrollbar), true)]
[CanEditMultipleObjects]
internal class NetworkScrollbarEditor : ScrollbarEditor {

    SerializedProperty serverOnlyProperty;
    protected override void OnEnable() {
        base.OnEnable();
        serverOnlyProperty = serializedObject.FindProperty("serverOnly");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serverOnlyProperty);
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Space();

        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(NetworkSlider), true)]
[CanEditMultipleObjects]
internal class NetworkSliderEditor : SliderEditor {

    SerializedProperty serverOnlyProperty;
    protected override void OnEnable() {
        base.OnEnable();
        serverOnlyProperty = serializedObject.FindProperty("serverOnly");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serverOnlyProperty);
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Space();

        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(NetworkToggle), true)]
[CanEditMultipleObjects]
internal class NetworkToggleEditor : ToggleEditor {

    SerializedProperty serverOnlyProperty;
    protected override void OnEnable() {
        base.OnEnable();
        serverOnlyProperty = serializedObject.FindProperty("serverOnly");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serverOnlyProperty);
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Space();

        base.OnInspectorGUI();
    }
}


#endif
