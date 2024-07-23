#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

// thanks to Dynamite3D
// https://discussions.unity.com/t/editor-script-to-make-play-button-always-jump-to-a-start-scene/68990/9

[InitializeOnLoad]
public static class DefaultSceneLoader {
    static DefaultSceneLoader() {
        EditorApplication.playModeStateChanged += LoadDefaultScene;
    }

    static void LoadDefaultScene(PlayModeStateChange state) {
        if (state == PlayModeStateChange.ExitingEditMode) {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }

        if (state == PlayModeStateChange.EnteredPlayMode) {
            EditorSceneManager.LoadScene(0);
        }
    }
}
#endif