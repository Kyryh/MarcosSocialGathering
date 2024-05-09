using System.Collections;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class TraversableEditor : MonoBehaviour {


    private void Awake() {
        if (!Application.isPlaying && TryGetComponent<Traversable>(out var traversable)) {
            foreach(var transform in Selection.transforms) {
                if (transform.TryGetComponent<Traversable>(out var other)) {
                    other.nextTraversable.Add(traversable);
                }
            }
        }
    }
}
