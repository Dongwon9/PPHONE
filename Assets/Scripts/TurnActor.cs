using System;
using UnityEngine;

public abstract class TurnActor : MonoBehaviour {

    /// <summary>
    /// 모든 TurnActor들이 사용하는 다음턴 action
    /// </summary>
    protected Action nextAction;

    private Collider2D colldier;
    private Vector3 moveDir = Vector3.zero;
    protected float timeCounter = -1f;
    private const float movingTime = 0.1f;

    // Start is called before the first frame update
    protected virtual void OnEnable() {
        Player.OnTurnUpdate += TurnUpdate;
        DecideNextAction();
    }

    private void OnDisable() {
        Player.OnTurnUpdate -= TurnUpdate;
    }

    protected virtual void Awake() {
        colldier = GetComponent<Collider2D>();
    }

    private void FixedUpdate() {
        if (timeCounter >= 0) {
            transform.Translate(moveDir * Time.fixedDeltaTime / movingTime);
            timeCounter += Time.deltaTime;
            colldier.enabled = false;
            if (timeCounter >= movingTime) {
                timeCounter = -1f;
                moveDir = Vector3.zero;
                //움직임을 마친 후에 오브젝트의 좌표를 (정수)+0.5로 교정한다.
                transform.position = new Vector3(MathF.Floor(transform.position.x) + 0.5f, MathF.Floor(transform.position.y) + 0.5f);
                colldier.enabled = true;
            }
        }
        Debug.Log(gameObject.ToString() + "frameCounter = " + timeCounter);
    }

    /// <summary>
    /// TurnActor들이 다음 행동을 정할 때 사용하는 함수
    /// </summary>
    protected abstract void DecideNextAction();

    protected virtual void TurnUpdate() {
        if (nextAction == null) {
            Debug.Log(ToString() + "의 nextAction이 null입니다");
        } else {
            nextAction();
        }
        nextAction = null;
        DecideNextAction();
    }

    /// <summary>
    /// position에 공격 경고를 띄운다. 이 함수를 반복적으로 사용해 적의 공격을 구현한다.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="damage"></param>
    public void AttackPreTurn(Vector3 position, int damage, Action onHitEffect = null) {
        var attack = ObjectPool.AttackPool.Get();
        attack.tag = gameObject.tag;
        attack.transform.position = position;
        attack.damage = damage;
        attack.onHitEffect = onHitEffect;
    }

    /// <summary>
    /// dir 방향으로 1칸 이동한다.
    /// </summary>
    /// <param name="dir"></param>
    protected void Move(Direction dir) {
        Vector3 direction = Vector3.zero;
        switch (dir) {
            case Direction.Left:
                direction = Vector3.left;
                break;

            case Direction.Right:
                direction = Vector3.right;
                break;

            case Direction.Up:
                direction = Vector3.up;
                break;

            case Direction.Down:
                direction = Vector3.down;
                break;
        }
        //내가 진행하려는 방향으로 1칸 떨어진 곳에 벽이 있는가?
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.0f, LayerMask.GetMask("Wall"));
        if (hit.collider == null) {
            moveDir = direction;
            timeCounter = 0;
        }
    }

    /// <summary>
    /// 해당 방향으로 움직일 수 있는지만 알고 싶을 때 사용한다.
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    protected bool MoveCheck(Direction dir) {
        Vector3 direction = Vector3.zero;
        switch (dir) {
            case Direction.Left:
                direction = Vector3.left;
                break;

            case Direction.Right:
                direction = Vector3.right;
                break;

            case Direction.Up:
                direction = Vector3.up;
                break;

            case Direction.Down:
                direction = Vector3.down;
                break;
        }
        //내가 진행하려는 방향으로 1칸 떨어진 곳에 벽이 있는지 여부를 반환한다.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.0f, LayerMask.GetMask("Wall"));
        return hit.collider != null;
    }
}