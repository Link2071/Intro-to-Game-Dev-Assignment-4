using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject level01;
    [SerializeField] private TileBase[] levelTiles;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TextAsset csv;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (level01 != null) Destroy(level01);

        string[,] csvData = readCsv(csv.text);
        placeTiles(csvData);

        Tilemap quadrant2 = Instantiate(tilemap, new Vector3(0, 0, 0), Quaternion.identity, transform);
        Tilemap quadrant3 = Instantiate(tilemap, new Vector3(0, 0, 0), Quaternion.identity, transform);
        Tilemap quadrant4 = Instantiate(tilemap, new Vector3(0, 0, 0), Quaternion.identity, transform);
        quadrant2.transform.localScale = new Vector3( -1, 1, 1);
        quadrant3.transform.localScale = new Vector3( 1, -1, 1);
        quadrant4.transform.localScale = new Vector3( -1, -1, 1);
        for (int i = 0; i < csvData.GetLength(1); i++)
        {
            quadrant3.SetTile(new Vector3Int(i, -csvData.GetLength(0) + 1, 0), null);
            quadrant4.SetTile(new Vector3Int(i, -csvData.GetLength(0) + 1, 0), null);
        }
        quadrant2.transform.localPosition = new Vector3((csvData.GetLength(1)) * 2, 0, 0) ;
        quadrant3.transform.localPosition = new Vector3(0, (-csvData.GetLength(0) * 2)  + 3, 0);
        quadrant4.transform.localPosition = new Vector3((csvData.GetLength(1)) * 2, (-csvData.GetLength(0) * 2) + 3, 0);
    }

    private void placeTiles(string[,] grid)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                switch (grid[i, j])
                {
                    case "0":
                        placeTile(grid, j, -i, 0);
                        break;
                    case "1":
                        placeTile(grid, j, -i, 1);
                        break;
                    case "2":
                        placeTile(grid, j, -i, 2);
                        break;
                    case "3":
                        placeTile(grid, j, -i, 3);
                        break;
                    case "4":
                        placeTile(grid, j, -i, 4);
                        break;
                    case "5":
                        placeTile(grid, j, -i, 5);
                        break;
                    case "6":
                        placeTile(grid, j, -i, 6);
                        break;
                    case "7":
                        placeTile(grid, j, -i, 7);
                        break;
                    case "8":
                        placeTile(grid, j, -i, 8);
                        break;
                }
            }
        }
    }

    private void placeTile(string[,] grid, int xPos, int yPos, int tileIndex)
    {
        if (tileIndex == 0)
        {
            tilemap.SetTile(new Vector3Int(xPos, yPos, 0), null);
            return;
        } 
        tilemap.SetTile(new Vector3Int(xPos, yPos, 0), levelTiles[tileIndex - 1]);
        Matrix4x4 rotationMatrix = getRotation(grid, xPos, yPos);
        tilemap.SetTransformMatrix(new Vector3Int(xPos, yPos, 0), rotationMatrix);
    }

    private Matrix4x4 getRotation(string[,] grid, int xPos, int yPos)
    {
        switch (grid[-yPos, xPos])
        {
            case "0":
                return Matrix4x4.Rotate(Quaternion.identity);
            case "1":
            case "3":
            case "7":
                return calculateRotationCorners(grid, xPos, yPos);
            case "2":
            case "4":
                return calculateRotationEdges(grid, xPos, yPos);
        }
        return Matrix4x4.Rotate(Quaternion.identity);
    }

    private Matrix4x4 calculateRotationEdges(String[,] grid, int xPos, int yPos)
    {
        int gridY = -yPos;

        int xDimensionSize = grid.GetLength(1);
        int yDimensionSize = grid.GetLength(0);

        string neighbourRight = (xPos + 1 < xDimensionSize) ? grid[gridY, xPos + 1] : "0";
        string neighbourUp = (gridY - 1 >= 0) ? grid[gridY - 1, xPos] : "0";
        string neighbourLeft = (xPos - 1 >= 0) ? grid[gridY, xPos - 1] : "0";
        string neighbourDown = (gridY + 1 < yDimensionSize) ? grid[gridY + 1, xPos] : "0";

        string[] pelletTiles = {"5", "6"};
        string[] notValidWalls = {"0", "5", "6"};

        
        if (!notValidWalls.Contains(neighbourUp) && !notValidWalls.Contains(neighbourDown))
        {
            if (!pelletTiles.Contains(neighbourLeft))
            {
                return Matrix4x4.Rotate(Quaternion.Euler(0, 0, 90));
            }
            return Matrix4x4.Rotate(Quaternion.Euler(0, 0, 270));
        }
        if (!notValidWalls.Contains(neighbourRight) && !notValidWalls.Contains(neighbourLeft))
        {
            if (!pelletTiles.Contains(neighbourUp))
            {
                return Matrix4x4.Rotate(Quaternion.identity);
            }
            return Matrix4x4.Rotate(Quaternion.Euler(0, 0, 180));
        }
        return Matrix4x4.Rotate(Quaternion.identity);
    }

    private Matrix4x4 calculateRotationCorners(string[,] grid, int xPos, int yPos)
    {
        //convert back to grid position
        int gridY = -yPos;

        int xDimensionSize = grid.GetLength(1);
        int yDimensionSize = grid.GetLength(0);

        string neighbourRight = (xPos + 1 < xDimensionSize) ? grid[gridY, xPos + 1] : "0";
        string neighbourUp = (gridY - 1 >= 0) ? grid[gridY - 1, xPos] : "0";
        string neighbourLeft = (xPos - 1 >= 0) ? grid[gridY, xPos - 1] : "0";
        string neighbourDown = (gridY + 1 < yDimensionSize) ? grid[gridY + 1, xPos] : "0";

        string[] notValidWalls = {"0", "5", "6"};
        
        if (!notValidWalls.Contains(neighbourRight))
        {
            if (!notValidWalls.Contains(neighbourDown))
            {
                return Matrix4x4.Rotate(Quaternion.Euler(0, 0, 0));
            }
            return Matrix4x4.Rotate(Quaternion.Euler(0, 0, 90));
        }
        if (!notValidWalls.Contains(neighbourLeft))
        {
            if (!notValidWalls.Contains(neighbourDown)) 
            {
                return Matrix4x4.Rotate(Quaternion.Euler(0, 0, 270));
            }
            return Matrix4x4.Rotate(Quaternion.Euler(0, 0, 180));
        }
        return Matrix4x4.Rotate(Quaternion.Euler(0, 0, 90));
    }

    private string[,] readCsv(string text)
    {
        text = text.Replace("\r\n", "\n");
        text = text.Replace("\r", "\n");

        string[] rows = text.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);
        
        int rowCount = rows.Length;
        int columnCount = rows[0].Split(',').Length;

        string[,] dataGrid = new string[rowCount, columnCount];

        for (int i = 0; i < rowCount; i++)
        {
            string[] cells = rows[i].Split(',');
            for (int j = 0; j < columnCount; j++)
            {
                dataGrid[i, j] = cells[j].Trim();
            }
        }
        return dataGrid;
    }
}
