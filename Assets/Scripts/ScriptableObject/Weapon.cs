using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "새 무기")]
public class Weapon : ScriptableObject {
    /// <summary>
    /// 플레이어가 오른쪽을 보고 있을 때를 기준으로, 플레이어를 중심으로 어디를 공격하는가?<br></br>
    /// 플레이어가 회전하면, 이것을 똑같이 회전시켜서 적용한다.
    /// </summary>

    public Sprite icon;
    public new string name;
    public int damage;
    public List<Vector2> attackSquare = new List<Vector2>();
    public List<Vector2> GetAttackSquare(Direction direction = Direction.Right) {
        List<Vector2> result = new();
        switch (direction) {
            case Direction.Right:
                return attackSquare;

            case Direction.Left:
                foreach (Vector2 square in attackSquare) {
                    result.Add(square * new Vector2(-1, -1));
                }
                break;

            case Direction.Up:
                foreach (Vector2 square in attackSquare) {
                    result.Add(new Vector2(square.y * -1, square.x));
                }
                break;

            case Direction.Down:
                foreach (Vector2 square in attackSquare) {
                    result.Add(new Vector2(square.y, square.x * -1));
                }
                break;
        }
        return result;
    }
}