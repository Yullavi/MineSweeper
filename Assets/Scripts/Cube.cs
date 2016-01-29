using System;
using UnityEngine;
using UnityEngine.UI;

public class Cube : MonoBehaviour
{
    public GameObject Explosion;
    public int CoordX { get; private set; }
    public int CoordY { get; private set; }
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
        if (!IsBlocked&&!IsOpened)
        {
            IsOpened = true;
            if (NumberCube != 0)
            {
                gameObject.transform.rotation = Quaternion.identity;
                Debug.Log(name);
                RightMarkInt();
            }
            else
            {
                var scale = transform.localScale;
                scale.z = scale.z / 5;
                gameObject.transform.localScale = scale;
                var pos = transform.position;
                pos.z = 1;
                gameObject.transform.position = pos;
                Debug.Log(name);

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

    public void Init(int coordX, int coordY, int numberCube)
    {
        NumberCube = numberCube;
        CoordY = coordY;
        CoordX = coordX;
        gameObject.name = "Cube" + " " + coordX + " " + coordY + " " + numberCube;
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
        if (!IsBlocked)
        {
            Destroy(gameObject);
            Instantiate(Explosion, transform.position, transform.rotation);
        }
    }
}
