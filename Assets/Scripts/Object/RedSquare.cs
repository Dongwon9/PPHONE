using UnityEngine;
using UnityEngine.Pool;

public class RedSquare : TurnActor {
    private const float lifeTime = 2 * MovingTurnActor.movingTime;
    private bool destroying;
    private IObjectPool<RedSquare> managedPool;
    private float timeCount = 0f;
    public bool instant;

    private void Update() {
        if (destroying) {
            timeCount += Time.deltaTime;
            if (timeCount >= lifeTime) {
                managedPool.Release(this);
                timeCount = 0f;
            }
        }
        if (instant) {
            destroying = true;
        }
    }

    protected override void DecideNextAction() {
        nextAction += () => destroying = true;
    }

    public void SetManagedPool(IObjectPool<RedSquare> pool) {
        managedPool = pool;
    }
}