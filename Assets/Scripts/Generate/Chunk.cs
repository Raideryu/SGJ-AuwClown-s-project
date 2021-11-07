using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Chunk : MonoBehaviour
{
    public Transform Enter;
    public Transform EnterRoom;

    public Transform[] Exits;

    [HideInInspector]
    public bool isRoom=true;

    [HideInInspector]
    private List<ExitInfo> exitsInfo = new List<ExitInfo>();

    public bool isOpen = false; 

    public List<ExitInfo> ExitsInfo
    {
        get
        {
            if (exitsInfo.Count != Exits.Length)
            {
                exitsInfo.Clear();
                for (int i = 0; i < Exits.Length; i++)
                {
                    ExitInfo exInf = new ExitInfo(Exits[i]);
                    exitsInfo.Add(exInf);
                }
            }

            return exitsInfo;
        }
        
    }
    public List<Enemy> currentEnemys = new List<Enemy>();
    public void SpawnEnemys(GameObject enemyPrefab)
    {
        currentEnemys.Clear();
        List<PatrolPoints> patrolsP = GetComponentsInChildren<PatrolPoints>().ToList();
        
        foreach(PatrolPoints point in patrolsP)
        {
            Enemy enemy = Instantiate(enemyPrefab, point.transform.position, point.transform.rotation).GetComponent<Enemy>();
            enemy.SetPatrolsPoints(patrolsP);
            currentEnemys.Add(enemy);
        }
    }

    public void SetOpen()
    {
        // открыть двери
    }

}
public class ExitInfo
{
    public Transform transform;
    public bool alreadeGenerated;


    public ExitInfo(Transform tr)
    {
        transform = tr;
        alreadeGenerated = false;

    }
}