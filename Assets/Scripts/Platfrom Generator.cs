using UnityEngine;
using UnityEngine.Tilemaps;

public class PlatfromGenerator : MonoBehaviour
{
    [SerializeField] private TileBase upperTile;
    [SerializeField] private TileBase bottomTile;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private int platformMaxLength = 4;
    [SerializeField] private int platformMinLength = 12;

    private void Update()
    {
        Collider2D collider = Physics2D.OverlapPoint(transform.position);

        if (collider == null)
        {
            BuildPlarform();
        }
    }

    private void BuildPlarform()
    {
        _tilemap.SetTile(new Vector3Int((int)transform.position.x, 0, 0), upperTile);
        _tilemap.SetTile(new Vector3Int((int)transform.position.x, -1, 0), bottomTile);
    }
}
