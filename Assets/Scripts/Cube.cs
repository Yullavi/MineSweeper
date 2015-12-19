using System;
using UnityEngine;
using UnityEngine.UI;

public class Cube : MonoBehaviour
{
    public int CoordX { get; private set; }
    public int CoordY { get; private set; }
    public int NumberCube { get; private set; }
    public event Action<Cube> Selected;
    public Field Field;
    public bool IsOpened { get; private set; }

    public void Open()
    {
        IsOpened = true;
        gameObject.transform.rotation = Quaternion.identity;
    }

    public void OnMouseUp()
    {
        gameObject.GetComponentInChildren<Text>().text = NumberCube.ToString();
        Selected(this);
    }

    public void Init(int coordX, int coordY, int numberCube)
    {
        NumberCube = numberCube;
        CoordY = coordY;
        CoordX = coordX;
        gameObject.name = "Cube" + " " + coordX + " " + coordY + " " + numberCube;
        gameObject.GetComponentInChildren<Text>().text = numberCube.ToString();
    }
}
