using UnityEngine;

enum BLOCKS
{
    AIR, GRASS, DIRT, PISTON, SPIKE, PISTON_UP_1, PISTON_UP_2, WOOD, LEAVE
}

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject blockTemplate, mapObject;
    [SerializeField]
    string mapPath;
    [SerializeField]
    Vector3 shift;
    static Sprite[] sprites;
    static MapGenerator instance;
    [SerializeField]
    bool saveMap = true;

    static BLOCKS[][] blocks = new BLOCKS[0][];
    static GameObject[][] gameObjects = new GameObject[0][];
    public static Vector3 clampMovement(Vector3 pos)
    {
        // check for bottom collision
        if (blockAt(pos.x+0.40f, -pos.y+1f) != BLOCKS.AIR || blockAt(pos.x, -pos.y+1f) != BLOCKS.AIR)
            pos.y = Mathf.Ceil(pos.y);

        // check for right collision
        if (blockAt(pos.x+0.8f, -pos.y+0.90f) != BLOCKS.AIR || blockAt(pos.x+0.8f, -pos.y) != BLOCKS.AIR)
            pos.x = Mathf.Floor(pos.x) + 0.22f;

        // check for left collision
        if (blockAt(pos.x, -pos.y+0.90f) != BLOCKS.AIR || blockAt(pos.x, -pos.y) != BLOCKS.AIR)
            pos.x = Mathf.Ceil(pos.x) - 0.02f;

        // special block detection
        pos.z = block2char(blockAt(pos.x+0.5f, -pos.y+1.1f));
        return pos;
    }

    public static void triggerBlock(Vector3 pos)
    {
        if (blockAt(pos.x+0.5f, -pos.y+1.1f) == BLOCKS.PISTON)
        {
            setSprite(pos.x+0.5f, 1, block2char(BLOCKS.PISTON_UP_2));
            setSprite(pos.x+0.5f, 2, block2char(BLOCKS.PISTON_UP_1));
            blocks[1][(int)(pos.x+0.5f)] = BLOCKS.PISTON_UP_2;
            blocks[2][(int)(pos.x+0.5f)] = BLOCKS.PISTON_UP_1;
        }
    }

    private static void setSprite(float x, float y, char spriteIndex)
    {
        if (blocks[(int)y][(int)x] != BLOCKS.AIR)
            gameObjects[(int)y][(int)x].GetComponent<SpriteRenderer>()
            .sprite = sprites[spriteIndex-1];
        else gameObjects[(int)y][(int)x] = createBlock(spriteIndex, new Vector3((int)x, (int)-y, 0), instance);
    }

    static BLOCKS blockAt(float x, float y)
    {
        if (y < 0 || y >= blocks.Length || x < 0 || x >= blocks[(int)y].Length)
            return BLOCKS.AIR;
        return blocks[(int)y][(int)x];
    }

    static GameObject createBlock(char index, Vector3 pos, MapGenerator inst)
    {
        GameObject obj = GameObject.Instantiate(inst.blockTemplate, pos, Quaternion.Euler(0, 0, 0), inst.mapObject.transform);
        obj.GetComponent<SpriteRenderer>().sprite = sprites[index-1];
        return obj;
    }

    static BLOCKS char2block(char nbr)
    {
        switch ((int)nbr)
        {
            case 1:
                return BLOCKS.GRASS;
            case 2:
                return BLOCKS.DIRT;
            case 3:
                return BLOCKS.WOOD;
            case 4:
                return BLOCKS.LEAVE;
            case 6:
                return BLOCKS.PISTON;
            case 7:
                return BLOCKS.PISTON_UP_1;
            case 8:
                return BLOCKS.PISTON_UP_2;
            case 9:
                return BLOCKS.SPIKE;
            default:
            break;
        }
        return BLOCKS.AIR;
    }

    static char block2char(BLOCKS blc)
    {
        switch (blc)
        {
            case BLOCKS.GRASS:
                return (char)1;
            case BLOCKS.DIRT:
                return (char)2;
            case BLOCKS.WOOD:
                return (char)3;
            case BLOCKS.LEAVE:
                return (char)4;
            case BLOCKS.PISTON:
                return (char)6;
            case BLOCKS.PISTON_UP_1:
                return (char)7;
            case BLOCKS.PISTON_UP_2:
                return (char)8;
            case BLOCKS.SPIKE:
                return (char)9;
            default:
            break;
        }
        return (char)0;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (this.saveMap) instance = this;
        // load blocks sprites
        sprites = new Sprite[10];
        sprites[0] = Resources.Load<Sprite>("Sprites/Menu_Grass");
        sprites[1] = Resources.Load<Sprite>("Sprites/Menu_Dirt");
        sprites[2] = Resources.Load<Sprite>("Sprites/Wood");
        sprites[3] = Resources.Load<Sprite>("Sprites/Leave");
        sprites[5] = Resources.Load<Sprite>("Sprites/Piston_Down");
        sprites[6] = Resources.Load<Sprite>("Sprites/Piston_Up_1");
        sprites[7] = Resources.Load<Sprite>("Sprites/Piston_Up_2");
        sprites[8] = Resources.Load<Sprite>("Sprites/Spike");

        // load the map data file
        TextAsset textAsset = Resources.Load<TextAsset>(mapPath);
        string[] mapdata = textAsset.text.Split('\n');
        if (this.saveMap) blocks = new BLOCKS[mapdata.Length][];
        if (this.saveMap) gameObjects = new GameObject[mapdata.Length][];

        // create the game map
        int y = 0;
        foreach (string mapRow in mapdata)
        {
            int x = -1;
            char[] charRow = mapRow.ToCharArray();
            if (this.saveMap) blocks[y] = new BLOCKS[mapRow.Length];
            if (this.saveMap) gameObjects[y] = new GameObject[mapRow.Length];
            Debug.Log("y="+y);
            foreach (char bloc in charRow)
            {
                ++x;
                char nbr = (char) (bloc-'0');
                if (nbr > 65000) continue;
                if (this.saveMap) blocks[y][x] = char2block(nbr);
                if (nbr == 0) continue; // air block, skip
                if (this.saveMap) gameObjects[y][x] = createBlock(nbr, new Vector3(x, -y, 0), this);
                else createBlock(nbr, new Vector3(x, -y, 0), this);
            }
            ++y;
        }

        this.transform.position = shift;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
