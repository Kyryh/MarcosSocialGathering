#if UNITY_EDITOR
using System.Collections;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

public class TraversableEditor {


    [MenuItem("Traversables/Connect with path")]
    private static void CreatePath() {
        var traversables = GetSelectedTraversables();

        for (int i = 0; i < traversables.Count-1; i++)
        {
            AddNextTraversable(traversables[i], traversables[i + 1]);
        }

        
    }


    [MenuItem("Traversables/Connect all to last")]
    private static void ConnectAllToLast() {
        var traversables = GetSelectedTraversables();

        for (int i = 0; i < traversables.Count - 1; i++) {
            AddNextTraversable(traversables[i], traversables[traversables.Count - 1]);
        }
    }


    [MenuItem("Traversables/Connect first to all")]
    private static void ConnectFirstToAll() {
        var traversables = GetSelectedTraversables();

        for (int i = 1; i < traversables.Count; i++) {
            AddNextTraversable(traversables[0], traversables[i]);
        }
    }

    [MenuItem("Traversables/Connect with path", true)]
    [MenuItem("Traversables/Connect all to last", true)]
    [MenuItem("Traversables/Connect first to all", true)]
    private static bool Validate() => GetSelectedTraversables().Count() > 1;

    private static List<Traversable> GetSelectedTraversables() {
        return Selection.transforms.Select(t => t.GetComponent<Traversable>()).Where(t => t != null).ToList();
    }

    private static void AddNextTraversable(Traversable traversable, Traversable nextTraversable) {
        if (traversable.nextTraversables.Contains(nextTraversable))
            return;
        var serializedObject = new SerializedObject(traversable);
        serializedObject.Update();
        var nextTraversables = serializedObject.FindProperty("nextTraversables");

        int index = nextTraversables.arraySize;
        nextTraversables.InsertArrayElementAtIndex(index);

        nextTraversables.GetArrayElementAtIndex(index).objectReferenceValue = nextTraversable;

        serializedObject.ApplyModifiedProperties();
    }
}
#endif