using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class ChunkRenderer : MonoBehaviour
{

    public const int ChunkWidth = 16;
    public const int ChunkHeight = 128;
    public const float BlockScale = .5f;

    public ChunkData ChunkData;
    public GameWorld ParentWorld;

    private Mesh chunkMesh;

    private List<Vector3> verticies = new List<Vector3>();
    private List<Vector2> uvs = new List<Vector2>();
    private List<int> triangles = new List<int>();
   
    void Start()
    {
        chunkMesh = new Mesh();

        RegenerateMesh();

        GetComponent<MeshFilter>().mesh = chunkMesh;
    }

    private void RegenerateMesh()
    {
        verticies.Clear();
        uvs.Clear();
        triangles.Clear();

        for (int y = 0; y < ChunkHeight; y++)
        {
            for (int x = 0; x < ChunkWidth; x++)
            {
                for (int z = 0; z < ChunkWidth; z++)
                {
                    GenereteBlock(x, y, z);
                }
            }
        }

        chunkMesh.triangles = new int[0];
        chunkMesh.vertices = verticies.ToArray();
        chunkMesh.uv = uvs.ToArray();
        chunkMesh.triangles = triangles.ToArray();

        chunkMesh.Optimize();

        chunkMesh.RecalculateBounds();
        chunkMesh.RecalculateNormals();

        GetComponent<MeshCollider>().sharedMesh = chunkMesh;
    }

    public void SpawnBlock(Vector3Int blockPosition)
    {
        ChunkData.Block[blockPosition.x, blockPosition.y, blockPosition.z] = BlockType.Stone;
        RegenerateMesh();
    }
    public void DeleteBlock(Vector3Int blockPosition)
    {
        ChunkData.Block[blockPosition.x, blockPosition.y, blockPosition.z] = BlockType.Air;
        RegenerateMesh();
    }

    private void GenereteBlock(int x, int y, int z)
    {
        Vector3Int blockPosition = new Vector3Int(x, y, z);

        BlockType blockType = GetBlockAtPosition(blockPosition);

        if (blockType == BlockType.Air) return;

        if (GetBlockAtPosition(blockPosition + Vector3Int.right) == 0) 
        {
            GenereteRightSide(blockPosition);

            AddUvs(blockType);
        }

        if (GetBlockAtPosition(blockPosition + Vector3Int.left) == 0)
        {
            GenereteLeftSide(blockPosition);

            AddUvs(blockType);
        }
            
        if (GetBlockAtPosition(blockPosition + Vector3Int.forward) == 0)
        {

            GenereteFrontSide(blockPosition);

            AddUvs(blockType);
        }

        if (GetBlockAtPosition(blockPosition + Vector3Int.back) == 0)
        { 
            GenereteBackSide(blockPosition);

            AddUvs(blockType);
        }

        if (GetBlockAtPosition(blockPosition + Vector3Int.up) == 0) 
        {
            GenereteTopSide(blockPosition);

            AddUvs(blockType);
        }

        if (GetBlockAtPosition(blockPosition + Vector3Int.down) == 0)
        {
            GenereteBottomSide(blockPosition);

            AddUvs(blockType);
        }       
    }

    private BlockType GetBlockAtPosition(Vector3Int blockPosition)
    {
        if (blockPosition.x >= 0 && blockPosition.x < ChunkWidth &&
            blockPosition.y >= 0 && blockPosition.y < ChunkHeight &&
            blockPosition.z >= 0 && blockPosition.z < ChunkWidth)
        {
            return ChunkData.Block[blockPosition.x,
                                   blockPosition.y,
                                   blockPosition.z];
        }

        else
        {
            if (blockPosition.y < 0 || blockPosition.y >= ChunkHeight) return BlockType.Air;

            Vector2Int adjacentChunkPosition = ChunkData.ChunkPosistion;
            if (blockPosition.x < 0)
            {
                adjacentChunkPosition.x--;
                blockPosition.x += ChunkWidth;
            }

            else if (blockPosition.x >= ChunkWidth)
            {
                adjacentChunkPosition.x++;
                blockPosition.x -= ChunkWidth;
            }

            if (blockPosition.z < 0)
            {
                adjacentChunkPosition.y--;
                blockPosition.z += ChunkWidth;
            }

            else if (blockPosition.z >= ChunkWidth)
            {
                adjacentChunkPosition.y++;
                blockPosition.z -= ChunkWidth;
            }

            if (ParentWorld.ChungDatas.TryGetValue(adjacentChunkPosition, out ChunkData adjacentChunk))
            {
                return adjacentChunk.Block[blockPosition.x, blockPosition.y, blockPosition.z];
            }

            else
            {
                return BlockType.Air;
            }
        } 
    }
   
    private void GenereteRightSide(Vector3Int blockPosition)
    {
        verticies.Add((new Vector3(1, 0, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 1, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 0, 1) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 1, 1) + blockPosition) * BlockScale);

        AddLastVerticiesSquare();
    } 
    
    private void GenereteLeftSide(Vector3Int blockPosition) 
    {
        verticies.Add((new Vector3(0, 0, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(0, 0, 1) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(0, 1, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(0, 1, 1) + blockPosition) * BlockScale);

        AddLastVerticiesSquare();
    }

    private void GenereteFrontSide(Vector3Int blockPosition)
    {
        verticies.Add((new Vector3(0, 0, 1) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 0, 1) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(0, 1, 1) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 1, 1) + blockPosition) * BlockScale);

        AddLastVerticiesSquare();
    }

    private void GenereteBackSide(Vector3Int blockPosition)
    {
        verticies.Add((new Vector3(0, 0, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(0, 1, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 0, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 1, 0) + blockPosition) * BlockScale);

        AddLastVerticiesSquare();
    }

    private void GenereteTopSide(Vector3Int blockPosition)
    {
        verticies.Add((new Vector3(0, 1, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(0, 1, 1) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 1, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 1, 1) + blockPosition) * BlockScale);

        AddLastVerticiesSquare();
    }

    private void GenereteBottomSide(Vector3Int blockPosition)
    {
        verticies.Add((new Vector3(0, 0, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 0, 0) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(0, 0, 1) + blockPosition) * BlockScale);
        verticies.Add((new Vector3(1, 0, 1) + blockPosition) * BlockScale);

        AddLastVerticiesSquare();
    }

    private void AddLastVerticiesSquare()
    {
        triangles.Add(verticies.Count - 4);
        triangles.Add(verticies.Count - 3);
        triangles.Add(verticies.Count - 2);

        triangles.Add(verticies.Count - 3);
        triangles.Add(verticies.Count - 1);
        triangles.Add(verticies.Count - 2);
    }
    
    private void AddUvs(BlockType blockType)
    {
        Vector2 uv;

        if (blockType == BlockType.Grass)
        {
            uv = new Vector2(5f / 40, 13f / 40);
        }

        else if (blockType == BlockType.Stone)
        {
            uv = new Vector2(21f / 40, 13f / 40);

        }

        else
        {
            uv = new Vector2(0, 0);
        }

        for (int i = 0; i < 4; i++) 
        {
            uvs.Add(uv);
        }

    }

}
