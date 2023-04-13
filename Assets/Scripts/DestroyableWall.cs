public class DestroyableWall : Enemy {

    protected override void DecideNextAction() => nextAction = () => { };

    public override void TakeDamage(int damage) {
        HP -= 1;
        if (HP <= 0) {
            Destroy(gameObject);
        }
    }
}