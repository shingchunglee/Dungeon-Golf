using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<EnemyUnit> enemyUnitsOnLevel = new List<EnemyUnit>();

    private void Start()
    {

    }


    public void AddEnemyToList(EnemyUnit enemy)
    {
        enemyUnitsOnLevel.Add(enemy);
    }

    public void RemoveEnemyFromList(EnemyUnit enemy)
    {
        int index = enemyUnitsOnLevel.FindIndex(i => i.ID == enemy.ID);

        //Index is -1 if the FindIndex call above didn't find anything
        if (index != -1)
        {
            enemyUnitsOnLevel.RemoveAt(index);
        }
        else
        {
            Debug.Log("Error: Enemy not found on enemyUnits list.");
        }
    }

    public void EnemyTurn()
    {
        foreach (EnemyUnit enemy in enemyUnitsOnLevel)
        {
            enemy.TakeTurn();
        }
    }
}