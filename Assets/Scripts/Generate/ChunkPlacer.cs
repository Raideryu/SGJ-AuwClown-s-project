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
        DestroyChunks();


        startRoom = SpawnChunk(transform, startRoom); // ��������� �������
        startRoom.isRoom = true;
        spawnedChunks.Add(startRoom);

        // ������ ������� �������� ����
        for(int i=0;i< mainPathRoomsCount; i++)
        {
            Chunk prevChunk = spawnedChunks.Last();
            int rndExitNumb = Random.Range(0, prevChunk.Exits.Length-1); //������� ��������� �����
            
            // ������ ������� ��� �������� (���� prevChunk.isRoom==true)
            Chunk totalChank = SpawnChunk(prevChunk.Exits[rndExitNumb], GetRNDChunk(prevChunk.isRoom));
            // ������ ���� - ������� ��� �������
            totalChank.isRoom = !prevChunk.isRoom;
            // ������, ��� � ����� ������ ��� �������������
            prevChunk.ExitsInfo[i].alreadeGenerated = true; //����� �� �������� �.�. �� ������, � ����� ����...
            // �������� � ����� ������
            spawnedChunks.Add(totalChank);

        }
        spawnedChunks.Add(finishRoom);
        // �������� ���� �����
    }

    Chunk GetRNDChunk(bool prevIsRoom)
    {
        if (!prevIsRoom)
        {
            int rndN = Random.Range(0, ChunkPrefabs.Length - 1);
            return ChunkPrefabs[rndN];
        }
        else
        {
            int rndN = Random.Range(0, TransitionPrefabs.Length - 1);
            return TransitionPrefabs[rndN];
        }
    }

    void DestroyChunks()
    {
        foreach (Chunk chunks in spawnedChunks)
            Destroy(chunks.gameObject);
        spawnedChunks.Clear();
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
