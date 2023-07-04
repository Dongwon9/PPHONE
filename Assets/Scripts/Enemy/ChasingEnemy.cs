/// <summary>플레이어를 쫓아와서 거리가 1칸이 되면 공격하는 적</summary>
public class ChasingEnemy : Enemy {
    private Path pathfinding;

    protected override void DecideNextAction() {
        pathfinding.FindPath(transform.position, Player.Position);
        if (!pathfinding.PathExists) {
            nextAction = () => { };
            return;
        }
        //필요한 이동 길이가 1칸보다 많으면 움직인다.
        if (pathfinding.PathLength > 1) {
            nextAction = () => Move(pathfinding.GetNextPos());
        } else {
            //필요한 이동 길이가 1칸이면 공격한다.
            AttackWarning(pathfinding.GetNextPos());
            nextAction = () => Attack(pathfinding.GetNextPos(), 5, Target.Player);
        }
    }

    protected override void OnEnable() {
        Player.OnTurnUpdate += TurnUpdate;
        pathfinding = new Path();
        pathfinding.FindPath(transform.position, Player.Position);
        DecideNextAction();
    }
}