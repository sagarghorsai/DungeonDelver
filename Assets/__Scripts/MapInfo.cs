using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
public class MapInfo : MonoBehaviour
{
    static public int W { get; private set; }
    static public int H { get; private set; }
    static public int[,] MAP { get; private set; }
    static public Vector3 OFFSET = new Vector3(0.5f, 0.5f, 0);
    static public string COLLISIONS { get; private set; }
    static public string GRAP_TILES { get; private set; }

    [Header("Inscribed")]
    static public  TextAsset delverLevel;
    public TextAsset delverCollisions;
    public TextAsset delverGrapTiles;

    void Start()
    {

        LoadMap();

        COLLISIONS = Utils.RemoveLineEndings(delverCollisions.text);
        Debug.Log("COLLISIONS contains " + COLLISIONS.Length
        + " chars");

        GRAP_TILES =Utils.RemoveLineEndings(delverGrapTiles.text);
        Debug.Log("GRAP_TILES contains " + GRAP_TILES.Length
       + " chars");

    }


    void LoadMap()
    {
        string[] lines = delverLevel.text.Split('\n');
        H = lines.Length;
        string[] tileNums = lines[0].Trim().Split(' ');
        W = tileNums.Length;

        MAP = new int[W, H];
        for (int j = 0; j < H; j++)
        {
            tileNums = lines[j].Trim().Split(' ');
            for (int i = 0; i < W; i++)
            {
                if (tileNums[i] == "..")
                {
                    MAP[i, j] = 0;
                }
                else
                {
                    MAP[i, j] = int.Parse(tileNums[i],
                   NumberStyles.HexNumber);
                }
            }
        }

        TileSwapManager.SWAP_TILES(MAP);
        Debug.Log("Map size: " + W + " wide by " + H + "high");
    }

    public static BoundsInt GET_MAP_BOUNDS()
    {
        BoundsInt bounds = new BoundsInt(0, 0, 0, W, H, 1);
        return bounds;
    }
    public static int GET_MAP_AT_VECTOR2(Vector2 pos)
    {
        Vector2Int posInt = Vector2Int.FloorToInt(pos);
        return MAP[posInt.x, posInt.y];
    }
    public static bool UNSAFE_TILE_AT_VECTOR2(Vector2 pos)
    { 
        int tileNum = GET_MAP_AT_VECTOR2(pos);
        return (GRAP_TILES[tileNum] == 'U');
    }
}
