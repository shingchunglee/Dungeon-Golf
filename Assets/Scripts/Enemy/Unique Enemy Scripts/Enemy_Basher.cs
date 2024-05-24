using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy_Basher : EnemyUnit
{
    public float bashForce = 5;
    private bool isPlayerBallHit = false;
    public bool doesDamage = false;

    protected override void AttackPlayer(int xDir, int yDir)
    {
        var modifierX = Random.Range(-0.3f, 0.3f);
        var modifierY = Random.Range(-0.3f, 0.3f);

        var direction = new Vector2(xDir + modifierX, yDir + modifierY);

        if (doesDamage) PlayerManager.Instance.TakeDamage(attackDamage);

        PlayerManager.Instance.ballRB.AddForce(direction * bashForce, ForceMode2D.Impulse);
        StartCoroutine(SetIsPlayerBallHitTrue());

        StartCoroutine(base.AttackAnimation(xDir, yDir));
    }

    private void FixedUpdate()
    {

        if (isPlayerBallHit &&
            !PlayerManager.Instance.IsBallMoving)
        {
            PlayerManager.Instance.TeleportPlayerToBall();

            isPlayerBallHit = false;
        }
    }

    private IEnumerator SetIsPlayerBallHitTrue()
    {
        yield return new WaitForSeconds(1f);

        isPlayerBallHit = true;
    }
}
