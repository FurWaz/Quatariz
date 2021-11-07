using UnityEngine;
using UnityEngine.SceneManagement;
public class PongBall : MonoBehaviour
{

    [SerializeField]
    float maxX, minX, maxY, minY;

    Vector3 direction;
    float speed = 2f;
    float rotSpeed = 0.25f;
    float animTime = 0f;

    Material mat;

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
        this.mat = GetComponent<MeshRenderer>().material;
        this.direction = new Vector2(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.animTime < 2)
        {
            this.animTime += Time.deltaTime;
            if (this.animTime < 1)
                this.mat.color = new Color(this.animTime, this.animTime, this.animTime);
            transform.localScale = new Vector3(this.animTime*0.01f, this.animTime*0.01f, this.animTime*0.01f);
        }


        float horizontal = Input.GetAxisRaw(PlayerController.HORIZONTAL);
        if (horizontal != 0)
        {
            float curAngle = Mathf.Atan2(this.direction.y, this.direction.x);
            curAngle -= Time.deltaTime * horizontal * rotSpeed;
            this.direction = new Vector2(
                Mathf.Cos(curAngle),
                Mathf.Sin(curAngle)
            ).normalized;
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
        SceneManager.LoadScene("Menu");
    }
}
