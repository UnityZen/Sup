using UnityEngine;

public class Crystal : MonoBehaviour
{
    private int crystalType;

    public void SetCrystalType(int type)
    {
        crystalType = type;
        // ����� �������� ��� ��� ������������ ���� ��������� (��������, ����� ��������)
    }

    public int GetCrystalType()
    {
        return crystalType;
    }
}
