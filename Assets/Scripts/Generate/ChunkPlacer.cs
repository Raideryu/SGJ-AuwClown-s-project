using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
public class ChunkPlacer : MonoBehaviour
{
    [SerializeField] Chunk startRoomPrefab;
    [SerializeField] Chunk finishRoomPrefab;
    [SerializeField] Chunk[] ChunkPrefabs;
    [SerializeField] Chunk[] TransitionPrefabs;

    [SerializeField] int mainPathRoomsCount = 5;
    [SerializeField] int tupikPathRoomsCount = 2;

    [SerializeField] GameObject playerPrefab;

    private List<Chunk> spawnedChunks = new List<Chunk>();
    //private List<Chunk> spawnedTransition = new List<Chunk>();

    private Chunk startRoom;
    private Chunk finishRoom;

    // ����� ������ ������
    private RespawnPoint playerSpawnPoint;
    private NavMeshSurface navMesh;

    private void Start()
    {
        navMesh = FindObjectOfType<NavMeshSurface>();
        GeneratePath(); 
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.S))
    //        GeneratePath();
    //}

    void GeneratePath()
    {
        DestroyChunks();


        startRoom = SpawnChunk(transform, startRoomPrefab); // ��������� �������
        startRoom.isRoom = true;
        spawnedChunks.Add(startRoom);

        // ���������� ����� ������ ������
        playerSpawnPoint = startRoom.GetComponentInChildren<RespawnPoint>();

        // ������ ������� �������� ����
        for(int i=0;i< mainPathRoomsCount; i++)
        {
            Chunk prevChunk = spawnedChunks.Last();
            int rndExitNumb = Random.Range(0, prevChunk.Exits.Length - 1); //������� ��������� �����

            // ������ ������� ��� �������� (���� prevChunk.isRoom==true)
            Chunk totalChank = SpawnChunk(prevChunk.Exits[rndExitNumb], GetRNDChunk(prevChunk.isRoom));
            // ������ ���� - ������� ��� �������
            totalChank.isRoom = !prevChunk.isRoom;
            // ������, ��� � ����� ������ ��� �������������
            prevChunk.ExitsInfo[rndExitNumb].alreadeGenerated = true; //����� �� �������� �.�. �� ������, � ����� ����...
            // �������� � ����� ������
            spawnedChunks.Add(totalChank);

        }
        finishRoom = SpawnChunk(transform, finishRoomPrefab); // ��������� �������
        finishRoom.isRoom = true;
        spawnedChunks.Add(finishRoom);
        // �������� ���� �����

        navMesh.BuildNavMesh();
        InstansePlayer();
    }

    void InstansePlayer()
    {
        if (playerPrefab)
            Instantiate(playerPrefab, playerSpawnPoint.transform.position, playerSpawnPoint.transform.rotation);
        else
            Debug.LogError("�� �������� ������ ������ �� ChunkPlacer");
    }

    // ��� ������ ������
    // int rndExitNumb = 0;
    ////if(prevChunk.Exits.Length =)
    // rndExitNumb = Random.Range(0, prevChunk.Exits.Length-1); //������� ��������� �����
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


    //List<Chunk> SpawnOneRoom(Transform exit)
    //{
    //    //if (spawnedChunks.Count >= maxRooms) return;

    //    Chunk rndChunk = TransitionPrefabs[Random.Range(0, ChunkPrefabs.Length - 1)];

    //    Chunk tr = Instantiate(rndChunk, exit.position + rndChunk.Enter.position, exit.rotation);
    //    tr.transform.position = exit.position + tr.Enter.localPosition;
    //    //spawnedTransition.Add(tr);

    //    rndChunk = ChunkPrefabs[Random.Range(0, ChunkPrefabs.Length - 1)];
    //    Chunk room = Instantiate(rndChunk, tr.Exits[0].position + rndChunk.Enter.position, tr.Exits[0].rotation);
    //    //room.transform.position = tr.Exits[0].position + room.Enter.localPosition;
    //    //spawnedChunks.Add(room);

    //    return new List<Chunk> { tr };
    //}

    Chunk SpawnChunk(Transform previosExit, Chunk chunkPref)
    {
        Chunk ch;
        //Chunk rndChunk = TransitionPrefabs[Random.Range(0, ChunkPrefabs.Length - 1)];
        if (chunkPref.Enter)
            ch = Instantiate(chunkPref, previosExit.position + chunkPref.Enter.position, previosExit.rotation);
        else
            ch = Instantiate(chunkPref, previosExit.position, previosExit.rotation);
        //ch.transform.position = previosExit.position + ch.Enter.localPosition;

        return ch;
    }



  


}
