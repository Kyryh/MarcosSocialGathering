using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Traversable : MonoBehaviour
{
    [SerializeField]
    private TraversableType traversableType;

    private TraversableType traversableOverride;

    public TraversableType TraversableOverride {
        get {
            return traversableOverride;
        }
        set {
            SetSpace(value != null ? value : traversableType);
            traversableOverride = value;   
        }
    }


    public List<Traversable> nextTraversable = new();

    [DoNotSerialize]
    [HideInInspector]
    public List<Traversable> previousTraversable = new();

    public virtual bool ShouldCountForDiceRolls => traversableType.ShouldCountForDiceRolls;

    public virtual void Awake() {
        foreach (Traversable traversable in nextTraversable) {
            traversable.previousTraversable.Add(this);
        }
    }

    private void SetSpace(TraversableType traversableType) {
        // set the icon here or something idk i'll figure it out
    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        foreach (var traversable in nextTraversable) {
            if (traversable != null) {

                Vector3 direction = (transform.position - traversable.transform.position)/ 5;
                Vector3 arrowEnd1 = traversable.transform.position + Quaternion.Euler(0, 30, 0) * direction;
                Vector3 arrowEnd2 = traversable.transform.position + Quaternion.Euler(0, -30, 0) * direction;

                Gizmos.DrawLine(transform.position, traversable.transform.position);
                Gizmos.DrawLine(traversable.transform.position, arrowEnd1);
                Gizmos.DrawLine(traversable.transform.position, arrowEnd2);
                Gizmos.DrawLine(arrowEnd1, arrowEnd2);

            }
        }

    }
}
