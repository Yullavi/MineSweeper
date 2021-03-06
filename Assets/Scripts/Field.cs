﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class Field : MonoBehaviour
{
    public Menu Menu;
    public Canvas NumberMinesLeavs;
    public Cube CubePrefab;
    public Canvas CanvasPrefab;
    private Canvas _canvas;
    private int[,] ArrayField { get; set; }
    private int _numberMinesRemained;
    public GameObject Explosion;

    private int IndexA;
    private int IndexB;
    private int BombeNumber;
    Cube[,] _cube;
    public Camera Camera;
    private int _countMarks = 0;

    // Use this for initialization
    void Start()
    {
        FieldInit();
    }

    private void OnRightMarkInt()
    {
        _countMarks++;

        if (_countMarks == IndexA * IndexB)
        {
            Win();
        }
    }

    private void OnWrongMark()
    {
        _numberMinesRemained--;
        NumberMinesLeavs.GetComponentInChildren<Text>().text = _numberMinesRemained.ToString();
    }

    private void OnDeMark(Cube cube)
    {
        if (cube.NumberCube == 9)
            _countMarks--;
        _numberMinesRemained++;
        NumberMinesLeavs.GetComponentInChildren<Text>().text = _numberMinesRemained.ToString();

    }

    private void OnRightMark()
    {
        _countMarks++;
        _numberMinesRemained--;
        NumberMinesLeavs.GetComponentInChildren<Text>().text = _numberMinesRemained.ToString();

        if (_countMarks == IndexA * IndexB)
        {
            Win();
        }
    }

    private void FieldSelectedRight(Cube cube)
    {
        if(!cube.IsOpened)
        cube.Block();
    }

    private Cube[] GetNeighbors(Cube cube)
    {
        List<Cube> neighbors = new List<Cube>();
        foreach (var coord in GetCoords(cube.CoordX, cube.CoordZ))
        {
            neighbors.Add(_cube[coord.X, coord.Z]);
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
        Instantiate(Explosion, Vector3.zero, transform.rotation);

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
                        if (newField[coord.X, coord.Z] == 9)
                            count++;
                    }
                    newField[i, j] = count;
                }
            }
        }
        return newField;
    }

    private void FieldInit()
    {
        IndexA = Menu.IndexA;
        IndexB = Menu.IndexB;
        BombeNumber = Menu.BombeNumber;
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

        var pos = Vector3.zero;
        for (int i = 0; i < IndexB; i++)
        {
            for (int j = 0; j < IndexA; j++)
            {
                pos.x = (float)((i - IndexB / 2) * 1.2);
                pos.z = (float)((j - IndexA / 2) * 1.2);

                _cube[i, j] = (Cube)Instantiate(CubePrefab, pos, CubePrefab.transform.rotation);
                _cube[i, j].Init(i, j, ArrayField[i, j]);
                _cube[i, j].SelectedLeft += FieldSelectedLeft;
                _cube[i, j].SelectedRight += FieldSelectedRight;
                _cube[i, j].RightMark += OnRightMark;
                _cube[i, j].RightMarkInt += OnRightMarkInt;
                _cube[i, j].WrongMark += OnWrongMark;
                _cube[i, j].DeMark += OnDeMark;
            }
        }
        _numberMinesRemained = BombeNumber;
        NumberMinesLeavs.GetComponentInChildren<Text>().text = _numberMinesRemained.ToString();
    }
    public void RestartOnClick()
    {
        foreach (var cube in _cube)
        {
            cube.Destroyer();
        }
        FieldInit();
    }

    public void MenuOnClick()
    {
        foreach (var cube in _cube)
        {
            cube.EnableFalse();
        }
    }

    private void Win()
    {
        _canvas = (Canvas)Instantiate(CanvasPrefab, CanvasPrefab.transform.position, CanvasPrefab.transform.rotation);
    }

    struct Coords
    {
        public int X;
        public int Z;

        public Coords(int x, int y)
        {
            X = x;
            Z = y;
        }
    }
}

