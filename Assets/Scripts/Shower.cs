using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shower : MonoBehaviour
{
    [SerializeField]
    GameObject obj;
    bool showing = true;
    // Start is called before the first frame update
    void Start()
    {
        toogleShow();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toogleShow()
    {
        showing = !showing;
        obj.SetActive(showing);
    }
}
