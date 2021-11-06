using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkPlacer : MonoBehaviour
{
    public Transform Player;
    public Chunk[] ChunkPrefabs;
    public Chunk[] TransitionPrefabs;
    public Chunk[] endsList;
    private List<Chunk> spawnedChunks = new List<Chunk>();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnChunk () 
    {
        Chunk newChunk = Instantiate( ChunkPrefabs[Random.Range(0, ChunkPrefabs.Length)]);

        spawnedChunks.Add(newChunk);
    }
}
