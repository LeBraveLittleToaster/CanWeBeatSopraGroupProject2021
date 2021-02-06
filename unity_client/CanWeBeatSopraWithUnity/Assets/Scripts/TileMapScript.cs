using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapScript : MonoBehaviour
{
    public int xExtend;
    public int yExtend;

    public RuleTile baseTile;
    public RuleTile highlightTile;

    public Tilemap mapBase;
    public Tilemap mapHighlight;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = -xExtend; x < xExtend; x++)
        {
            for (int y = -yExtend; y < yExtend; y++)
            {
                mapBase.SetTile(new Vector3Int(x, y, 0), baseTile);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
