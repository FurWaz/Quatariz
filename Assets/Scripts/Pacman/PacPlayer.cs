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
        float horizontal = Input.GetAxisRaw(PlayerController.HORIZONTAL);
        float vertical = Input.GetAxisRaw(PlayerController.VERTICAL);

        if (horizontal != 0)
            this.phantom.setTargetPos(new Vector2(transform.position.x+horizontal, transform.position.y));
        else if (vertical != 0)
            this.phantom.setTargetPos(new Vector2(transform.position.x, transform.position.y+vertical));
    }
}
