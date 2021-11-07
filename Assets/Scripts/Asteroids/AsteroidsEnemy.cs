using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsEnemy : MonoBehaviour
{
    [SerializeField]
    float speed = 1;
    float direction;
    float orientation = 0;
    float rotSpeed = 0;
    Vector3 position;
    Vector3 movements;
    // Start is called before the first frame update
    void Start()
    {
        this.direction = transform.rotation.eulerAngles.z * 3.141592f / 180f;
        this.movements = new Vector3(
            - Mathf.Cos(this.direction),
            - Mathf.Sin(this.direction), 0
        );
        this.orientation = Random.Range(0f, 359.9f);
        this.rotSpeed = Random.Range(-10f, 10f);
        this.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // update position
        this.position += this.movements * this.speed * Time.deltaTime;
        this.orientation += this.rotSpeed * Time.deltaTime;
        transform.position = this.position;
        transform.rotation = Quaternion.Euler(0, 0, this.orientation);

        // kill if touched by bullet
        foreach (GameObject bullet in AsteroidsPlayer.bullets)
        {
            if (bullet == null) continue;
            float dx = bullet.transform.position.x - transform.position.x;
            float dy = bullet.transform.position.y - transform.position.y;
            float dist = Mathf.Sqrt(dx*dx + dy*dy);

            if (dist < transform.localScale.x + bullet.transform.localScale.x * 10f) // touched
            {
                // delete the bullet and the ennemy
                if (gameObject != null) Destroy(gameObject, 0.1f);
                AsteroidsPlayer.bullets.Remove(bullet);
                if (bullet != null) Destroy(bullet, 0.1f);

                AstreoidsManager.increaseScore((int)(transform.localScale.x * 100f));
                if (transform.localScale.x > 0.22)
                    for (int i = 0; i < 3; i++)
                    {
                        GameObject go = GameObject.Instantiate(
                            gameObject, transform.position, Quaternion.Euler(
                                0, 0, transform.rotation.eulerAngles.z + Random.Range(0f, 359.9f)
                            ), transform.parent
                        );
                        go.transform.localScale = new Vector3(
                            transform.localScale.x - 0.1f,
                            transform.localScale.x - 0.1f,
                            transform.localScale.x - 0.1f
                        );
                        if (i == 0) go.GetComponent<AsteroidsEnemy>().playSound();
                    }
                return;
            }
        }

        // if touch player, kill player
        float m_dx = AsteroidsPlayer.getPos().x - transform.position.x;
        float m_dy = AsteroidsPlayer.getPos().y - transform.position.y;
        float m_dist = Mathf.Sqrt(m_dx*m_dx + m_dy*m_dy);
        if (m_dist < transform.localScale.x + 0.2) // touched
            AsteroidsPlayer.makeDie();

        // kill if out of screen
        if (this.position.x < -7f || this.position.x > 7f ||
            this.position.y < -10f || this.position.y > 10f)
            if (gameObject != null) Destroy(gameObject, 1);
    }

    void playSound()
    {
        gameObject.GetComponent<AudioSource>().Play();
    }
}
