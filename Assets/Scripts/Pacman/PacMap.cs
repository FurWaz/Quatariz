using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMap : MonoBehaviour
{
    [SerializeField]
    GameObject mapCube;
    char[][] map;
    static PacMap instance;
    Vector2 center;

    public static void clear()
    {
        for (int i = 0; i < instance.transform.childCount; i++)
            instance.transform.GetChild(i).gameObject.SetActive(false);
        instance.gameObject.GetComponent<AudioSource>().Stop();
    }

    public static int getBlockAt(float x, float y, bool debug = false)
    {
        x += instance.center.x;
        y -= instance.center.y;
        x = -x - 0.5f;
        y = -y + 0.5f;

        if (x < 0 || x >= instance.map.Length)
            return -1;
        if (y < 0 || y >= instance.map[0].Length)
            return -1;
        return instance.map[Mathf.FloorToInt(y)][Mathf.FloorToInt(x)]-'0';
    }

    public static bool isWall(int cube)
    {
        return cube == 1;
    }

    Color nbr2color(int nbr)
    {
        switch (nbr)
        {
            case 1: return new Color(0, 0, 1);
            default: return new Color(0, 0, 0);
        }
    }

    public static void launch()
    {
        // load map from file
        TextAsset textAsset = Resources.Load<TextAsset>("pacMap");
        string[] mapdata = textAsset.text.Split('\n');
        instance.map = new char[mapdata.Length][];
        for (int i = 0; i < mapdata.Length; i++)
            instance.map[i] = mapdata[i].ToCharArray();
        instance.center = new Vector2(-instance.map.Length*0.5f, instance.map[0].Length*0.5f-1f);

        // spawn map graphically
        for (int i = 0; i < instance.map.Length; i++)
        {
            for (int j = 0; j < instance.map[i].Length; j++)
            {
                int nbr = (instance.map[i][j])-'0';
                if (nbr > 10 || nbr < 0) continue;

                // place the entities
                if (nbr == 5) PacMan.setPos(new Vector2(j+instance.center.x, -i+instance.center.y));
                if (nbr == 6) Phantom.blue.setPos(new Vector2(j+instance.center.x, -i+instance.center.y));
                if (nbr == 7) Phantom.pink.setPos(new Vector2(j+instance.center.x, -i+instance.center.y));
                if (nbr == 8) Phantom.orange.setPos(new Vector2(j+instance.center.x, -i+instance.center.y));
                if (nbr == 9) Phantom.red.setPos(new Vector2(j+instance.center.x, -i+instance.center.y));

                Color col = instance.nbr2color(nbr);
                GameObject.Instantiate(
                    instance.mapCube,
                    new Vector3(j+instance.center.x, -i+instance.center.y, 1f), Quaternion.Euler(90, 0, 180),
                    instance.transform
                ).GetComponent<MeshRenderer>().material.color = col;
            }
        }

        Phantom.pink.setReady(true);
        Phantom.blue.setReady(true);
        Phantom.orange.setReady(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
