using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    GameObject player, back;
    float speed = 2.0f;
    float zoom = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 camPos = transform.position;
        Vector3 movements = new Vector3(
            playerPos.x - camPos.x,
            playerPos.y - camPos.y,
            zoom + camPos.z
        );
        movements *= Time.deltaTime * speed;
        transform.Translate(movements);
        back.transform.Translate(movements * 0.5f);
    }
}
