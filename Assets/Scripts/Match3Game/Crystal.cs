using UnityEngine;

public class Crystal : MonoBehaviour
{
    private int crystalType;

    public void SetCrystalType(int type)
    {
        crystalType = type;
        // Здесь добавьте код для визуализации типа кристалла (например, смена текстуры)
    }

    public int GetCrystalType()
    {
        return crystalType;
    }
}
