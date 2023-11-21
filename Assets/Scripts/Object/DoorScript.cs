using UnityEngine;

public class DoorScript : MonoBehaviour {
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
        if (!PlayerFrontOfDoor()) {
            return;
        }
        //문을 연다
        GameManager.Instance.WalkableGrid.SetWalkable(transform.position, true);
        GameManager.Instance.WalkableGrid.SetWalkable(transform.position + transform.right, true);
        GameManager.Instance.WalkableGrid.SetWalkable(transform.position - transform.right, true);
        animator.enabled = true;
        if (shadowAnimator) {
            shadowAnimator.enabled = true;
        }
        collider.enabled = false;
        Invoke(nameof(Destroy), 1f);
    }

    private void Destroy() {
        Destroy(gameObject);
    }

    private bool PlayerFrontOfDoor() {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position - transform.right - transform.up, transform.right, 2, LayerMask.GetMask("Player"));
        if (hit.collider != null) {
            return true;
        }
        hit = Physics2D.Raycast(
            transform.position - transform.right + transform.up, transform.right, 2, LayerMask.GetMask("Player"));
        return hit.collider != null;
    }
}