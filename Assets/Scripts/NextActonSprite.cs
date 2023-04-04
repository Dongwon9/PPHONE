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
    //TurnAction���� ���� �� ���̰� �Ƿ��� �Ѵ�.
    //Enemy�� movePreTurn���� �ٽ� ���̰� �ȴ�.
    protected override void TurnUpdate() {
        base.TurnUpdate();
        nextAction = () => spriteRenderer.enabled = false;
    }
}
