using System;
using UnityEngine;
using UnityEngine.UI;

public class Cube : MonoBehaviour
{
    public int CoordX { get; private set; }
    public int CoordZ { get; private set; }
    public int NumberCube { get; private set; }
    public event Action<Cube> SelectedLeft;
    public event Action<Cube> SelectedRight;
    public event Action RightMark;
    public event Action RightMarkInt;
    public event Action WrongMark;
    public event Action<Cube> DeMark;
    public bool IsOpened { get; private set; }
    public bool IsBlocked { get; private set; }


    public void Open()
    {
        if (!IsBlocked && !IsOpened)
        {
            IsOpened = true;
            if (NumberCube != 0)
            {
                gameObject.transform.rotation = Quaternion.identity;
                RightMarkInt();
            }
            else
            {
                transform.localScale = transform.localScale / 2;
                RightMarkInt();
            }

        }
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
            SelectedLeft(this);
        if (Input.GetMouseButtonUp(1))
            SelectedRight(this);
    }

    public void Init(int coordX, int coordZ, int numberCube)
    {
        NumberCube = numberCube;
        CoordZ = coordZ;
        CoordX = coordX;
        gameObject.name = "Cube" + " " + coordX + " " + coordZ + " " + numberCube;
        gameObject.GetComponentInChildren<Text>().text = numberCube.ToString();
    }

    public void Block()
    {
        IsBlocked = !IsBlocked;
        if (IsBlocked)
        {
            gameObject.GetComponentInChildren<Text>().text = '\u2552'.ToString();
            gameObject.transform.rotation = Quaternion.identity;
            if (NumberCube == 9) RightMark();
            else
            {
                WrongMark();
            }
        }
        if (!IsBlocked)
        {
            gameObject.GetComponentInChildren<Text>().text = NumberCube.ToString();
            var rot = transform.rotation;
            rot.x = 180;
            gameObject.transform.rotation = rot;
            DeMark(this);
        }
    }

    public void Destroyer()
    {
        if (this)
            Destroy(gameObject);
    }
}
