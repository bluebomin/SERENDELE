using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    public void ToMAP()
    {
        SceneManager.LoadScene("NURI4");
        //SceneManager �޼����� LoadScene �Լ��� ���� NURI4.scene���� �� ��ȯ
    }

    public void ToDUNGEON1()
    {
        SceneManager.LoadScene("BUILDING 1");
        //SceneManager �޼����� LoadScene �Լ��� ���� BUILDING 1.scene���� �� ��ȯ
    }
}
