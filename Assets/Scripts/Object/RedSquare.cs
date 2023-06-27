using UnityEngine.Pool;

public class RedSquare : TurnActor {
    private const float lifeTime = 2 * MovingTurnActor.movingTime;
    private bool destroying;
    private IObjectPool<RedSquare> managedPool;
    private float timeCount = 0f;
    public bool instant;

    private void Update() {
        if (instant) {
            destroying = true;
        }
    }

    protected override void DecideNextAction() {
        nextAction = destroying ? () => { managedPool.Release(this); }
        : () => { destroying = true; };
    }

    public void SetManagedPool(IObjectPool<RedSquare> pool) {
        managedPool = pool;
    }
}