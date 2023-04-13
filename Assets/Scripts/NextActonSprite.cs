using UnityEngine;

public class NextActonSprite : TurnActor {
    private SpriteRenderer spriteRenderer;

    private void OnEnable() {
        base.OnEnable();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //TurnAction���� ���� �� ���̰� �Ƿ��� �Ѵ�.
    //Enemy�� movePreTurn���� �ٽ� ���̰� �ȴ�.
    protected override void DecideNextAction() {
        nextAction = () => spriteRenderer.enabled = false;
    }
}