using UnityEngine;

public static class GeneratorWorld
{
    public static BlockType[,,] WorldGenerator(float x0ffset, float z0ffset)
    {
        var result = new BlockType[ChunkRenderer.ChunkWidth, ChunkRenderer.ChunkHeight, ChunkRenderer.ChunkWidth]; 

        for (int x = 0; x < ChunkRenderer.ChunkWidth; x++)
        {
            for (int z = 0; z < ChunkRenderer.ChunkWidth; z++)
            {
                float height = Mathf.PerlinNoise((x/8f + x0ffset) * .2f, (z/8f + z0ffset) * .2f) * 3 + 15;

                for (int y = 0; y < height; y++)
                {
                    result[x, y, z] = BlockType.Grass;
                }
            }
        }
        return result;
    }
}
