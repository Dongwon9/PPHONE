using System.Collections.Generic;
using UnityEngine;

public class Boss4 : Enemy, IDamagable {

    private enum AIMode { Moving, Attacking };

    private int turnsSinceTeleport = 0;
    private Direction[] directions = { Direction.Right, Direction.Up, Direction.Left, Direction.Down };
    private int[] directionCoolTime = { 0, 0, 0, 0 };
    private AIMode mode = AIMode.Moving;
    private int counter = 0;

    protected override void TurnUpdate() {
        if (mode == AIMode.Attacking) {
            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    Vector3 targetPosition = transform.position + new Vector3(x, y, 0);
                    if (targetPosition != transform.position) {
                        Attack(targetPosition, enemydata.Damage, Target.Player);
                        CreateSlash(targetPosition);
                    }
                }
            }
            animator.SetTrigger("attack02");
            mode = AIMode.Moving;
            return;
        }

        turnsSinceTeleport++;
        if (turnsSinceTeleport >= 5) {
            List<Vector3> teleportPositions = new List<Vector3>();
            foreach (var dir in directions) {
                Vector3 v = Player.Position + DirectionToVector(dir);
                if (CheckForObjectAtPosition(new Target[] { Target.Wall }, v) && (v != transform.position)) {
                    teleportPositions.Add(v);
                }
            }
            transform.position = teleportPositions[Random.Range(0, teleportPositions.Count)];
            turnsSinceTeleport = 0;
            for (int i = 0; i < 4; i++) {
                directionCoolTime[i] = 0;
            }
            mode = AIMode.Attacking;
        } else {
            counter++;
            if (counter >= 2) {
                counter = 0;
                List<Direction> canMoveTo = new();
                for (int i = 0; i < 4; i++) {
                    if (CanMove(directions[i]) && (directionCoolTime[i] == 0)) {
                        canMoveTo.Add(directions[i]);
                    }
                }
                if (canMoveTo.Count > 0) {
                    Direction pickedDir = canMoveTo[Random.Range(0, canMoveTo.Count)];
                    Move(pickedDir);
                    animator.SetTrigger("walk");
                    for (int i = 0; i < 4; i++) {
                        if (directionCoolTime[i] > 0) {
                            directionCoolTime[i]--;
                        }
                    }
                    switch (pickedDir) {
                        case Direction.Left:
                            if (directionCoolTime[0] == 0) {
                                directionCoolTime[0] = 2;
                            }
                            break;

                        case Direction.Right:
                            if (directionCoolTime[2] == 0) {
                                directionCoolTime[2] = 2;
                            }
                            break;

                        case Direction.Up:
                            if (directionCoolTime[3] == 0) {
                                directionCoolTime[3] = 2;
                            }
                            break;

                        case Direction.Down:
                            if (directionCoolTime[1] == 0) {
                                directionCoolTime[1] = 2;
                            }
                            break;
                    }
                }
            }
        }
    }
}