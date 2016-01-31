using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Menu : MonoBehaviour
{
    public Toggle TogBaby;
    public Toggle TogMaster;
    public Toggle TogExpert;
    public Text BabyWidth;
    public Text BabyHeight;
    public Text BabyBombs;
    public Text MasterWidth;
    public Text MasterHeight;
    public Text MasterBombs;
    public Text ExpertWidth;
    public Text ExpertHeight;
    public Text ExpertBombs;


    // Use this for initialization
    void Start () {
        OnToggleSelected();
    }


    public int BombeNumber { get; private set; }

    public int IndexB { get; private set; }

    public int IndexA { get; private set; }

    // Update is called once per frame
	public void OnToggleSelected () {
        if (TogBaby.isOn)
        {
            IndexA = int.Parse(BabyWidth.text);
            IndexB = int.Parse(BabyHeight.text);
            BombeNumber = int.Parse(BabyBombs.text);
        }
        else if (TogMaster.isOn)
        {
            IndexA = int.Parse(MasterWidth.text);
            IndexB = int.Parse(MasterHeight.text);
            BombeNumber = int.Parse(MasterBombs.text);
        }
        else if (TogExpert.isOn)
        {
            IndexA = int.Parse(ExpertWidth.text);
            IndexB = int.Parse(ExpertHeight.text);
            BombeNumber = int.Parse(ExpertBombs.text);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
