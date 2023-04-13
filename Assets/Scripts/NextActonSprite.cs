using UnityEngine;

public class NextActonSprite : TurnActor {
    private SpriteRenderer spriteRenderer;

    private void OnEnable() {
        base.OnEnable();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //TurnAction으로 매턴 안 보이게 되려고 한다.
    //Enemy의 movePreTurn으로 다시 보이게 된다.
    protected override void DecideNextAction() {
        nextAction = () => spriteRenderer.enabled = false;
    }
}