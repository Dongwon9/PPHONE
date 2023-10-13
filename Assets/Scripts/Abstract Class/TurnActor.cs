using System;
using UnityEngine;

/// <summary>
/// 턴마다 어떤 행동을 하는 모든 오브젝트는 TurnActor를 상속한다.
/// </summary>
public abstract class TurnActor : MonoBehaviour {
    /// <summary>모든 TurnActor들이 사용하는 다음턴 action</summary>

    public enum Direction { Left, Up, Right, Down };

    public enum Target { Player, Enemy, Any, Wall }

    public interface IDamagable {
        public void TakeDamage(int damage);
    }

    /// <summary> position 칸에 공격을 한다.</summary>
    /// <param name="target">공격할 대상(플레이어,적 또는 둘다)</param>
    protected void Attack(Vector3 position, int damage, Target target) {
        RaycastHit2D[] hits = Physics2D.RaycastAll(position, Vector2.down, 0.1f);
        foreach (RaycastHit2D hit in hits) {
            switch (target) {
                case Target.Player:
                    hit.collider.GetComponent<Player>()?.TakeDamage(damage);
                    break;

                case Target.Enemy:
                    hit.collider.GetComponent<Enemy>()?.TakeDamage(damage);
                    break;

                case Target.Any:
                    hit.collider.GetComponent<IDamagable>()?.TakeDamage(damage);
                    break;
            }
        }
    }

    /// <param name="pierce">이 레이저는 벽을 만날때까지 관통하는 레이저인가?</param>
    protected void LaserAttack(Vector3 origin, Vector2 direction, int damage, Target target, bool pierce = false) {
        float laserLength = Physics2D.Raycast(origin, direction, 99999, LayerMask.GetMask("Wall")).distance;
        RaycastHit2D[] hits;
        if (pierce) {
            hits = Physics2D.RaycastAll(origin, direction, laserLength);
        } else {
            hits = new RaycastHit2D[1];
            hits[0] = Physics2D.Raycast(origin, direction, laserLength);
        }
        foreach (RaycastHit2D hit in hits) {
            switch (target) {
                case Target.Player:
                    hit.collider.GetComponent<Player>()?.TakeDamage(damage);
                    break;

                case Target.Enemy:
                    hit.collider.GetComponent<Enemy>()?.TakeDamage(damage);
                    break;

                case Target.Any:
                    hit.collider.GetComponent<IDamagable>()?.TakeDamage(damage);
                    break;
            }
        }
    }

    /// <summary> TurnActor들이 다음 행동을 정할 때 사용하는 함수</summary>

    protected virtual void OnDisable() {
        Player.OnTurnUpdate -= TurnUpdate;
    }

    protected virtual void OnEnable() {
        Player.OnTurnUpdate += TurnUpdate;
    }

    /// <summary>현재 턴의 행동을 실행하는 함수<br></br>
    /// 행동 코드는 이 함수를 오버라이드해 그 안에 구현한다.
    /// </summary>
    protected abstract void TurnUpdate();

    public static Vector3 DirectionToVector(Direction dir) {
        switch (dir) {
            case Direction.Left:
                return Vector3.left;

            case Direction.Right:
                return Vector3.right;

            case Direction.Up:
                return Vector3.up;

            case Direction.Down:
                return Vector3.down;

            default:
                return Vector3.zero;
        }
    }

    public bool CheckForObjectAtPosition(Target[] objectType, Vector3 position) {
        if (objectType.Length == 0) {
            Debug.LogError("objectType 매개변수에 요소가 하나도 없습니다");
            return false;
        }
        string[] objects = new string[objectType.Length];
        for (int i = 0; i < objects.Length; i++) {
            switch (objectType[i]) {
                case Target.Enemy:
                    objects[i] = "Enemy";
                    break;

                case Target.Player:
                    objects[i] = "Player";
                    break;

                case Target.Wall:
                    objects[i] = "Wall";
                    break;
            }
        }
        return Physics2D.Raycast(position, Vector2.down, 0.1f, LayerMask.GetMask(objects)).collider == null;
    }

    public Vector2 Round(Vector2 orig) {
        return new Vector2(
         MathF.Round(orig.x),
         MathF.Round(orig.y));
    }

    /// <summary>
    /// position에 공격을 경고하는 빨간 사각형을 생성한다.
    /// </summary>
    public void AttackWarning(Vector3 position) {
        Instantiate(GameManager.Instance.redSquare, position, Quaternion.identity);
    }

    public void CreateSlash(Vector3 position) {
        Quaternion rotation = Quaternion.identity;
        //기본:오른쪽
        if (position.x < transform.position.x) { //왼쪽
            rotation = Quaternion.Euler(0, 0, 180);
        }
        if (position.y > transform.position.y) { //위
            rotation = Quaternion.Euler(0, 0, 90);
        } else if (position.y < transform.position.y) { //아래
            rotation = Quaternion.Euler(0, 0, -90);
        }
        Instantiate(GameManager.Instance.slash, position, rotation);
    }
}