using System.Collections;
using UnityEngine;

public class Phantom : MonoBehaviour
{
    static Vector2 DIR_LEFT = new Vector2(-1, 0);
    static Vector2 DIR_RIGHT = new Vector2(1, 0);
    static Vector2 DIR_UP = new Vector2(0, 1);
    static Vector2 DIR_DOWN = new Vector2(0, -1);
    static int nbDone = 0;

    public static Phantom red;
    public static Phantom blue;
    public static Phantom orange;
    public static Phantom pink;

    const float ANIM_UP = 0f;
    const float ANIM_LEFT = 0.25f;
    const float ANIM_DOWN = 0.5f;
    const float ANIM_RIGHT = 0.75f;
    Vector2 direction, position, targetPosition;
    bool auto = true;
    bool ready = false;
    float speed = 1f;
    float animTime = 0f;
    float animType = ANIM_DOWN;
    float animSpeed = 1f;
    Material mat;
    // Start is called before the first frame update
    void Start()
    {
        switch (gameObject.name.Substring(7).ToLower())
        {
            case "red": red = this; nbDone++; break;
            case "pink": pink = this; nbDone++; break;
            case "blue": blue = this; nbDone++; break;
            case "orange": orange = this; nbDone++; break;
            default: break;
        };
        if (nbDone == 4) PacMap.launch();
        this.mat = GetComponent<MeshRenderer>().material;
        this.speed = Random.Range(1f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        // auto phantom stuff
        if (this.auto)
        {
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
        } else
        {
            if (this.position.x - 0.01f < this.targetPosition.x &&
                this.position.x + 0.01f > this.targetPosition.x &&
                this.position.y - 0.01f < this.targetPosition.y &&
                this.position.y + 0.01f > this.targetPosition.y)
            {
                Vector2 newPos = this.position + this.direction * 0.6f;
                if (PacMap.isWall(PacMap.getBlockAt(newPos.x, newPos.y)))
                    this.direction = new Vector2(0, 0);
                else this.targetPosition = this.position + this.direction;
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
            this.setPos(new Vector2(10, 0));
        if (nextPos.x > 10)
            this.setPos(new Vector2(11, 0));
        this.position = nextPos;
        transform.position = nextPos;

        // animation stuff
        if (this.direction.x != 0)
        {
            if (this.direction.x > 0) this.animType = ANIM_RIGHT;
            else this.animType = ANIM_LEFT;
        }
        if (this.direction.y != 0)
        {
            if (this.direction.y > 0) this.animType = ANIM_UP;
            else this.animType = ANIM_DOWN;
        }

        this.animTime += Time.deltaTime * animSpeed;
        if (this.animTime > 1) this.animTime--;
        float offX = (int)(animTime*2) * 0.5f;
        this.mat.mainTextureOffset = new Vector2(offX, animType);
    }

    public void setPos(Vector2 pos)
    {
        this.position = pos;
        this.targetPosition = this.position;
        transform.position = pos;
    }

    public Vector2 getPos()
    {
        return this.position;
    }

    public void setTargetPos(Vector2 pos)
    {
        if (this.position.x - 0.01f < this.targetPosition.x &&
            this.position.x + 0.01f > this.targetPosition.x &&
            this.position.y - 0.01f < this.targetPosition.y &&
            this.position.y + 0.01f > this.targetPosition.y)
        {
            if (!PacMap.isWall(PacMap.getBlockAt(pos.x, pos.y)))
                this.targetPosition = pos;
            else Debug.Log("new positon in wall, rejected");
        } else Debug.Log("not at final position yet");
    }

    public void setReady(bool state)
    {
        this.ready = state;
        if (!state) gameObject.SetActive(false);
    }

    public Vector2 getDir()
    {
        return this.direction;
    }

    public void setDir(Vector2 dir)
    {
        this.direction = dir;
    }

    public bool isAuto()
    {
        return this.auto;
    }

    public void setAuto(bool state)
    {
        this.auto = state;
    }
}
