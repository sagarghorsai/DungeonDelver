using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileSwap
{
    public int fromTileNum;
    public GameObject swapPrefab;
    public GameObject guaranteedDrop;
    public int toTileNum = 1;
}

public class TileSwapManager : MonoBehaviour
{
    private static TileSwapManager S;
    private static Dictionary<int, TileSwap> TILE_SWAP_DICT;
    public List<TileSwap> tileSwapList;

    private void Awake()
    {
        S = this;
    }
    public static void SWAP_TILES(int[,] map)
    {
        if (TILE_SWAP_DICT == null) S.BuildTileSwapDict();

        int fromTileNum;
        TileSwap tSwap;
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                fromTileNum = map[i, j];

                if (TILE_SWAP_DICT.ContainsKey(fromTileNum
               ))
                {
                    tSwap = TILE_SWAP_DICT[fromTileNum];
                    map[i, j] = tSwap.toTileNum;


                    GameObject go = Instantiate<GameObject>(tSwap.swapPrefab);
                    ISwappable iSwap =
                   go.GetComponent<ISwappable>();
                    if (iSwap != null)
                    {
                        iSwap.Init(fromTileNum, i, j);
                        iSwap.guaranteedDrop =
                       tSwap.guaranteedDrop;
                    }
                    else
                    {
                        go.transform.position = new Vector3(
                       i, j, 0)
                        +
                       MapInfo.OFFSET;

                    }
                }
            }

        }
    }

    void BuildTileSwapDict()
    {
        TILE_SWAP_DICT = new Dictionary<int, TileSwap>();
        foreach (TileSwap swap in tileSwapList)
        {
            if (TILE_SWAP_DICT.ContainsKey(swap.fromTileNum
           ))
            {
                Debug.LogError("More than one TileSwap with a From # of "
                + swap.fromTileNum);
            }
            else
            {
                TILE_SWAP_DICT.Add(swap.fromTileNum, swap);
            }
        }
    }
}
