public class EffectInstance {
    private int timeLeft;
    private int counter;
    private MovingTurnActor target;
    public Effect effectData;
    public void TurnUpdate() {
        if (timeLeft <= 0) { return; }
        effectData.specialFunction(target);
        timeLeft -= 1;
        if (timeLeft <= 0) {
            effectData = null;
        }
    }
}