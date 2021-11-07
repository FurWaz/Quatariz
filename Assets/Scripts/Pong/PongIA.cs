using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongIA : MonoBehaviour
{
    [SerializeField]
    float maxY, minY;
    [SerializeField]
    GameObject ball;
    float speed = 4.5f;
    float fadetime = 0;
    Material mat;
    // Start is called before the first frame update
    void Start()
    {
        this.mat = GetComponent<MeshRenderer>().material;
        this.mat.color = new Color(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.fadetime < 3)
        {
            if (this.fadetime > 1)
            {
                float col = (this.fadetime-1)*0.5f + 30f/255f;
                this.mat.color = new Color(col, col, col);
            }
            this.fadetime += Time.deltaTime;
        }
        Vector3 pos = transform.position;
        Vector3 bpos = ball.transform.position;
        Vector3 newPos = pos;
        float factor = 0f;

        float dist2ball = Mathf.Abs(bpos.x - pos.x);
        if (dist2ball < 5f) factor = (5f - dist2ball) / 5f;

        if (bpos.y > pos.y && pos.y < maxY)
            newPos.y += this.speed * Time.deltaTime * factor;
        if (bpos.y < pos.y && pos.y > minY)
            newPos.y -= this.speed * Time.deltaTime * factor;

        if (dist2ball < transform.localScale.x * 2f) // check for plank hit
        {
            float distY = Mathf.Abs(pos.y - bpos.y);
            Debug.Log("checking for collision: dist = "+distY);
            // hit plank, change X direction
            if (distY < (ball.transform.localScale.y+transform.localScale.y)*8)
            {
                Debug.Log("collision !!");
                PongBall pb = ball.GetComponent<PongBall>();
                // if ball in our direction, inverse it
                if (pb.getDirectionX() * pos.x > 0)
                    pb.changeX();
            }
        }

        transform.position = newPos;
    }
}
