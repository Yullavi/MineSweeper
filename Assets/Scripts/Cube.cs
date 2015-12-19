using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Cube : MonoBehaviour
{
    public int CoordX { get; private set; }
    public int CoordY { get; private set; }
    public int NumberCube { get; private set; }
    public event Action<Cube> Open;

    public void OnMouseUp()
    {
        gameObject.GetComponentInChildren<Text>().text = (-NumberCube).ToString();
        Open(this);
    }

    public void Init(int coordX, int coordY, int numberCube)
    {
        NumberCube = numberCube;
        CoordY = coordY;
        CoordX = coordX;
        gameObject.name = "Cube" + " " + coordX + " " + coordY;
        gameObject.GetComponentInChildren<Text>().text = numberCube.ToString();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
