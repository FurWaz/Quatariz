using System;
using UnityEngine;
using UnityEngine.UI;

public class KeySetup : MonoBehaviour
{
    [SerializeField]
    GameObject buttonUP, buttonDOWN, buttonLEFT, buttonRIGHT, buttonACTION;

    bool listUP, listDOWN, listLEFT, listRIGHT, listACTION;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (listUP||listDOWN||listLEFT||listRIGHT||listACTION)
            foreach(KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kcode))
                {
                    Debug.Log("key: "+kcode);
                    if (listUP) Keys.UP = (int) kcode;
                    if (listDOWN) Keys.DOWN = (int) kcode;
                    if (listLEFT) Keys.LEFT = (int) kcode;
                    if (listRIGHT) Keys.RIGHT = (int) kcode;
                    if (listACTION) Keys.ACTION = (int) kcode;
                    listUP = false;
                    listDOWN = false;
                    listLEFT = false;
                    listRIGHT = false;
                    listACTION = false;
                    buttonUP.transform.GetChild(0).GetComponent<Text>().text = "Setup [UP] Key";
                    buttonDOWN.transform.GetChild(0).GetComponent<Text>().text = "Setup [DOWN] Key";
                    buttonLEFT.transform.GetChild(0).GetComponent<Text>().text = "Setup [LEFT] Key";
                    buttonRIGHT.transform.GetChild(0).GetComponent<Text>().text = "Setup [RIGHT] Key";
                    buttonACTION.transform.GetChild(0).GetComponent<Text>().text = "Setup [ACTION] Key";
                }
            }
    }

    public void setupUP()
    {
        listUP = true;
        buttonUP.transform.GetChild(0).GetComponent<Text>().text = "...";
    }

    public void setupDOWN()
    {
        listDOWN = true;
        buttonDOWN.transform.GetChild(0).GetComponent<Text>().text = "...";
    }

    public void setupLEFT()
    {
        listLEFT = true;
        buttonLEFT.transform.GetChild(0).GetComponent<Text>().text = "...";
    }

    public void setupRIGHT()
    {
        listRIGHT = true;
        buttonRIGHT.transform.GetChild(0).GetComponent<Text>().text = "...";
    }

    public void setupACTION()
    {
        listACTION = true;
        buttonACTION.transform.GetChild(0).GetComponent<Text>().text = "...";
    }
}
