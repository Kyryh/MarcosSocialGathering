using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Space : Traversable
{
    public Sprite spaceIcon;

    public SpriteRenderer spaceIconRenderer;
    public virtual bool ShouldCountForDiceRolls => true;
    public virtual void OnLanding() {

    }

    public override void Awake() {
        if (spaceIconRenderer != null) { 
            spaceIconRenderer.sprite = spaceIcon;
        }
    }
    private void OnValidate() {
        Awake();
    }
}
