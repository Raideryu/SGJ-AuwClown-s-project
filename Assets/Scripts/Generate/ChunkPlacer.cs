using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class ChunkPlacer : MonoBehaviour
{
    public Transform Player;
    public Chunk[] ChunkPrefabs;
    public Chunk[] TransitionPrefabs;
    //public Chunk[] endsList;
    private List<Chunk> spawnedChunks = new List<Chunk>();
    private List<Chunk> spawnedTransition = new List<Chunk>();
    [SerializeField]
    float DestroyDistance = 20, spawnDistance = 3;

    void Start()
    {
        Player = FindObjectOfType<PlayerInput>().gameObject.transform;

        //SpawnChunk();
    }

    // Update is called once per frame
    void Update()
    {
        List<Chunk> removeList = new List<Chunk>();
        List<Transform> addList = new List<Transform>();

        foreach (Chunk chunk in spawnedTransition)
        {
            if (Vector3.Distance(Player.transform.position, chunk.transform.position) > DestroyDistance)
            {
                removeList.Add(chunk);
            }
            else if (Vector3.Distance(Player.transform.position, chunk.Exits[0].transform.position) < spawnDistance)
            {
                //SpawnAllRooms(chunk.Exits[0]);
                addList.Add(chunk.Exits[0]);
                break;
            }
        }

        //foreach

        //foreach (Chunk chunk in spawnedTransition)


        //    foreach (Transform exit in chunk.Exits)
        //        foreach (Chunk chunk in spawnedChunks)
        //            Destroy(chunk.gameObject);

        //if (Input.GetKeyDown(KeyCode.Space))
        if (Input.GetKeyDown(KeyCode.S))
        {
            SpawnChunk();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Del();
        }

    }

    private void SpawnChunk()
    {
        Chunk newChunk = Instantiate(ChunkPrefabs[Random.Range(0, ChunkPrefabs.Length - 1)], Vector3.zero, Quaternion.identity);

        foreach (var exit in newChunk.Exits)
        {
            SpawnAllRooms(exit);
        }
        spawnedChunks.Add(newChunk);
    }
    [SerializeField]
    int maxRooms = 10;

    void SpawnAllRooms(Transform exit)
    {
        if (spawnedChunks.Count >= maxRooms) return;

        Chunk rndChunk = TransitionPrefabs[Random.Range(0, ChunkPrefabs.Length - 1)];

        Chunk tr = Instantiate(rndChunk, exit.position + rndChunk.Enter.position, exit.rotation);
        tr.transform.position = exit.position + tr.Enter.localPosition;
        spawnedTransition.Add(tr);

        rndChunk = ChunkPrefabs[Random.Range(0, ChunkPrefabs.Length - 1)];
        Chunk room = Instantiate(rndChunk, tr.Exits[0].position + rndChunk.Enter.position, tr.Exits[0].rotation);
        //room.transform.position = tr.Exits[0].position + room.Enter.localPosition;
        spawnedChunks.Add(room);

        foreach (Transform _exit in room.Exits)
        {
            SpawnAllRooms(_exit);
        }
    }


    List<Chunk> SpawnOneRoom(Transform exit)
    {
        //if (spawnedChunks.Count >= maxRooms) return;

        Chunk rndChunk = TransitionPrefabs[Random.Range(0, ChunkPrefabs.Length - 1)];

        Chunk tr = Instantiate(rndChunk, exit.position + rndChunk.Enter.position, exit.rotation);
        tr.transform.position = exit.position + tr.Enter.localPosition;
        //spawnedTransition.Add(tr);

        rndChunk = ChunkPrefabs[Random.Range(0, ChunkPrefabs.Length - 1)];
        Chunk room = Instantiate(rndChunk, tr.Exits[0].position + rndChunk.Enter.position, tr.Exits[0].rotation);
        //room.transform.position = tr.Exits[0].position + room.Enter.localPosition;
        //spawnedChunks.Add(room);

        return new List<Chunk> { tr };
    }

    Chunk SpawnChunk(Transform previosExit, Chunk chunkPref)
    {
        //Chunk rndChunk = TransitionPrefabs[Random.Range(0, ChunkPrefabs.Length - 1)];

        Chunk ch = Instantiate(chunkPref, previosExit.position + chunkPref.Enter.position, previosExit.rotation);
        ch.transform.position = previosExit.position + ch.Enter.localPosition;
        return ch;
    }



    void Del()
    {
        foreach (Chunk chunk in spawnedTransition)
            Destroy(chunk.gameObject);

        foreach (Chunk chunk in spawnedChunks)
            Destroy(chunk.gameObject);
    }


}
