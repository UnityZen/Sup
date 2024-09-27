using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] crystalPrefabs; // Префабы кристаллов
    public int gridWidth = 5; // Ширина поля
    public int gridHeight = 5; // Высота поля
    private int[,] grid; // Массив для хранения типов кристаллов
    private Crystal selectedCrystal; // Выбранный кристалл
    private Vector2Int selectedCrystalPos; // Позиция выбранного кристалла

    private void Start()
    {
        grid = new int[gridWidth, gridHeight];
        GenerateGrid(); // Генерация сетки
        FillGrid(); // Заполнение кристаллами
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
                grid[x, y] = Random.Range(0, crystalPrefabs.Length); // Случайный тип кристалла
            }
        }
    }

    void FillGrid()
    {
        // Удаляем все старые кристаллы из игрового поля
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Заполняем игровое поле новыми кристаллами
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y] >= 0) // Проверка, что это не удаленный кристалл
                {
                    Vector3 position = new Vector3(x, y, 0);
                    GameObject crystal = Instantiate(crystalPrefabs[grid[x, y]], position, Quaternion.identity);
                    crystal.GetComponent<Crystal>().SetCrystalType(grid[x, y]);
                    crystal.transform.SetParent(transform); // Делаем дочерним объектом GameManager
                }
            }
        }
    }

    public void HandleCrystalClick(Crystal clickedCrystal)
    {
        Debug.Log("Crystal clicked: " + clickedCrystal.GetCrystalType());

        if (selectedCrystal == null)
        {
            selectedCrystal = clickedCrystal; // Сохраняем первый кристалл
            selectedCrystalPos = new Vector2Int((int)clickedCrystal.transform.position.x, (int)clickedCrystal.transform.position.y);
            Debug.Log("Selected Crystal Position: " + selectedCrystalPos);
        }
        else
        {
            Vector2Int clickedCrystalPos = new Vector2Int((int)clickedCrystal.transform.position.x, (int)clickedCrystal.transform.position.y);
            if (IsAdjacent(selectedCrystalPos, clickedCrystalPos))
            {
                // Пытаемся обменять кристаллы
                StartCoroutine(SwapCrystalsRoutine(selectedCrystal, clickedCrystal));
            }
            selectedCrystal = null; // Сбрасываем выбор
        }
    }

    private bool IsAdjacent(Vector2Int pos1, Vector2Int pos2)
    {
        // Проверка на соседство
        return (Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y)) == 1;
    }

    private IEnumerator SwapCrystalsRoutine(Crystal crystalA, Crystal crystalB)
    {
        Debug.Log("Swapping crystals");

        // Меняем местами типы кристаллов в массиве
        int posA_X = (int)crystalA.transform.position.x;
        int posA_Y = (int)crystalA.transform.position.y;
        int posB_X = (int)crystalB.transform.position.x;
        int posB_Y = (int)crystalB.transform.position.y;

        // Обмен типов в массиве
        int tempType = grid[posA_X, posA_Y];
        grid[posA_X, posA_Y] = grid[posB_X, posB_Y];
        grid[posB_X, posB_Y] = tempType;

        // Меняем местами кристаллы в пространстве
        Vector3 tempPosition = crystalA.transform.position;
        crystalA.transform.position = crystalB.transform.position;
        crystalB.transform.position = tempPosition;

        // Обновляем типы кристаллов
        crystalA.SetCrystalType(grid[posA_X, posA_Y]);
        crystalB.SetCrystalType(grid[posB_X, posB_Y]);

        // Ждем 0.1 секунды
        yield return new WaitForSeconds(0.1f);

        // Проверка на совпадения
        if (CheckForMatches())
        {
            // Если были совпадения, заполняем пустоты
            yield return new WaitForSeconds(0.5f); // Ждем перед заполнением
            while (FillEmptySpaces()) { } // Заполняем пока есть пустоты
        }
        else
        {
            // Если совпадений нет, возвращаем кристаллы на место
            grid[posA_X, posA_Y] = tempType; // Возвращаем тип кристалла
            grid[posB_X, posB_Y] = grid[posA_X, posA_Y];

            // Возвращаем позиции кристаллов
            crystalA.transform.position = tempPosition;
            crystalB.transform.position = new Vector3(posB_X, posB_Y, 0);

            // Обновляем типы кристаллов
            crystalA.SetCrystalType(grid[posA_X, posA_Y]);
            crystalB.SetCrystalType(grid[posB_X, posB_Y]);
        }
    }

    public bool CheckForMatches()
    {
        List<Vector2Int> matchesToRemove = new List<Vector2Int>();

        // Проверка на совпадения по горизонтали
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

        // Проверка на совпадения по вертикали
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
                grid[pos.x, pos.y] = -1; // Удаляем кристаллы
            }
            return true; // Найдены совпадения
        }
        return false; // Совпадений нет
    }

    private bool FillEmptySpaces()
    {
        bool hasFilledSpaces = false;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // Начнем с нижнего ряда и будем двигаться вверх
                if (grid[x, y] == -1) // Если кристалл удален
                {
                    hasFilledSpaces = true; // Показать, что есть заполненные пространства
                                            // Сдвиг вниз
                    for (int k = y; k < gridHeight - 1; k++)
                    {
                        grid[x, k] = grid[x, k + 1]; // Перемещение кристаллов вниз
                    }
                    // В самом нижнем ряду ставим новый кристалл
                    grid[x, gridHeight - 1] = Random.Range(0, crystalPrefabs.Length);
                }
            }
        }

        // Обновляем визуализацию
        UpdateGridVisual();

        // Проверяем наличие матчей после заполнения пустот
        CheckForMatchesAfterFilling();

        return hasFilledSpaces; // Вернуть true, если были заполненные пространства
    }

    private void CheckForMatchesAfterFilling()
    {
        // Проверяем наличие матчей
        while (CheckForMatches())
        {
            // Если были совпадения, заполняем пустоты
            FillEmptySpaces(); // Заполняем пустоты, пока есть совпадения
        }
    }

    private void UpdateGridVisual()
    {
        // Удаляем все старые кристаллы из игрового поля
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Заполняем игровое поле новыми кристаллами
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y] >= 0 && grid[x, y] < crystalPrefabs.Length) // Проверка границ
                {
                    Vector3 position = new Vector3(x, y, 0);
                    GameObject crystal = Instantiate(crystalPrefabs[grid[x, y]], position, Quaternion.identity);
                    crystal.GetComponent<Crystal>().SetCrystalType(grid[x, y]);
                    crystal.transform.SetParent(transform); // Делаем дочерним объектом GameManager
                }
            }
        }
    }

    private bool HasMatches()
    {
        return CheckForMatches();
    }
}
