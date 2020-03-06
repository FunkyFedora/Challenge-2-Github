using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrPlayer : MonoBehaviour
{
    private Rigidbody2D rd2d;
    private int scoreValue = 0;
    private int liveValue = 3;
    private bool levelTwo = true;
    private bool facingRight = true;

    public float speed;
    public float hozMovement;
    public float verMovement;
    public static bool gameEnding;

    public Text score;
    public Text message;
    public Text lives;

    public AudioSource audioSource;
    public AudioClip clip;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {        
        rd2d = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        score.text = "Score: " + scoreValue.ToString();
        message.text = "";
        lives.text = "Lives: " + liveValue.ToString();
    }

    void Update()
    {
        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("intState", 1);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("intState", 0);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("intState", 1);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("intState", 0);
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    void FixedUpdate()
    {
        hozMovement = Input.GetAxis("Horizontal");
        verMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
        }

        if (scoreValue == 4 && levelTwo == true)
        {
            transform.position = new Vector2(44.05f, 0f);
            liveValue = 3;
            lives.text = "Lives: " + liveValue.ToString();
            levelTwo = false;
        }

        if (scoreValue == 8)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
            message.text = "You win! Game created by Jenna Ward";

            gameEnding = true;
        }

        if (collision.collider.tag == "Enemy")
        {
            liveValue -= 1;
            lives.text = "Lives: " + liveValue.ToString();
            Destroy(collision.collider.gameObject);
        }

        if (liveValue == 0)
        {
            message.text = "You lose. Game created by Jenna Ward";
            levelTwo = false;

            gameEnding = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            anim.SetBool("jump", false);

            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);

                anim.SetBool("jump", true);
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}
