using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Field : MonoBehaviour
{
    public Cube CubePrefab;

    private int[,] ArrayField { get; set; }

    public int IndexA = 5;
    public int IndexB = 9;
    public int BombeNumber = 7;
    Cube[,] _cube;
    public Camera Camera;

    // Use this for initialization
    void Start()
    {
        ArrayField = CreatField(IndexA, IndexB, BombeNumber);
        _cube = new Cube[IndexB, IndexA];

        CameraPosition();

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

                _cube[i, j] = (Cube)Instantiate(CubePrefab, pos, CubePrefab.transform.rotation);
                _cube[i, j].Init(i, j, ArrayField[i, j]);
                _cube[i, j].SelectedLeft += FieldSelectedLeft;
                _cube[i, j].SelectedRight += FieldSelectedRight;
            }
        }
    }

    private void CameraPosition()
    {
        var posCamera = Camera.transform.position;
        posCamera.x = (float)((IndexB * 1.2 -1) / 2);
        posCamera.y = (float)((IndexA * 1.2 -1) / 2);
        if (IndexA >= 6 || IndexB >= 10) posCamera.z = -11;
        if (IndexA >= 8 || IndexB >= 14) posCamera.z = -12;
        if (IndexA >= 10 || IndexB >= 18) posCamera.z = -14;
        if (IndexA >= 13 || IndexB >= 22) posCamera.z = -16;
        Camera.transform.position = posCamera;
    }

    private void FieldSelectedRight(Cube cube)
    {
        cube.Block();
    }

    private Cube[] GetNeighbors(Cube cube)
    {
        List<Cube> neighbors = new List<Cube>();
        foreach (var coord in GetCoords(cube.CoordX, cube.CoordY))
        {
            neighbors.Add(_cube[coord.X, coord.Y]);
        }
        return neighbors.ToArray();
    }

    private Coords[] GetCoords(int x, int y)
    {
        List<Coords> neighborsCoords = new List<Coords>();
        if (x > 0 && y > 0)
        {
            neighborsCoords.Add(new Coords(x - 1, y - 1));
        }
        if (x > 0 && y < IndexA - 1)
        {
            neighborsCoords.Add(new Coords(x - 1, y + 1));
        }
        if (x < IndexB - 1 && y > 0)
        {
            neighborsCoords.Add(new Coords(x + 1, y - 1));
        }
        if (x < IndexB - 1 && y < IndexA - 1)
        {
            neighborsCoords.Add(new Coords(x + 1, y + 1));
        }
        if (x > 0)
        {
            neighborsCoords.Add(new Coords(x - 1, y));
        }
        if (y < IndexA - 1)
        {
            neighborsCoords.Add(new Coords(x, y + 1));
        }
        if (x < IndexB - 1)
        {
            neighborsCoords.Add(new Coords(x + 1, y));
        }
        if (y > 0)
        {
            neighborsCoords.Add(new Coords(x, y - 1));
        }
        return neighborsCoords.ToArray();
    }



    private void FieldSelectedLeft(Cube cube)
    {
        Debug.Log(cube.name);
        if (cube.NumberCube == 0)
        {
            OpenEmptyField(cube);
        }
        if (cube.NumberCube == 9)
        {
            DestroyCloseField(cube);
        }
        cube.Open();

    }

    private void OpenEmptyField(Cube cube)
    {
        if (cube.IsOpened == true)
        {
            return;
        }
        if (cube.NumberCube != 9)
        {
            cube.Open();
            if (cube.NumberCube != 0)
            {
                return;
            }
            foreach (var c in GetNeighbors(cube))
            {
                OpenEmptyField(c);
            }
        }
    }
    private void DestroyCloseField(Cube cube)
    {
        if (cube.IsOpened == true)
        {
            return;
        }
        foreach (var c in _cube)
        {
            c.Destroyer();
        }


        /*        foreach (var c in GetNeighbors(cube))
                {
                    DestroyCloseField(c);
                }*/
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

            } while (k == 9);
            newField[r, c] = 9;
        }

        //вычисление цифр
        for (int i = 0; i < IndexB; i++)
        {
            for (int j = 0; j < IndexA; j++)
            {
                int count = 0;
                if (newField[i, j] != 9)
                {
                    foreach (var coord in GetCoords(i, j))
                    {
                        if (newField[coord.X, coord.Y] == 9)
                            count++;
                    }
                    newField[i, j] = count;
                }
            }
        }
        return newField;
    }

    struct Coords
    {
        public int X;
        public int Y;

        public Coords(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}

