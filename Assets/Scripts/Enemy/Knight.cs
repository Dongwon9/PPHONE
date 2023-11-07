/// <summary>
/// 체스의 나이트처럼 움직이고, 착지하는 자리에 공격한다.
/// </summary>
public class Knight : Enemy {
    private Path pathFinding = new Path();
    private int counter = 0;
    protected override void OnEnable() {
        base.OnEnable();
        pathFinding.NeighborOffset = new (int x, int y)[] {
            (2,1),(2,-1),(-2,1),(-2,-1),
            (1,2),(-1,2),(1,-2),(-1,-2)};
    }

    protected override void TurnUpdate() {
        counter += 1;
        if (counter <= 1) {
            return;
        }

        pathFinding.FindPath(transform.position, Player.Position);
        if (!pathFinding.PathExists) {
            return;
        }
        Move(pathFinding.GetNextPos());
        Attack(pathFinding.GetNextPos(), enemydata.Damage, Target.Player);
        counter = 0;
    }
}