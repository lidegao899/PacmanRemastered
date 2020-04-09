using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public class Tile
    {
        public int X { get; set; }

        public int Y { get; set; }

        public bool Occupied { get; set; }

        public int AdjacentCount { get; set; }

        public bool IsIntersection { get; set; }

        public Tile up, right, left, down;


        public Tile(int x, int y)
        {
            X = x;
            Y = y;
            Occupied = false;
            up = right = left = down = null;
        }
    }

    private int gridWidth = 28, gridHigh = 31;

    public List<Tile> tiles = new List<Tile>();

    private void Awake()
    {
        ReadTileMap();
    }

    private void ReadTileMap()
    {
        string data = @"0000000000000000000000000000
0111111111111001111111111110
0100001000001001000001000010
0100001000001111000001000010
0100001000001001000001000010
0111111111111001111111111110
0100001001000000001001000010
0100001001000000001001000010
0111111001111001111001111110
0001001000001001000001001000
0001001000001001000001001000
0111001111111111111111001110
0100001001000000001001000010
0100001001000000001001000010
0111111001000000001001111110
0100001001000000001001000010
0100001001000000001001000010
0111001001111111111001001110
0001001001000000001001001000
0001001001000000001001001000
0111111111111111111111111110
0100001000001001000001000010
0100001000001001000001000010
0111001111111001111111001110
0001001001000000001001001000
0001001001000000001001001000
0111111001111001111001111110
0100001000001001000001000010
0100001000001001000001000010
0111111111111111111111111110
0000000000000000000000000000";

        // the left bottom corner is the origin point of unity, so the top left corner is 
        int x = 1, y = 31;
        using (StringReader stringReader = new StringReader(data))
        {
            string line;
            while ((line = stringReader.ReadLine()) != null)
            {
                x = 1;
                for (int i = 0; i < line.Length; i++)
                {
                    Tile newTile = new Tile(x, y);
                    if (line[i].Equals('1'))
                    {
                        if (i != 0 && line[i - 1].Equals('1'))
                        {
                            newTile.left = tiles[tiles.Count - 1];
                            tiles[tiles.Count - 1].right = newTile;

                            newTile.AdjacentCount++;
                            tiles[tiles.Count - 1].AdjacentCount++;
                        }
                    }
                    else
                    {
                        newTile.Occupied = true;
                    }

                    int upNeighborIndex = tiles.Count - gridWidth;
                    if (y < 30 && !newTile.Occupied && !tiles[upNeighborIndex].Occupied)
                    {
                        tiles[upNeighborIndex].down = newTile;
                        newTile.up = tiles[upNeighborIndex];

                        tiles[upNeighborIndex].AdjacentCount++;
                        newTile.AdjacentCount++;
                    }

                    tiles.Add(newTile);
                    x++;
                }
                y--;
            }

            foreach (Tile tile in tiles)
            {
                tile.IsIntersection = tile.AdjacentCount > 2;
            }
        }
    }

    private int Index(int x, int y)
    {
        x = x < 1 ? 1 : x;
        x = x > gridWidth ? gridWidth : x;

        y = y < 1 ? 1 : y;
        y = y > gridHigh ? gridHigh : y;
        return (gridHigh - y) * gridWidth + x - 1;
    }

    public Tile GetTileByPos(Vector3 pos)
    {
        return tiles[Index((int)(pos.x + .499f), (int)(pos.y + .499f))];
    }

    public float GetTileDistance(Tile tile1, Tile tile2)
    {
        return Mathf.Sqrt(Mathf.Pow(tile1.X - tile1.X, 2) + Mathf.Pow(tile1.Y + tile2.Y, 2));
    }
}