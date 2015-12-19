﻿using UnityEngine;
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

    private Cube[] GetNeighbors(Cube cube)
    {
        List<Cube> neighbors = new List<Cube>();
        foreach (var coord in GetCoords(cube.CoordX,cube.CoordY))
        {
            neighbors.Add(_cube[coord.X,coord.Y]);
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

    // Use this for initialization
    void Start()
    {
        ArrayField = CreatField(IndexA, IndexB, BombeNumber);
        _cube = new Cube[IndexB, IndexA];

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
                _cube[i, j].Selected += FieldSelected;
            }
        }
    }

    private void FieldSelected(Cube cube)
    {
        Debug.Log(cube.name);
        if (cube.NumberCube == 0)
        {
            OpenEmptyField(cube);
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
                    foreach (var coord in GetCoords(i,j))
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

