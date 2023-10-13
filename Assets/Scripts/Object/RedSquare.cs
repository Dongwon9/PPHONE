public class RedSquare : TurnActor {
    protected override void TurnUpdate() {
        Destroy(gameObject);
    }
}