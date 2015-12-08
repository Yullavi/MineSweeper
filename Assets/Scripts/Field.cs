using UnityEngine;
using System.Collections;


public class Field : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        int[,] a = CreatField(9, 7, 15);
        string b = null;
        for (int i = 0; i < a.GetLength(0); i++)
        {
            for (int j = 0; j < a.GetLength(1); j++)
            {
                b = b + " " + a[i, j];
            }
            b += "\n";

        }
        Debug.Log(b);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private int[,] CreatField(int column, int row, int bombsNumber)
    {
        int[,] newField = new int[row, column];

        //заполнение минами
        int r, c, k;
        for (int i = 0; i < bombsNumber; i++)
        {
            do
            {
                c = Random.Range(0, column);
                r = Random.Range(0, row);
                k = newField[r, c];

            } while (k == -9);
            newField[r, c] = -9;
        }

        //вычисление цифр
        for (int i = 0; i < newField.GetLength(0); i++)
        {
            for (int j = 0; j < newField.GetLength(1); j++)
            {
                int count = 0;
                if (newField[i, j] != -9)
                {
                    if (i < (newField.GetLength(0) - 1) & j < (newField.GetLength(1) - 1))
                    {
                        if (newField[i + 1, j + 1] == -9)
                        {
                            count--;
                        }
                    }
                    if (i < (newField.GetLength(0) - 1) & j > 0)
                    {
                        if (newField[i + 1, j - 1] == -9)
                        {
                            count--;
                        }
                    }
                    if (i > 0 & j < (newField.GetLength(1) - 1))
                    {
                        if (newField[i - 1, j + 1] == -9)
                        {
                            count--;
                        }
                    }
                    if (i > 0 & j > 0)
                    {
                        if (newField[i - 1, j - 1] == -9)
                        {
                            count--;
                        }
                    }

                    if (i < (newField.GetLength(0) - 1))
                    {
                        if (newField[i + 1, j] == -9)
                        {
                            count--;
                        }
                    }
                    if (j < (newField.GetLength(1) - 1))
                    {
                        if (newField[i, j + 1] == -9)
                        {
                            count--;
                        }
                    }
                    if (i > 0)
                    {
                        if (newField[i - 1, j] == -9)
                        {
                            count--;
                        }
                    }
                    if (j > 0)
                    {
                        if (newField[i, j - 1] == -9)
                        {
                            count--;
                        }
                    }
                    newField[i, j] = count;
                }
            }
        }

        return newField;
    }
}
