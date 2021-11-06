using UnityEngine;

public class PongBall : MonoBehaviour
{

    [SerializeField]
    float maxX, minX, maxY, minY;

    Vector3 direction;
    float speed = 3f;

    public void changeX()
    {
        this.direction.x = -this.direction.x;
    }

    public float getDirectionX()
    {
        return this.direction.x;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.direction = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            0
        );
        this.direction.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw(PlayerController.HORIZONTAL);
        float vertical = Input.GetAxisRaw(PlayerController.VERTICAL);
        if (horizontal != 0 || vertical != 0)
        {
            // modify direction based on the key
        }

        Vector3 movements = this.direction * Time.deltaTime * this.speed;
        Vector3 newPos = (transform.position + movements);

        if ((newPos.x < minX && movements.x < 0) || (newPos.x > maxX && movements.x > 0))
            this.win();
        if ((newPos.y < minY && movements.y < 0) || (newPos.y > maxY && movements.y > 0))
            this.direction.y = -this.direction.y;

        transform.position += this.direction * Time.deltaTime * this.speed;
    }

    void win()
    {

    }
}
