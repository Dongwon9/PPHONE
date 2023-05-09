using UnityEngine;

public class NextActonSprite : TurnActor {
    //TurnAction���� ���� �� ���̰� �Ƿ��� �Ѵ�.
    //Enemy�� movePreTurn���� �ٽ� ���̰� �ȴ�.
    protected override void DecideNextAction() {
        nextAction = () => spriteRenderer.enabled = false;
    }

    protected override void OnEnable() {
        base.OnEnable();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}