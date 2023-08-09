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

    protected override void TurnUpdate() {
        if (destroying) {
            managedPool.Release(this);
        } else { destroying = true; }
    }

    public void SetManagedPool(IObjectPool<RedSquare> pool) {
        managedPool = pool;
    }
}