using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MegaManController : MonoBehaviour
{

    public GameObject buster;
    public GameObject charged_buster;
    public Transform groundCheck;
    public bool grounded = false;
    public bool facingRight = true;
    public float groundRadius = 0.2f;
    public float jumpForce;
    public float move;
    public float speed = 0f;
    public int health = 8;
    public LayerMask whatIsGround;	

    public bool stunMovement = false;
    public float deathTime;

    private int ENEMY_LAYER = 12;
    private int RESPAWN_LAYER = 13;
    private Animator anim;
    private AnimatorStateInfo stateinfo;
    private SoundController sound;
    private bool charging;
    private float chargedTime;
    private float fireModeTime;
    private float stunMovementTime;
    private bool deathInit = false;
    private bool init = false;
    private float busterPos;
    private Transform healthBar;

    public Scene sceneLoaded;

    void Start()
    {
        anim = GetComponent<Animator>();
        sound = GameObject.Find("GlobalScripts").GetComponent<SoundController>();
        speed = 3f;
        jumpForce = 400f;
        charging = false;
        healthBar = GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<Transform>();
        sceneLoaded = SceneManager.GetActiveScene();
        UpdateHealthBar();
    }

    void FixedUpdate()
    {

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("Grounded", grounded);
        anim.SetFloat("vSpeed", GetComponent<Rigidbody2D>().velocity.y);

        if (!stunMovement)
            move = Input.GetAxis("Horizontal");
        else
            move = 0;

        anim.SetFloat("Speed", Mathf.Abs(move));
        GetComponent<Rigidbody2D>().velocity = new Vector2(move * speed, GetComponent<Rigidbody2D>().velocity.y);

        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }
    }

    void Update()
    {

        if (!stunMovement)
            checkInputs();

        if (fireModeTime <= Time.time)
        {
            fireModeTime = 0f;
            anim.SetBool("FireMode", false);
        }

        if (health <= 0 && !deathInit)
        {
            anim.SetTrigger("Death");
            deathInit = true;
            deathTime = stunMovementTime = Time.time + 2f;
            stunMovement = true;
        }

        if (deathInit && deathTime <= Time.time)
            GameObject.Find("GlobalScripts").GetComponent<GameGod>().CallGodToRespawn();

        if (!init)
        {
            schedulePlay(sound.mm_spawn);
            anim.SetBool("Spawning", true);

            stunMovementTime = Time.time + 1.5f;
            stunMovement = true;
            init = true;
            UpdateHealthBar();
        }

        if (Time.time >= stunMovementTime)
        {
            anim.SetBool("Spawning", false);
            stunMovement = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == ENEMY_LAYER)
        {
            if (Time.time >= stunMovementTime)
            {
                Damage();
            }
        }
        else if (col.gameObject.layer == RESPAWN_LAYER)
        {
            health = 0;
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == ENEMY_LAYER)
        {
            if (Time.time >= stunMovementTime)
            {
                Damage();
            }

        }

        if(col.gameObject.CompareTag("goal"))
        {
            SceneManager.LoadScene(sceneLoaded.buildIndex + 1);
        }
    }
    void Damage()
    {
        anim.SetTrigger("Damaged");
        stunMovement = true;
        stunMovementTime = Time.time + 0.5f;
        health--;
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        try
        {
            healthBar.localScale = new Vector3(1, health, 1);
        }
        catch
        {
            healthBar = GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<Transform>();
            healthBar.localScale = new Vector3(1, health, 1);
        }
    }

    public void schedulePlay(AudioClip sound)
    {
        GetComponent<AudioSource>().clip = sound;
        GetComponent<AudioSource>().Play();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void checkInputs()
    {
        if (grounded && (Input.GetKeyDown(KeyCode.Z)))
        {
            schedulePlay(sound.mm_jump);
            anim.SetBool("Grounded", false);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            schedulePlay(sound.mm_burst);
            anim.SetBool("FireMode", true);
            anim.SetTrigger("Fire");
            fireModeTime = Time.time + 0.5f;

            busterPos = facingRight ? 0.3f : -0.3f;
            Instantiate(buster, new Vector3(transform.position.x + busterPos, transform.position.y + 0.04f, 0f), Quaternion.identity);
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            charging = false;
            if (chargedTime > 1.5f)
            {
                schedulePlay(sound.mm_charged_burst);
                Instantiate(charged_buster, new Vector3(transform.position.x + busterPos, transform.position.y + 0.04f, 0f), Quaternion.identity);
                anim.SetBool("FireMode", true);
                anim.SetTrigger("Fire");
            }
        }

        if (Input.GetKey(KeyCode.X))
        {
            if (!charging)
            {
                charging = true;
                chargedTime = 0;
            }
            else
            {
                fireModeTime = Time.time + 0.5f;
                chargedTime += Time.deltaTime;
            }
        }
    }
}