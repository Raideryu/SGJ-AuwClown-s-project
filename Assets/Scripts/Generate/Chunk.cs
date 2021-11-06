using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Transform Enter;
    public Transform EnterRoom;

    public Transform[] Exits;

    [HideInInspector]
    public bool isRoom=true;

    [HideInInspector]
    private List<ExitInfo> exitsInfo = new List<ExitInfo>();

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