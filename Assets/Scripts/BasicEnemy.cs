using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;

public class BasicEnemy : Enemy {
    private List<EnemyAction> sampleAI;
    private int counter = 0;
    void LaserPreTurn() {
        for (int i = 1; i <= 10; i++) {
            AttackPreTurn(-i, 0, 2);
        }
    }
    private void Update() {
        if (nextAction != null)
            return;
        nextAction = sampleAI[counter].TurnAction;
        sampleAI[counter].PreTurnAction();
    }
    protected override void OnEnable() {
        base.OnEnable();
        sampleAI = new List<EnemyAction> {
            new EnemyAction(()=>{return; },()=>{return; })
        //MoveAction(Direction.Left),
        //MoveAction(Direction.Left),
        //MoveAction(Direction.Right),
        //MoveAction(Direction.Right),
        //new EnemyAction(()=>AttackPreTurn(-1,0,1),()=>{return; }),
        //MoveAction(Direction.Up),
        //MoveAction(Direction.Up),
        //MoveAction(Direction.Down),
        //new EnemyAction(LaserPreTurn,()=>{return; })
        };
        nextAction = sampleAI[0].TurnAction;
        sampleAI[0].PreTurnAction();
    }
    protected override void TurnUpdate() {
        base.TurnUpdate();
        counter = (counter + 1) % sampleAI.Count;
    }
}
