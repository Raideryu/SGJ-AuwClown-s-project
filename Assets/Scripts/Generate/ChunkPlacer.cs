using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class ChunkPlacer : MonoBehaviour
{
    [SerializeField] Chunk startRoomPrefab;
    [SerializeField] Chunk finishRoomPrefab;
    [SerializeField] Chunk[] ChunkPrefabs;
    [SerializeField] Chunk[] TransitionPrefabs;

    [SerializeField] int mainPathRoomsCount = 5;
    [SerializeField] int tupikPathRoomsCount = 2;

    private List<Chunk> spawnedChunks = new List<Chunk>();
    //private List<Chunk> spawnedTransition = new List<Chunk>();

    private Chunk startRoom;
    private Chunk finishRoom;

    void GeneratePath()
    {
        foreach (Chunk chunks in spawnedChunks)
            Destroy(chunks.gameObject);
        spawnedChunks.Clear();


        startRoom = SpawnChunk(transform, startRoom); // стартовая комната

        spawnedChunks.Add(startRoom);

        // спавню комнаты главного пути
        for(int i=0;i< mainPathRoomsCount; i++)
        {
            int rndEnumb = Random.Range(0,spawnedChunks.Last().Exits.Length-1);
        }
    }





    //private void SpawnChunk()
    //{
    //    Chunk newChunk = Instantiate(ChunkPrefabs[Random.Range(0, ChunkPrefabs.Length - 1)], Vector3.zero, Quaternion.identity);

    //    foreach (var exit in newChunk.Exits)
    //    {
    //        SpawnAllRooms(exit);
    //    }
    //    spawnedChunks.Add(newChunk);
    //}
    //[SerializeField]
    //int maxRooms = 10;

    //void SpawnAllRooms(Transform exit)
    //{
    //    if (spawnedChunks.Count >= maxRooms) return;

    //    Chunk rndChunk = TransitionPrefabs[Random.Range(0, ChunkPrefabs.Length - 1)];

    //    Chunk tr = Instantiate(rndChunk, exit.position + rndChunk.Enter.position, exit.rotation);
    //    tr.transform.position = exit.position + tr.Enter.localPosition;
    //    spawnedTransition.Add(tr);

    //    rndChunk = ChunkPrefabs[Random.Range(0, ChunkPrefabs.Length - 1)];
    //    Chunk room = Instantiate(rndChunk, tr.Exits[0].position + rndChunk.Enter.position, tr.Exits[0].rotation);
    //    //room.transform.position = tr.Exits[0].position + room.Enter.localPosition;
    //    spawnedChunks.Add(room);

    //    foreach (Transform _exit in room.Exits)
    //    {
    //        SpawnAllRooms(_exit);
    //    }
    //}


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



  


}
