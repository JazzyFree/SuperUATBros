using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {
    public float speed;
    Rigidbody2D rb2d;
    public float jumpForce;
    float horizontal = 0f;
    SpriteRenderer spriteRenderer;
    Animator anim;
    public Text messageText;
    public float attackDelay;
    float lastAttackTime;
    public GameObject laserBeam;
    private float health;
    public float maxHealth;
    public int lives;
    public Text livesText;
    public GameObject gameOverPanel;
    public Slider healthSlider;

    public Transform laserPsRT;
    public Transform laserposLT;

        public bool facingRight;

    public RuntimeAnimatorController unarmedController;
    public RuntimeAnimatorController swordController;
    public RuntimeAnimatorController gunController;

    public Transform currentCheckpoint;
    public bool disabled = false;


    // Use this for initialization
    void Start () {// serach for the rigidbody
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        facingRight = true;
        health = maxHealth;
        livesText.text = "Lives:" + lives;
	}

	
	// Update is called once per frame
	void Update () {

        if (disabled)
        {
            return;
        }
        
        // player to move let and right 
        //Help Give the player air control for jummping
       
        
        if(Mathf.Abs(rb2d.velocity.y) < .01)
        {
            horizontal = Input.GetAxis("Horizontal");
        }


        // Moves the player left and right with thw arrows 
        transform.Translate(Vector2.right * Time.deltaTime * speed * horizontal);
       // Flip the sprite to face the direction he is moving
       if(horizontal > .01)
        {
            spriteRenderer.flipX = false;
            facingRight = true;
        }
       if(horizontal < -.01)
        {
            spriteRenderer.flipX = true;
            facingRight = false;

        }

        //Set the spped parameter of the ainmtor 
        anim.SetFloat("speed", Mathf.Abs(horizontal));


        // the player jump and do not double jump
        if (Input.GetButtonDown("Jump") && Mathf.Abs(rb2d.velocity.y) < .01)
        {
            //Add aforce on the y axis to make the player jump
            rb2d.AddRelativeForce(Vector2.up * jumpForce);
        }

        if(!anim.GetBool("Jumping")&& Mathf.Abs(rb2d.velocity.y) > .01)
        {
            anim.SetBool("Jumping", true);
        }
        
        if(anim.GetBool("Jumping")&& Mathf.Abs(rb2d.velocity.y) < .01)
        {
            anim.SetBool("Jumping", false);
            lastAttackTime = Time.time;
        }

        if (Input.GetButtonDown("Fire1")&& Time.time > lastAttackTime + attackDelay)
        {
            anim.SetTrigger("attack");
        }

        if (Input.GetKeyDown("u"))
        {
            //switch to unarmed controller 
            anim.runtimeAnimatorController = unarmedController as RuntimeAnimatorController;
        }

        if (Input.GetKeyDown("g"))
        {
            //switch to Gun controller 
            anim.runtimeAnimatorController = gunController  as RuntimeAnimatorController;
        }
        if (Input.GetKeyDown("m"))
        {
            //switch to sword  controller 
            anim.runtimeAnimatorController = swordController  as RuntimeAnimatorController;
        }

    }

    void UnarmedAttack()
    {
        DisplayMessage("I am Unarmed and can't attack...", 2.0f);
    }
    void DisplayMessage(string message, float displayTime)
    {
        messageText.text = message;
        Invoke("ClearMessage", displayTime);
    }

    void ClearMessage()
    {
        messageText.text = "";
    }

    void LaserAttack()
    {
        if (facingRight)
        {
            Instantiate(laserBeam, laserPsRT.position, transform.rotation);
        }
        else
        {
            Instantiate(laserBeam, laserposLT.position, Quaternion.Euler(0f,0f,180f));
        }
        
    }

    void Respawn()
    {
        health = maxHealth;
        healthSlider.value = 1f;
        transform.position = currentCheckpoint.position;
        spriteRenderer.enabled = true;
        disabled = false;
    }
      void OnCollisionEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            health -= 5;
            //check for death
            if(health <= 0)
            {
                lives--;
                //check to see we are out of lives
                if(lives <= 0)
                {
                    //Gameover
                    gameOverPanel.SetActive(true);

                }
                else
                {
                    
                    Invoke("Respawn", 1f);
                }
                health = 0;
                livesText.text = "Lives:" + lives;
                spriteRenderer.enabled = false;
                disabled = true;

            }
            healthSlider.value = health / maxHealth;
        }
    }
    // this function will run whenever the player collides with a trigger colldier 
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }


    public void PlayAgain()
    {
        SceneManager.LoadScene("Gameplay");
    }





     void OntriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("checkPoint"))
        {
            currentCheckpoint = other.transform;
        }
    }
}
