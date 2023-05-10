using UnityEngine;

public class DoorScript : TurnActor {
    private Animator animator;

    protected override void Awake() {
        base.Awake();
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    protected override void DecideNextAction() {
        //TODO: 턴 진행이 끝난 후에도 플레이어가 레이캐스트에 닿아있으면
        //문이 열린다.
        nextAction = () => { };
    }

    private void Update() {
        RaycastHit2D hit =
          Physics2D.Raycast(transform.position - transform.right - transform.up, transform.right, 2, LayerMask.GetMask("Player"));
        if (hit.collider == null) {
            hit = Physics2D.Raycast(transform.position - transform.right + transform.up, transform.right, 2, LayerMask.GetMask("Player"));
            if (hit.collider == null) {
                return;
            }
        }

        GameManager.WalkableGrid.SetGridObject(transform.position, true);
        GameManager.WalkableGrid.SetGridObject(transform.position + transform.right, true);
        GameManager.WalkableGrid.SetGridObject(transform.position - transform.right, true);
        animator.enabled = true;
        collider.enabled = false;
    }
}