using System.Collections;
using UnityEngine;

public class DelayedActivator : MonoBehaviour
{
    public GameObject objectToActivate; // ������, ������� ����� ������������
    public float delay = 2f; // �������� ����� ���������� (� ��������)

    void Start()
    {
        // ��������� �������� ��� ��������� ������� � ���������
        StartCoroutine(ActivateAfterDelay());
    }

    // �������� ��� ��������� ������� � ���������
    IEnumerator ActivateAfterDelay()
    {
        // ���� ��������� ���������� �������
        yield return new WaitForSeconds(delay);

        // ���������� ������
        objectToActivate.SetActive(true);
    }
}
