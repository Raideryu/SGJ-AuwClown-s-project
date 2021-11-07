using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
public class ChunkPlacer : MonoBehaviour
{
    [SerializeField] Chunk startRoomPrefab;
    [SerializeField] Chunk finishRoomPrefab;
    [SerializeField] Chunk[] BigRoomPrefabs;
    [SerializeField] Chunk[] blockedRoomPrefabs;
    [SerializeField] Chunk[] TransitionPrefabs;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject bossPrefab;


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
            int rndExitNumb = Random.Range(0, prevChunk.Exits.Length); //������� ��������� �����

            // ������ ������� ��� �������� (���� prevChunk.isRoom==true)
            Chunk totalChank = SpawnChunk(prevChunk.Exits[rndExitNumb], GetRNDChunk(prevChunk.isRoom));
            // ������ ���� - ������� ��� �������
            totalChank.isRoom = !prevChunk.isRoom;

            totalChank.SpawnEnemys(enemyPrefab);

            // ������, ��� � ����� ������ ��� �������������
            prevChunk.ExitsInfo[rndExitNumb].alreadeGenerated = true; //����� �� �������� �.�. �� ������, � ����� ����...
            // �������� � ����� ������
            spawnedChunks.Add(totalChank);

        }
        finishRoom = SpawnChunk(transform, finishRoomPrefab); // ��������� �������
        finishRoom.SpawnEnemys(bossPrefab);
        finishRoom.isRoom = true;
        spawnedChunks.Add(finishRoom);
        // �������� ���� �����

        List<Chunk> secondPath = new List<Chunk>();

        // ����� �����������
        foreach(Chunk chunk in spawnedChunks)
        {
            foreach(ExitInfo exitInfo in chunk.ExitsInfo)
            {
                if (!exitInfo.alreadeGenerated)
                {
                    //������� �������� � �������
                    Chunk tr = SpawnChunk(exitInfo.transform, GetRNDChunk(true));
                    Chunk room = SpawnChunk(tr.Exits[0], GetRNDBlockChunk());
                    exitInfo.alreadeGenerated = true;
                    secondPath.Add(tr);
                    secondPath.Add(room);

                }
            }
        }


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
            int rndN = Random.Range(0, BigRoomPrefabs.Length);
            
            return BigRoomPrefabs[rndN];
        }
        else
        {
            int rndN = Random.Range(0, TransitionPrefabs.Length);
            return TransitionPrefabs[rndN];
        }
    }

    Chunk GetRNDBlockChunk()
    {
        int rndN = Random.Range(0, blockedRoomPrefabs.Length);

        return blockedRoomPrefabs[rndN];
    }



    void DestroyChunks()
    {
        foreach (Chunk chunks in spawnedChunks)
            Destroy(chunks.gameObject);
        spawnedChunks.Clear();
    }

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
