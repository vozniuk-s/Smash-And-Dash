using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnviromentGeneration : MonoBehaviour
{
    [SerializeField] private TileBase tile;
    [SerializeField] private Tilemap _tilemap;

    private void Update()
    {
       /* if((int)transform.position.x % 2 == 0)
            _tilemap.SetTile(new Vector3Int((int)transform.position.x, (int)transform.position.y+1, 0), tiles);*/
    }
}
