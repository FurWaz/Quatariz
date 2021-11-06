using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongIA : MonoBehaviour
{
    [SerializeField]
    float maxY, minY;
    [SerializeField]
    GameObject ball;
    float speed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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

        if (dist2ball < transform.localScale.x * 0.5f) // check for plank hit
        {
            float distY = Mathf.Abs(pos.y - bpos.y);
            // hit plank, change X direction
            if (distY < (ball.transform.localScale.y+transform.localScale.y)*5)
            {
                PongBall pb = ball.GetComponent<PongBall>();
                // if ball in our direction, inverse it
                if (pb.getDirectionX() * pos.x > 0)
                    pb.changeX();
            }
        }

        transform.position = newPos;
    }
}
