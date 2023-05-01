using UnityEngine;

public class DoorScript : MonoBehaviour {
    private Animator animator;
    private new Collider2D collider;

    private void Awake() {
        animator = GetComponent<Animator>();
        animator.enabled = false;
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    private void Update() {
        RaycastHit2D hit =
          Physics2D.Raycast(transform.position - transform.right - transform.up, transform.right, 2, LayerMask.GetMask("Player"));
        if (hit.collider == null) {
            hit = Physics2D.Raycast(transform.position - transform.right + transform.up, transform.right, 2, LayerMask.GetMask("Player"));
            if (hit.collider == null) {
                return;
            }
        }
        GameManager.Instance.WalkableGrid.SetGridObject(transform.position, true);
        GameManager.Instance.WalkableGrid.SetGridObject(transform.position + transform.right, true);
        GameManager.Instance.WalkableGrid.SetGridObject(transform.position - transform.right, true);
        animator.enabled = true;
        collider.enabled = false;
    }
}