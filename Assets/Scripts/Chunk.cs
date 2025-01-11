using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TileData
{
    public TileBase Tile { get; private set; }
    public Vector3Int Position { get; private set; }
    public TileData(TileBase tile, Vector3Int position)
    {
        Tile = tile;
        Position = position;
    }
}
public class Chunk
{
    private Tilemap terrainMap;
    private Tilemap environmentMap;
    private TileBase grassTile;
    private TileBase dirtTile;
    private TileBase environmentTileBase;
    private List<TileData> environmentTiles;
    private static int length = 20;
    private static int height = 5;
    private static float elementsDensity = 0.2f;
    private static Vector3Int initialPosition = new Vector3Int(1, 0, 0);
    private int id;
    private int koef = 0;

    public Chunk() : this (0, null, null, null, null, null) { }
    public Chunk(int id, Tilemap terrainMap, Tilemap environmentMap, TileBase grassTile, TileBase dirtTile, TileBase environmentTileBase)
    {
        environmentTiles = new List<TileData>();

        this.id = id;
        this.terrainMap = terrainMap;
        this.grassTile = grassTile;
        this.dirtTile = dirtTile;
        this.environmentMap = environmentMap;
        this.environmentTileBase = environmentTileBase;

        if (id < 0)
            koef = -1;
        else if(id > 0)
            koef = 1;

        GenerateEnvironment();
    }
    public void ShowChunk()
    {
        PrintChunk(grassTile, dirtTile);
    }
    public void HideChunk()
    {
        PrintChunk(null, null);
    }
    private void GenerateEnvironment()
    {
        int numberOfElements = Random.Range(0, (int)(length * elementsDensity));
        HashSet<Vector3Int> positionsOfElements = new HashSet<Vector3Int>();
        Vector3Int chunkPosition = initialPosition + new Vector3Int(length * id - (length - 1) * koef, 0, 0);

        for (int i = 0; i < numberOfElements; i++)
        {
            Vector3Int elementPosition = new Vector3Int(Random.Range(chunkPosition.x, chunkPosition.x + (length - 1) * koef), chunkPosition.y + 1);

            while(positionsOfElements.Contains(elementPosition))
                elementPosition = new Vector3Int(Random.Range(chunkPosition.x, chunkPosition.x + (length - 1) * koef), chunkPosition.y + 1);

            positionsOfElements.Add(elementPosition);
            TileData tile = new TileData(environmentTileBase, elementPosition);
            environmentTiles.Add(tile);
        }

        positionsOfElements.Clear();
    }
    public static int GetChunkIdByPosition(Vector3Int position)
    {
        int id = position.x / length;

        if (position.x > 0 && position.x % length != 0)
            ++id;
        else if (position.x < 0 && position.x % length != 0)
            --id;

        return id;
    }
    private void PrintChunk(TileBase upper, TileBase lower)
    {
        for (int i = 0; i < length; i++)
        {
            int x = length * id - (length - i) * koef;
            terrainMap.SetTile(initialPosition * koef + new Vector3Int(x, 0, 0), upper);
        }

        for (int y = -height; y < 0; y++)
        {
            for (int i = 0; i < length; i++)
            {
                int x = length * id - (length - i) * koef;
                terrainMap.SetTile(initialPosition * koef + new Vector3Int(x, y, 0), lower);
            }
        }

        foreach(TileData tile in environmentTiles)
        {
            environmentMap.SetTile(tile.Position, (upper == null) ? null : tile.Tile);
        }
    }
}
