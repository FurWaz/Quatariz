using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    public const string HORIZONTAL = "Horizontal";
    public const string VERTICAL = "Vertical";
    private const float ANIM_WALK = 0f;
    private const float ANIM_FALL = 0.25f;
    private const float ANIM_JUMP = 0.5f;
    private const float ANIM_IDLE = 0.75f;

    [SerializeField]
    float speed = 4f, animSpeed = 2f;
    [SerializeField]
    Texture2D texLeft, texRight;

    float yVel = 0f;
    Material mat;
    float animTime = 0f;
    float animType = ANIM_IDLE;
    bool dead = false;

    public static void writeLevel(int level)
    {
        System.IO.File.WriteAllText(Application.persistentDataPath+"/level.txt", level.ToString());
    }
    public static bool existsLevel()
    {
        return System.IO.File.Exists(Application.persistentDataPath+"/level.txt");
    }
    public static int readLevel()
    {
        string text = System.IO.File.ReadAllText(Application.persistentDataPath+"/level.txt");
        return System.Int32.Parse(text);
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(1, 0, 0);
        if (existsLevel())
        {
            switch (readLevel())
            {
                case 2:
                    transform.position = new Vector3(14, 5, 0);
                    break;
                case 5:
                    transform.position = new Vector3();
                    break;
                default: break;
            }
        }
        writeLevel(0);
        this.mat = GetComponent<MeshRenderer>().material;
    }

    public void setYVel(float vel)
    {
        this.yVel = vel;
    }

    // Update is called once per frame
    void Update()
    {
        // respawn if falled
        if (transform.position.y < -30 && transform.position.x < 10)
            transform.position = new Vector3(1, 0, 0);

        // launch other scenes if moved too far
        if (transform.position.y > 10)
        {
            writeLevel(1);
            SceneManager.LoadScene("Asteroids");
        }
        if (transform.position.y < -30)
        {
            writeLevel(3);
            SceneManager.LoadScene("Pacman");
        }

        // get inputs and move player
        this.yVel -= 9.0f * Time.deltaTime;
        Vector3 movements = new Vector3(Input.GetAxis(HORIZONTAL), yVel, 0);
        movements *= Time.deltaTime * speed;
        if (movements.y < -0.8f) movements.y = -0.8f;
        Vector3 newPos = MapGenerator.clampMovement(transform.position + movements);
        if (this.dead)
            newPos.y = (transform.position + movements).y;
        if ((transform.position+movements).y != newPos.y && this.yVel < 0)
            this.yVel = 0;
        if ((Input.GetAxisRaw(VERTICAL) > 0 || Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.Space)) && yVel == 0)
            this.yVel = 3;

        // trigger events for the block under
        switch (newPos.z)
        {
            case 6:
                MapGenerator.triggerBlock(transform.position+new Vector3(0, -1, 0));
                this.yVel += 10;
                break;
            case 9:
                if (!this.dead)
                {
                    this.yVel += 3;
                    newPos.y += 0.1f;
                }
                this.dead = true;
                break;
            default: break;
        }

        // apply new position
        newPos.z = 0;
        transform.position = newPos;

        // animation stuff
        this.animTime += Time.deltaTime * animSpeed;
        if (animTime > 1) animTime--;

        if (movements.x != 0) {
            this.animType = ANIM_WALK;
            if (movements.x > 0)
                this.mat.mainTexture = texRight;
            else this.mat.mainTexture = texLeft;
        }
        else this.animType = ANIM_IDLE;
        if (this.yVel != 0)
        {
            if (this.yVel > 0) this.animType = ANIM_JUMP;
            else this.animType = ANIM_FALL;
        }

        float offX = (int)(animTime*4) * 0.25f;
        this.mat.mainTextureOffset = new Vector2(offX, animType);
    }
}
