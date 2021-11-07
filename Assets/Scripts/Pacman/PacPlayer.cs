using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacPlayer : MonoBehaviour
{
    Phantom phantom;
    // Start is called before the first frame update
    void Start()
    {
        this.phantom = GetComponent<Phantom>();
        this.phantom.setAuto(false);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = (Input.GetKey((KeyCode)Keys.RIGHT)? 1: 0) - (Input.GetKey((KeyCode)Keys.LEFT)? 1: 0);
        float vertical = (Input.GetKey((KeyCode)Keys.UP)? 1: 0) - (Input.GetKey((KeyCode)Keys.DOWN)? 1: 0);

        if (horizontal != 0)
            this.phantom.setTargetPos(new Vector2(transform.position.x+horizontal, transform.position.y));
        else if (vertical != 0)
            this.phantom.setTargetPos(new Vector2(transform.position.x, transform.position.y+vertical));
    }
}
