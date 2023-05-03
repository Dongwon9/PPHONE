public class DestroyableWall : Enemy {

    public override void TakeDamage(int damage) {
        HP -= 1;
        if (HP <= 0) {
            GameManager.WalkableGrid.SetGridObject(transform.position, true);
            Destroy(gameObject);
        }
    }

    protected override void DecideNextAction() => nextAction = () => { };
}