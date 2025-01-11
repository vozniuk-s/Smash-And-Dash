using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class WorldGeneration : MonoBehaviour
{
    [SerializeField] private int viewDistance = 10;
    [SerializeField] private Tilemap terrainMap;
    [SerializeField] private Tilemap environmentMap;
    [SerializeField] private TileBase grassTile;
    [SerializeField] private TileBase dirtTile;
    [SerializeField] private TileBase environmentTileBase;

    private const int MaximumVisibleChunks = 5;

    private Dictionary<int,Chunk> chunks = new Dictionary<int,Chunk>();
    private Queue<Chunk> visibleChunks = new Queue<Chunk>();

    private void Update()
    {
        Vector3Int playerCellPosition = terrainMap.WorldToCell(transform.position);

        for (int x = -viewDistance; x <= viewDistance; x++)
        {
            Vector3Int checkPosition = playerCellPosition + new Vector3Int(x, 0, 0);
            if (terrainMap.GetTile(checkPosition) == null)
            {
                int chunkId = Chunk.GetChunkIdByPosition(checkPosition);
                if (!chunks.ContainsKey(chunkId))
                {
                    CreateAndShowChunk(chunkId, checkPosition);
                }
                else
                {
                    ShowExistingChunk(chunkId);
                }
            }
        }
    }

    private void CreateAndShowChunk(int chunkId, Vector3Int position)
    {
        Chunk chunk = new Chunk(chunkId, terrainMap, environmentMap, grassTile, dirtTile, environmentTileBase);
        chunks.Add(chunkId, chunk);
        chunk.ShowChunk();
        visibleChunks.Enqueue(chunk);

        LimitVisibleChunks();
    }

    private void ShowExistingChunk(int chunkId)
    {
        Chunk chunk = chunks[chunkId];
        chunk.ShowChunk();
        visibleChunks.Enqueue(chunk);

        LimitVisibleChunks();
    }

    private void LimitVisibleChunks()
    {
        if (visibleChunks.Count > MaximumVisibleChunks)
        {
            Chunk chunkToHide = visibleChunks.Dequeue();
            chunkToHide.HideChunk();
        }
    }
}
