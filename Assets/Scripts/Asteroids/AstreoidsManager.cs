using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstreoidsManager : MonoBehaviour
{
    [SerializeField]
    GameObject enemiesParent, templateEnemy;
    [SerializeField]
    TMPro.TextMeshProUGUI scoreText;
    float spawnDelay = 1;
    float timeout = 0;
    float backTime = 0;
    public int levelNbr = 0;
    public static bool running = false;
    Color backColor = new Color(109/255f, 166/255f, 1);

    int score = 0;
    static AstreoidsManager instance;

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
        if (!existsLevel()) return 0;
        string text = System.IO.File.ReadAllText(Application.persistentDataPath+"/level.txt");
        return System.Int32.Parse(text);
    }

    public static void increaseScore(int amount)
    {
        instance.score += amount;
        instance.scoreText.text = instance.score.ToString();
        if (instance.levelNbr == 1 && instance.score >= 2000)
            AsteroidsPlayer.makeWin();
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        running = true;
        backTime = 0;
        Camera.main.backgroundColor = backColor;
        levelNbr = readLevel();
        writeLevel(2);
    }

    // Update is called once per frame
    void Update()
    {
        if (backTime < 1)
        {
            Camera.main.backgroundColor = backColor * (1f - backTime);
            backTime += Time.deltaTime;
            if (backTime <= 0)
                Camera.main.backgroundColor = new Color(0, 0, 0);
        }

        if (!running)
        {
            for (int i = 0; i < enemiesParent.transform.childCount; i++)
            {
                GameObject child = enemiesParent.transform.GetChild(i).gameObject;
                if (child != null) Destroy(child);
            }
            score = 0; increaseScore(0);
            return;
        }

        timeout += Time.deltaTime;
        if (timeout > spawnDelay)
        {
            timeout = 0;
            // get random position
            int side = Random.Range(1, 4);
            float random = Random.Range(-1.0f, 1.0f);
            Vector3 pos = new Vector3(1f, 1f, 0f);
            switch (side)
            {
                case 0: pos = new Vector3(-1f, random); break;
                case 1: pos = new Vector3(1f, random); break;
                case 2: pos = new Vector3(random, -1f); break;
                case 3: pos = new Vector3(random, 1f); break;
            }
            pos.x *= 6.5f;
            pos.y *= 5f;

            float angle = Mathf.Atan2(pos.y, pos.x) + Random.Range(-0.4f, 0.4f);

            GameObject.Instantiate(
                templateEnemy, pos, Quaternion.Euler(0, 0, angle * 180 / 3.1415926f),
                enemiesParent.transform
            );
        }
    }
}
