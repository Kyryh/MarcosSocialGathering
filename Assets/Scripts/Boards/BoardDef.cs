using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName="New Board", menuName="Defs/Board")]
public class BoardDef : ScriptableObject
{
    public string boardSceneName;

    public string boardName;

    [TextArea]
    public string boardDescription;

    public Sprite boardPreview;
}
