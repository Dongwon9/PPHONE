using UnityEngine;

public class NextActonSprite : TurnActor {
    //TurnAction으로 매턴 안 보이게 되려고 한다.
    //Enemy의 movePreTurn으로 다시 보이게 된다.
    private SpriteRenderer spriteRenderer;
    protected override void TurnUpdate() {
        spriteRenderer.enabled = false;
    }

    protected override void OnEnable() {
        base.OnEnable();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}