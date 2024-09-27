using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] crystalPrefabs; // ������� ����������
    public int gridWidth = 5; // ������ ����
    public int gridHeight = 5; // ������ ����
    private int[,] grid; // ������ ��� �������� ����� ����������
    private Crystal selectedCrystal; // ��������� ��������
    private Vector2Int selectedCrystalPos; // ������� ���������� ���������

    private void Start()
    {
        grid = new int[gridWidth, gridHeight];
        GenerateGrid(); // ��������� �����
        FillGrid(); // ���������� �����������
    }

    void GenerateGrid()
    {
        do
        {
            InitializeGrid();
        } while (HasMatches());
    }

    void InitializeGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid[x, y] = Random.Range(0, crystalPrefabs.Length); // ��������� ��� ���������
            }
        }
    }

    void FillGrid()
    {
        // ������� ��� ������ ��������� �� �������� ����
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // ��������� ������� ���� ������ �����������
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y] >= 0) // ��������, ��� ��� �� ��������� ��������
                {
                    Vector3 position = new Vector3(x, y, 0);
                    GameObject crystal = Instantiate(crystalPrefabs[grid[x, y]], position, Quaternion.identity);
                    crystal.GetComponent<Crystal>().SetCrystalType(grid[x, y]);
                    crystal.transform.SetParent(transform); // ������ �������� �������� GameManager
                }
            }
        }
    }

    public void HandleCrystalClick(Crystal clickedCrystal)
    {
        Debug.Log("Crystal clicked: " + clickedCrystal.GetCrystalType());

        if (selectedCrystal == null)
        {
            selectedCrystal = clickedCrystal; // ��������� ������ ��������
            selectedCrystalPos = new Vector2Int((int)clickedCrystal.transform.position.x, (int)clickedCrystal.transform.position.y);
            Debug.Log("Selected Crystal Position: " + selectedCrystalPos);
        }
        else
        {
            Vector2Int clickedCrystalPos = new Vector2Int((int)clickedCrystal.transform.position.x, (int)clickedCrystal.transform.position.y);
            if (IsAdjacent(selectedCrystalPos, clickedCrystalPos))
            {
                // �������� �������� ���������
                StartCoroutine(SwapCrystalsRoutine(selectedCrystal, clickedCrystal));
            }
            selectedCrystal = null; // ���������� �����
        }
    }

    private bool IsAdjacent(Vector2Int pos1, Vector2Int pos2)
    {
        // �������� �� ���������
        return (Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y)) == 1;
    }

    private IEnumerator SwapCrystalsRoutine(Crystal crystalA, Crystal crystalB)
    {
        Debug.Log("Swapping crystals");

        // ������ ������� ���� ���������� � �������
        int posA_X = (int)crystalA.transform.position.x;
        int posA_Y = (int)crystalA.transform.position.y;
        int posB_X = (int)crystalB.transform.position.x;
        int posB_Y = (int)crystalB.transform.position.y;

        // ����� ����� � �������
        int tempType = grid[posA_X, posA_Y];
        grid[posA_X, posA_Y] = grid[posB_X, posB_Y];
        grid[posB_X, posB_Y] = tempType;

        // ������ ������� ��������� � ������������
        Vector3 tempPosition = crystalA.transform.position;
        crystalA.transform.position = crystalB.transform.position;
        crystalB.transform.position = tempPosition;

        // ��������� ���� ����������
        crystalA.SetCrystalType(grid[posA_X, posA_Y]);
        crystalB.SetCrystalType(grid[posB_X, posB_Y]);

        // ���� 0.1 �������
        yield return new WaitForSeconds(0.1f);

        // �������� �� ����������
        if (CheckForMatches())
        {
            // ���� ���� ����������, ��������� �������
            yield return new WaitForSeconds(0.5f); // ���� ����� �����������
            while (FillEmptySpaces()) { } // ��������� ���� ���� �������
        }
        else
        {
            // ���� ���������� ���, ���������� ��������� �� �����
            grid[posA_X, posA_Y] = tempType; // ���������� ��� ���������
            grid[posB_X, posB_Y] = grid[posA_X, posA_Y];

            // ���������� ������� ����������
            crystalA.transform.position = tempPosition;
            crystalB.transform.position = new Vector3(posB_X, posB_Y, 0);

            // ��������� ���� ����������
            crystalA.SetCrystalType(grid[posA_X, posA_Y]);
            crystalB.SetCrystalType(grid[posB_X, posB_Y]);
        }
    }

    public bool CheckForMatches()
    {
        List<Vector2Int> matchesToRemove = new List<Vector2Int>();

        // �������� �� ���������� �� �����������
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth - 2; x++)
            {
                if (grid[x, y] != -1 && grid[x, y] == grid[x + 1, y] && grid[x, y] == grid[x + 2, y])
                {
                    matchesToRemove.Add(new Vector2Int(x, y));
                    matchesToRemove.Add(new Vector2Int(x + 1, y));
                    matchesToRemove.Add(new Vector2Int(x + 2, y));
                }
            }
        }

        // �������� �� ���������� �� ���������
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight - 2; y++)
            {
                if (grid[x, y] != -1 && grid[x, y] == grid[x, y + 1] && grid[x, y] == grid[x, y + 2])
                {
                    matchesToRemove.Add(new Vector2Int(x, y));
                    matchesToRemove.Add(new Vector2Int(x, y + 1));
                    matchesToRemove.Add(new Vector2Int(x, y + 2));
                }
            }
        }

        if (matchesToRemove.Count > 0)
        {
            foreach (var pos in matchesToRemove)
            {
                grid[pos.x, pos.y] = -1; // ������� ���������
            }
            return true; // ������� ����������
        }
        return false; // ���������� ���
    }

    private bool FillEmptySpaces()
    {
        bool hasFilledSpaces = false;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // ������ � ������� ���� � ����� ��������� �����
                if (grid[x, y] == -1) // ���� �������� ������
                {
                    hasFilledSpaces = true; // ��������, ��� ���� ����������� ������������
                                            // ����� ����
                    for (int k = y; k < gridHeight - 1; k++)
                    {
                        grid[x, k] = grid[x, k + 1]; // ����������� ���������� ����
                    }
                    // � ����� ������ ���� ������ ����� ��������
                    grid[x, gridHeight - 1] = Random.Range(0, crystalPrefabs.Length);
                }
            }
        }

        // ��������� ������������
        UpdateGridVisual();

        // ��������� ������� ������ ����� ���������� ������
        CheckForMatchesAfterFilling();

        return hasFilledSpaces; // ������� true, ���� ���� ����������� ������������
    }

    private void CheckForMatchesAfterFilling()
    {
        // ��������� ������� ������
        while (CheckForMatches())
        {
            // ���� ���� ����������, ��������� �������
            FillEmptySpaces(); // ��������� �������, ���� ���� ����������
        }
    }

    private void UpdateGridVisual()
    {
        // ������� ��� ������ ��������� �� �������� ����
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // ��������� ������� ���� ������ �����������
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y] >= 0 && grid[x, y] < crystalPrefabs.Length) // �������� ������
                {
                    Vector3 position = new Vector3(x, y, 0);
                    GameObject crystal = Instantiate(crystalPrefabs[grid[x, y]], position, Quaternion.identity);
                    crystal.GetComponent<Crystal>().SetCrystalType(grid[x, y]);
                    crystal.transform.SetParent(transform); // ������ �������� �������� GameManager
                }
            }
        }
    }

    private bool HasMatches()
    {
        return CheckForMatches();
    }
}
