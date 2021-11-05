using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsteroidsPlayer : MonoBehaviour
{
    public static List<GameObject> bullets;

    [SerializeField]
    float force = 0.05f, rotSpeed = 4;
    [SerializeField]
    GameObject templateBullet, losePanel, winPanel, fire, scoreGoal;
    float direction;
    float velocity;
    float lapse = 0;
    float shootTimeout = 0;
    float animate = 1;
    Vector2 position;
    Vector2 velo;
    Vector2 movement;

    static AsteroidsPlayer instance;

    public static Vector3 getPos()
    {
        return instance.transform.position;
    }

    public static void makeDie()
    {
        instance.die();
    }

    public static void makeWin()
    {
        instance.win();
    }

    // Start is called before the first frame update
    void Start()
    {
        bullets = new List<GameObject>();
        instance = this;
        this.velo = new Vector2(
            Mathf.Cos(this.direction),
            Mathf.Sin(this.direction)
        );
        transform.rotation = Quaternion.Euler(0, 0, this.direction * (180/3.1415926f) - 90f);
        int level = PlayerController.readLevel();
        if (level == 1) // level in history mode
            scoreGoal.SetActive(true);
        else scoreGoal.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (lapse > 0) lapse -= Time.deltaTime;

        if (!AstreoidsManager.running)
        {
            if (Input.GetKey(KeyCode.Return))
            {
                if (lapse > 0) return;
                if (!winPanel.activeSelf)
                {
                    AstreoidsManager.running = true;
                    this.position = new Vector2(0, 0);
                    losePanel.SetActive(false);
                } else
                    SceneManager.LoadScene("menu");
            }
            return;
        }

        float accel = Input.GetAxisRaw(PlayerController.VERTICAL);
        float dir = Input.GetAxisRaw(PlayerController.HORIZONTAL);
        if (dir != 0)
        {
            this.direction -= dir * Time.deltaTime * rotSpeed;
            this.velo = new Vector2(
                Mathf.Cos(this.direction),
                Mathf.Sin(this.direction)
            );
            transform.rotation = Quaternion.Euler(0, 0, this.direction * (180/3.1415926f) - 90f);
        }

        // calculate the new translation
        this.velocity = 0;
        if (accel > 0)
        {
            fire.SetActive(true);
            fire.transform.localPosition = new Vector3(
                Random.Range(-0.03f, 0.03f),
                Random.Range(-0.03f, 0.03f),
                0
            );
            this.velocity = this.force * Time.deltaTime;
        } else
            fire.SetActive(false);
        this.movement += this.velocity * this.velo;
        this.movement /= 1f + Time.deltaTime;

        // apply the translation
        this.position += this.movement;
        this.position.x = Mathf.Clamp(this.position.x, -8f, 8f);
        this.position.y = Mathf.Clamp(this.position.y, -6f, 6f);
        
        if (this.animate > 0)
        {
            this.position = new Vector2(0, this.animate*4f);
            this.animate -= Time.deltaTime;
        }
        transform.position = this.position;

        // check for fire 
        this.shootTimeout += Time.deltaTime;
        if ((Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.Space)) && this.shootTimeout >= 0.2f)
        {
            bullets.Add(
                GameObject.Instantiate(templateBullet, transform.position, transform.rotation)
            );
            this.shootTimeout = 0;
        }
    }

    void die()
    {
        losePanel.SetActive(true);
        lapse = 0.5f;
        AstreoidsManager.running = false;
    }

    void win()
    {
        winPanel.SetActive(true);
        lapse = 0.5f;
        AstreoidsManager.running = false;
    }
}
