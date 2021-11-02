using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsBullet : MonoBehaviour
{
    [SerializeField]
    float speed = 1;
    float direction;
    float orientation = 0;
    float rotSpeed = 0;
    Vector3 position;
    Vector3 movements;
    // Start is called before the first frame update
    void Start()
    {
        this.direction = (transform.rotation.eulerAngles.z - 90f) * 3.141592f / 180f;
        this.movements = new Vector3(
            - Mathf.Cos(this.direction),
            - Mathf.Sin(this.direction), 0
        );
        this.orientation = Random.Range(0f, 359.9f);
        this.rotSpeed = Random.Range(-10f, 10f);
        this.position = transform.position + this.movements * 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        this.position += this.movements * this.speed * Time.deltaTime;
        transform.position = this.position;

        if (this.position.x < -7f || this.position.x > 7f ||
            this.position.y < -10f || this.position.y > 10f)
            if (gameObject != null) Destroy(gameObject, 1);
    }
}
