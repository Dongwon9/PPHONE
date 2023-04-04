using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextActonSprite : TurnActor
{
    SpriteRenderer spriteRenderer;
    private void OnEnable() {
        base.OnEnable();
        spriteRenderer = GetComponent<SpriteRenderer>();
        nextAction = () => spriteRenderer.enabled = false;
    }
    //TurnAction으로 매턴 안 보이게 되려고 한다.
    //Enemy의 movePreTurn으로 다시 보이게 된다.
    protected override void TurnUpdate() {
        base.TurnUpdate();
        nextAction = () => spriteRenderer.enabled = false;
    }
}
