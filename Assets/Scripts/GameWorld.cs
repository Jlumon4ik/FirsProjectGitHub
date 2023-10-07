using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class GameWorld : MonoBehaviour
{
    private const int ViewRaduis = 5;

    public Dictionary<Vector2Int, ChunkData> ChungDatas = new Dictionary<Vector2Int, ChunkData>();
    public ChunkRenderer ChunkPefab;

    private Camera maineCamera;
    private Vector2Int currentPlayerChunk;

    private void Start()
    {
        maineCamera = Camera.main;

        Generate();
    }

    private void Generate()
    {
        for (int x = currentPlayerChunk.x - ViewRaduis; x < currentPlayerChunk.x + ViewRaduis; x++)
        {
            for (int y = currentPlayerChunk.y - ViewRaduis; y < currentPlayerChunk.y + ViewRaduis; y++)
            {
                Vector2Int chunkPosition = new Vector2Int(x, y);
                if (ChungDatas.ContainsKey(chunkPosition)) continue;
                 
                loadChunk(chunkPosition);
            }
        }
    }

    private void loadChunk(Vector2Int chunkPosition)
    {

        var chunkData = new ChunkData();

        float xPos = chunkPosition.x * ChunkRenderer.ChunkWidth * ChunkRenderer.BlockScale;
        float zPos = chunkPosition.y * ChunkRenderer.ChunkWidth * ChunkRenderer.BlockScale;

        chunkData.ChunkPosistion = chunkPosition;
        chunkData.Block = GeneratorWorld.WorldGenerator(xPos, zPos);
        ChungDatas.Add(chunkPosition, chunkData);

        var chunk = Instantiate(ChunkPefab, new Vector3(xPos, 0, zPos), Quaternion.identity, transform);
        chunk.ChunkData = chunkData;
        chunk.ParentWorld = this;

        chunkData.Renderer = chunk;
    }

    private void Update()
    {
        Vector3Int playerWorldPos = Vector3Int.FloorToInt(maineCamera.transform.position / ChunkRenderer.BlockScale);
        Vector2Int playerChunk = GetChunkContainingBlock(playerWorldPos);
        if (playerChunk != currentPlayerChunk)
        {
            currentPlayerChunk = playerChunk;
            Generate();
        }

        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            bool isDestroing = Input.GetMouseButtonDown(0);

            Ray ray = maineCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

            if (Physics.Raycast(ray, out var hitInfo))
            {
                Vector3 blockCenter;

                if (isDestroing)
                {
                    blockCenter = hitInfo.point - hitInfo.normal * ChunkRenderer.BlockScale / 2;
                }
                else
                {
                    blockCenter = hitInfo.point + hitInfo.normal * ChunkRenderer.BlockScale / 2;
                }

                Vector3Int blockWorldPos = Vector3Int.FloorToInt(blockCenter / ChunkRenderer.BlockScale);

                Vector2Int chunkPos = GetChunkContainingBlock(blockWorldPos);

                if (ChungDatas.TryGetValue(chunkPos, out ChunkData chunkData))
                {
                    Vector3Int chunk0rigin = new Vector3Int(chunkPos.x, 0, chunkPos.y) * ChunkRenderer.ChunkWidth;
                    if (isDestroing)
                    {
                        chunkData.Renderer.DeleteBlock(blockWorldPos - chunk0rigin);
                    }
                    else
                    {
                        chunkData.Renderer.SpawnBlock(blockWorldPos - chunk0rigin);
                    }
                }
            }
        }
    }

    public Vector2Int GetChunkContainingBlock(Vector3Int blockWorldPos)
    {
        return new Vector2Int (blockWorldPos.x / ChunkRenderer.ChunkWidth, blockWorldPos.z / ChunkRenderer.ChunkWidth);
    }
}