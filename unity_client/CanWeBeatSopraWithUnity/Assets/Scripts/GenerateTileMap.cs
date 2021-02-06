using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateTileMap : MonoBehaviour
{
    public int xExtend;
    public int yExtend;

    public RuleTile tileBuilding;
    public RuleTile tileBase;
    public Tilemap mapBuildings;
    public Tilemap mapBase;

    private int i = 0;

    void Start()
    {
        for (int x = -xExtend; x < xExtend; x++)
        {
            for(int y = -yExtend; y < yExtend; y++)
            {
                mapBase.SetTile(new Vector3Int(x, y, 0), tileBase);
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mapBuildings.ClearAllTiles();
            for(int x = 0; x < i; x++)
            {
                for (int y = 0; y < i; y++)
                {
                    mapBuildings.SetTile(new Vector3Int(x, y, 0), tileBuilding);
                }
            }
            i++;
        }
    }
}
