using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TileMapScript : MonoBehaviour
{
    public int xExtend;
    public int yExtend;

    public RuleTile baseTile;
    public RuleTile highlightTile;

    public RuleTile rockTile;
    public RuleTile grassTile;

    public Tilemap mapBase;
    public Tilemap mapHighlight;

    public Text resizeTextfieldX;
    public Text resizeTextfieldY;

    private CameraScript cameraScript;

    private Vector3Int oldHighlightPos;

    private char[,] mapData;

    private char curPainter = 'g';
    private char paintGrass = 'g';
    private char paintRock = 'r';
    private char paintEmpty = 'e';

    private bool isPainting = false;

    // Start is called before the first frame update
    void Start()
    {
        this.mapData = new char[xExtend, yExtend];

        var len = mapData.GetLength(0);
        var sub = mapData.GetLength(1);
        for (int x = 0; x < len; x++)
        {
            for (int y = 0; y < sub; y++)
            {
                mapData[x, y] = paintEmpty;
            }
        }

        PaintMapData(mapData, mapBase);
        cameraScript = Camera.main.GetComponent<CameraScript>();
        cameraScript.SetCameraToMapMiddle(xExtend, yExtend);
    }


    private void PaintMapData(char[,] mData, Tilemap map)
    {
        map.ClearAllTiles();
        var len = mData.GetLength(0);
        var sub = mData.GetLength(1);
        for (int x = 0; x < len; x++)
        {
            for (int y = 0; y < sub; y++)
            {
                switch (mData[x, y])
                {
                    case 'g':
                        PaintTile(new Vector3Int(x, y, 0), grassTile, mapBase);
                        break;
                    case 'r':
                        PaintTile(new Vector3Int(x, y, 0), rockTile, mapBase);
                        break;
                    default:
                        PaintTile(new Vector3Int(x, y, 0), baseTile, mapBase);
                        break;
                }
            }
        }
    }

    public void ResizeFromUiButton()
    {
        var x = int.Parse(resizeTextfieldX.text);
        var y = int.Parse(resizeTextfieldY.text);

        ResizeTo(x, y);
    }

    private void ResizeTo(int x, int y)
    {
        mapData = ResizeMapData( x,y, mapData);
        PaintMapData(mapData, mapBase);
        cameraScript.SetCameraToMapMiddle(x, y);
    }

    private char[,] ResizeMapData(int xSize, int ySize, char[,] mapData)
    {
        var oldLen = mapData.GetLength(0);
        var oldSubLen = mapData.GetLength(1);
        var mapDataNew = new char[xSize, ySize];
        for(int x = 0; x < xSize; x++){
            for(int y = 0; y < ySize; y++)
            {
                if(x < oldLen && y < oldSubLen)
                {
                    mapDataNew[x, y] = mapData[x, y];
                }
                else
                {
                    mapDataNew[x, y] = paintEmpty;
                }
            }
        }
        return mapDataNew;
    }

    // Update is called once per frame
    void Update()
    {

        DoSwitchPainter();
        DoHighlight();

        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log(dumpToJson());
        }
    }

    private string dumpToJson()
    {
        JsonMap map = new JsonMap();
        map.name = "Test name";
        map.map = mapData;
        return JsonConvert.SerializeObject(map);
    }

    public void SetPainterFromUI(int painter)
    {
        
        curPainter = painter == 1 ? paintGrass : paintRock;
        
    }

    private void DoSwitchPainter()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Switch to grass painter");
            curPainter = paintGrass;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            curPainter = paintRock;
            Debug.Log("Switch to rock painter");
        }
    }

    private void DoHighlight()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Start painting");
            isPainting = true;
        }

        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int selectedTilePos = mapBase.WorldToCell(point);
        if (oldHighlightPos != null)
        {
            PaintTile(oldHighlightPos, null, mapHighlight);
        }
        if(selectedTilePos.x >= 0 && selectedTilePos.x < mapData.GetLength(0) && selectedTilePos.y >= 0 && selectedTilePos.y < mapData.GetLength(1))
        {
            PaintTile(selectedTilePos, highlightTile, mapHighlight);
            oldHighlightPos = selectedTilePos;

            if (isPainting)
            {
                Debug.Log("Painting...");
                PaintTile(selectedTilePos, GetTileFromPainter(), mapBase);
                mapData[selectedTilePos.x, selectedTilePos.y] = curPainter;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Stop painting");
            isPainting = false;
        }

    }


    private void PaintTile(Vector3Int posToPaint, RuleTile tileToPaint, Tilemap map)
    {
        map.SetTile(posToPaint, tileToPaint);
    }

    private RuleTile GetTileFromPainter()
    {
        if (curPainter.Equals(paintGrass))
        {
            return grassTile;
        }
        else if (curPainter.Equals(paintRock))
        {
            return rockTile;
        }
        return null;
    }
}
