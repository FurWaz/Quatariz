using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan : MonoBehaviour
{
    static Vector2 DIR_LEFT = new Vector2(-1, 0);
    static Vector2 DIR_RIGHT = new Vector2(1, 0);
    static Vector2 DIR_UP = new Vector2(0, 1);
    static Vector2 DIR_DOWN = new Vector2(0, -1);
    Vector2 position, targetPosition, direction;
    float speed = 1.5f;
    float animTime, dieTime, animSpeed = 2f;
    Material mat;
    bool dead = false;
    static PacMan instance;

    public static void setPos(Vector2 pos)
    {
        instance.position = pos;
        instance.targetPosition = pos;
        instance.transform.position = pos;
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        this.mat = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.dead)
        {
            this.dieTime += Time.deltaTime;
            if (this.dieTime <= 1.4)
            {
                float offset = (int)(this.dieTime*4) * 0.25f;
                this.mat.mainTextureOffset = new Vector2(offset, 0f);
            }

            if (this.dieTime > 2)
                UnityEngine.SceneManagement.SceneManager.LoadScene("Pong");
            return;
        }

        if (this.position.x - 0.01f < this.targetPosition.x &&
            this.position.x + 0.01f > this.targetPosition.x &&
            this.position.y - 0.01f < this.targetPosition.y &&
            this.position.y + 0.01f > this.targetPosition.y) // block reached, go to new block
        {
            Vector2 newTargetPos = this.position + this.direction;
            bool canMove = false;
            ArrayList movements = new ArrayList();
            movements.Add(DIR_LEFT); movements.Add(DIR_RIGHT);
            movements.Add(DIR_UP); movements.Add(DIR_DOWN);
            while (!canMove)
            {
                if (movements.Count < 1) break;
                int random = Random.Range(0, movements.Count-1);
                Vector2 move = (Vector2)movements[random];
                Vector2 newPos = this.position + move;
                if (!PacMap.isWall(PacMap.getBlockAt(newPos.x, newPos.y)))
                {
                    if (this.direction != -move || movements.Count == 1)
                    {
                        this.targetPosition = newPos;
                        this.direction = move;
                        canMove = true;
                    }
                }
                movements.RemoveAt(random);
            }
        }
        // choose direction
        if (this.position.x - this.targetPosition.x != 0)
        {
            if (this.position.x < this.targetPosition.x)
                this.direction = DIR_RIGHT;
            else this.direction = DIR_LEFT;
        } else
        {
            if (this.position.y < this.targetPosition.y)
                this.direction = DIR_UP;
            else this.direction = DIR_DOWN;
        }

        Vector2 nextPos = this.position + this.direction * this.speed * Time.deltaTime;
        if (nextPos.x < -11)
        {
            nextPos.x = 10;
            targetPosition = new Vector2(8, 0);
        }
        if (nextPos.x > 10)
        {
            nextPos.x = -11;
            targetPosition = new Vector2(-9, 0);
        }
        this.position = nextPos;
        transform.position = nextPos;

        // animation stuff
        if (this.direction.x != 0)
        {
            if (this.direction.x > 0) transform.localRotation = new Quaternion(0, -0.707106769f, 0.707106769f,0);
            else transform.localRotation = new Quaternion(-0.707106769f, 0f, 0f, 0.707106769f);
        }
        if (this.direction.y != 0)
        {
            if (this.direction.y > 0) transform.localRotation = new Quaternion(0.5f, -0.5f, 0.5f, -0.5f);
            else transform.localRotation = new Quaternion(-0.5f, -0.5f, 0.5f, 0.5f);
        }

        this.animTime += Time.deltaTime * animSpeed;
        if (this.animTime > 1) this.animTime--;
        float offX = (int)(animTime*4) * 0.25f;
        this.mat.mainTextureOffset = new Vector2(offX, 0.5f);

        // if pacman touched, die
        if (this.position.x - 0.08f < Phantom.red.getPos().x &&
            this.position.x + 0.08f > Phantom.red.getPos().x &&
            this.position.y - 0.08f < Phantom.red.getPos().y &&
            this.position.y + 0.08f > Phantom.red.getPos().y) // block reached, go to new block
        {
            this.die();
        }
    }

    void die()
    {
        Phantom.blue.setReady(false);
        Phantom.orange.setReady(false);
        Phantom.pink.setReady(false);
        Phantom.red.setReady(false);
        PacMap.clear();

        this.dieTime = 0;
        this.dead = true;
    }
}
