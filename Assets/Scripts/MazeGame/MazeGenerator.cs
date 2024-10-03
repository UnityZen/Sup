using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int mazeWidth = 21;
    public int mazeHeight = 21;
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject exitPrefab;
    public Transform player;

    public bool hasRooms = false;  // Flag to determine if rooms should be generated
    public bool hasHoles = false;  // Flag to determine if holes should be generated

    private int[,] maze;

    void Start()
    {
        maze = GenerateMaze();
        BuildMaze();
        PlacePlayer();
        PlaceExit();
    }

    int[,] GenerateMaze()
    {
        // Initialize the maze array (1 = wall, 0 = path)
        int[,] maze = new int[mazeWidth, mazeHeight];

        // Fill with walls
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                maze[x, y] = 1;
            }
        }

        // Optionally generate rooms before carving paths
        if (hasRooms)
        {
            GenerateRooms(maze);
        }

        // Starting point
        int startX = 1;
        int startY = 1;

        // Use DFS algorithm to carve out paths
        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        Vector2Int currentCell = new Vector2Int(startX, startY);
        stack.Push(currentCell);
        maze[startX, startY] = 0;  // Mark as path

        while (stack.Count > 0)
        {
            List<Vector2Int> neighbors = GetUnvisitedNeighbors(currentCell, maze);
            if (neighbors.Count > 0)
            {
                // Choose a random neighbor
                Vector2Int chosen = neighbors[Random.Range(0, neighbors.Count)];

                // Remove the wall between current and chosen
                maze[(currentCell.x + chosen.x) / 2, (currentCell.y + chosen.y) / 2] = 0;

                // Move to the chosen cell
                currentCell = chosen;
                maze[currentCell.x, currentCell.y] = 0;  // Mark as path
                stack.Push(currentCell);
            }
            else
            {
                // Backtrack
                currentCell = stack.Pop();
            }
        }

        // Optionally add holes in walls after maze is generated
        if (hasHoles)
        {
            GenerateHoles(maze);
        }

        return maze;
    }

    List<Vector2Int> GetUnvisitedNeighbors(Vector2Int cell, int[,] maze)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        // Check in four possible directions (up, down, left, right)
        if (cell.x > 2 && maze[cell.x - 2, cell.y] == 1)
            neighbors.Add(new Vector2Int(cell.x - 2, cell.y));
        if (cell.x < mazeWidth - 3 && maze[cell.x + 2, cell.y] == 1)
            neighbors.Add(new Vector2Int(cell.x + 2, cell.y));
        if (cell.y > 2 && maze[cell.x, cell.y - 2] == 1)
            neighbors.Add(new Vector2Int(cell.x, cell.y - 2));
        if (cell.y < mazeHeight - 3 && maze[cell.x, cell.y + 2] == 1)
            neighbors.Add(new Vector2Int(cell.x, cell.y + 2));

        return neighbors;
    }

    void GenerateRooms(int[,] maze)
    {
        // Attempt to create 2-3 rooms randomly
        int numRooms = Random.Range(2, 4); // Number of rooms (2 or 3)
        int[] roomSizes = { 4, 6 }; // Room sizes (4x4 or 6x6)

        for (int i = 0; i < numRooms; i++)
        {
            int roomSize = roomSizes[Random.Range(0, roomSizes.Length)]; // Random room size
            int roomStartX = Random.Range(1, mazeWidth - roomSize - 1);
            int roomStartY = Random.Range(1, mazeHeight - roomSize - 1);

            // Ensure that room coordinates are odd to match the grid carving pattern
            roomStartX = roomStartX % 2 == 0 ? roomStartX + 1 : roomStartX;
            roomStartY = roomStartY % 2 == 0 ? roomStartY + 1 : roomStartY;

            // Carve out a room
            for (int x = roomStartX; x < roomStartX + roomSize && x < mazeWidth - 1; x++)
            {
                for (int y = roomStartY; y < roomStartY + roomSize && y < mazeHeight - 1; y++)
                {
                    maze[x, y] = 0; // Mark as path (part of the room)
                }
            }
        }
    }

    void GenerateHoles(int[,] maze)
    {
        // Determine the number of holes based on maze size
        int numHoles = (mazeWidth * mazeHeight) / (21 * 21) * 7;  // For 21x21 it's 7 holes, adjust proportionally

        int holesCreated = 0;
        while (holesCreated < numHoles)
        {
            int holeX = Random.Range(1, mazeWidth - 1);  // Random X position (avoid borders)
            int holeY = Random.Range(1, mazeHeight - 1); // Random Y position (avoid borders)

            // Only replace walls with holes (value 1) and avoid outer walls
            if (maze[holeX, holeY] == 1)
            {
                maze[holeX, holeY] = 0;  // Turn this wall into a hole (path)
                holesCreated++;
            }
        }
    }

    void BuildMaze()
    {
        // Build the maze in the scene using prefabs
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                Vector3 pos = new Vector3(x, y, 0);
                if (maze[x, y] == 1)
                {
                    Instantiate(wallPrefab, pos, Quaternion.identity);
                }
                else
                {
                    Instantiate(floorPrefab, pos, Quaternion.identity);
                }
            }
        }
    }

    void PlacePlayer()
    {
        // Place the player in the center of the maze
        player.position = new Vector3(1, 1, 0);
    }

    void PlaceExit()
    {
        // Find an exit far from the start (bottom-right corner)
        Instantiate(exitPrefab, new Vector3(mazeWidth - 2, mazeHeight - 2, 0), Quaternion.identity);
    }
}
