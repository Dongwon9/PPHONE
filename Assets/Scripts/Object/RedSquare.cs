using UnityEngine.Pool;

public class RedSquare : TurnActor {
    private const float lifeTime = 2 * MovingTurnActor.movingTime;
    private bool destroying;
    public bool instant;

    private void Update() {
        if (instant) {
            destroying = true;
        }
    }

    protected override void TurnUpdate() {
        if (destroying) {
           Destroy(gameObject);
        } else { destroying = true; }
    }
}