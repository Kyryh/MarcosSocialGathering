using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Space : Traversable
{
    public Sprite spaceIcon;
    public virtual bool ShouldCountForDiceRolls => true;
    public virtual void OnLanding() {

    }
}
