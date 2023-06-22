using UnityEngine;

public class DoorScript : TurnActor {
    private Animator animator;
    private new Collider2D collider;
    [SerializeField] private Animator shadowAnimator;
    private void Awake() {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        animator.enabled = false;
        if (shadowAnimator != null) {
            shadowAnimator.enabled = false;
        }
    }

    private void Update() {
        //플레이어가 문 앞에 있는지 확인
        if (PlayerFrontOfDoor()) {
            //문을 연다
            OpenDoor();
        }

        void OpenDoor() {
            GameManager.WalkableGrid.SetGridObject(transform.position, true);
            GameManager.WalkableGrid.SetGridObject(transform.position + transform.right, true);
            GameManager.WalkableGrid.SetGridObject(transform.position - transform.right, true);
            animator.enabled = true;
            if (shadowAnimator) {
                shadowAnimator.enabled = true;
            }
            collider.enabled = false;
        }

        bool PlayerFrontOfDoor() {
            RaycastHit2D hit =
            Physics2D.Raycast(transform.position - transform.right - transform.up, transform.right, 2, LayerMask.GetMask("Player"));
            if (hit.collider == null) {
                hit = Physics2D.Raycast(transform.position - transform.right + transform.up, transform.right, 2, LayerMask.GetMask("Player"));
                if (hit.collider == null) {
                    return false;
                }
            }
            return true;
        }
    }
}