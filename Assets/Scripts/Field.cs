using UnityEngine;
using System.Collections;


public class Field : MonoBehaviour
{
    public Cube CubePrefab;
    private int[,] ArrayField { get; set; }

    public int IndexA { get; set; }
    public int IndexB { get; set; }
    public int BombeNumber { get; set; }
    Cube[,] cube;


    // Use this for initialization
    void Start()
    {
        
        IndexA = 5;
        IndexB = 9;
        BombeNumber = 15;
        ArrayField = CreatField(IndexA, IndexB, BombeNumber);
        cube = new Cube[IndexB, IndexA];

        string textField = null;
        for (int i = 0; i < ArrayField.GetLength(0); i++)
        {
            for (int j = 0; j < ArrayField.GetLength(1); j++)
            {
                textField += " " + ArrayField[i, j];
            }
            textField += "\n";

        }
        Debug.Log(textField);

        var pos = Vector3.zero;
        for (int i = 0; i < IndexB; i++)
        {
            for (int j = 0; j < IndexA; j++)
            {
                pos.x = (float)(i * 1.2);
                pos.y = (float)(j * 1.2);

                cube[i, j] = (Cube)Instantiate(CubePrefab, pos, CubePrefab.transform.rotation);
                cube[i, j].Init(i, j, ArrayField[i, j]);
                cube[i, j].Open += Field_Open;
            }
        }
    }

    private void Field_Open(Cube cube)
    {
        Debug.Log(cube.name);
        cube.transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButton("Fire1"))
        {
            for (int i = 0; i < ArrayField.GetLength(0); i++)
            {
                for (int j = 0; j < ArrayField.GetLength(1); j++)
                {
                    ArrayField[i, j] = -ArrayField[i, j];
                }
            }
        }

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
