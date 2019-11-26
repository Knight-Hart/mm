using UnityEngine;
using System.Collections;

public class WalkingBallAI : MonoBehaviour {

	public float alertDistance = 5f;
	public int health = 5;
	public float attackMin = 1f;
	public float attackMax = 4f;
	public GameObject explosion;

	private Animator anim;
	private Transform player;
	private bool enemyActive = false;
	private bool enemyMoving = false;
	private bool facingRight = true;
	public float movingTime;
	private float explosionTime = 0f;
	private Animator explosionAnim = null;

	public float xOrigin;			
	private float distance = 10f;	
	private BoxCollider2D hitBox;
	public float speed = 1f;
	public int move = 0;


	void Start () {
		hitBox = GetComponent<BoxCollider2D> ();
		hitBox.size = new Vector2 (0.2f, 0.2f); 	


		anim = GetComponent<Animator> ();
		explosionAnim = explosion.GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		move = -1;	
		facingRight = true;
		distance = 10f;
	}

	void FixedUpdate(){
		if(enemyMoving){
			Movement();
		}
	}

	void Update () {
		if(!enemyActive){
			try{
				distance = Vector2.Distance((Vector2)transform.position, player.position);
			} catch {
				player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
				distance = Vector2.Distance((Vector2)transform.position, player.position);
			}
			if(distance < 2.5f){
				enemyActive = true;
				xOrigin = transform.position.x;
				anim.SetBool("active", true);
				movingTime = Time.time + 1.75f;
			}
		}

		if((Time.time >= movingTime) && enemyActive){
			enemyMoving = true;

		} else if((Time.time >= movingTime-0.75f) && enemyActive){
			hitBox.size = Vector2.Lerp(hitBox.size, new Vector2(0.2f,0.5f), 5f * Time.deltaTime);
		}
		if(health <= 0){
			explosion.GetComponent<Animator>().SetBool("explode", true);
			anim.speed = 0;			
			speed = 0;				
			enemyActive = false;	
			hitBox.enabled = false;
			DeathDriver();			
			Destroy(this.gameObject, 0.75f);
		}
	}

	void Movement(){
		if(xOrigin - transform.position.x > 2.5f){
			move = 1;
		} else if (xOrigin - transform.position.x < -2.5f){
			move = -1;
		}
		GetComponent<Rigidbody2D>().velocity = new Vector2 (move * speed, 0);

		if(move > 0 && !facingRight){
			Flip();
		} else if(move < 0 && facingRight){
			Flip();
		}
	}

	void Flip(){
		facingRight = !facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

	void DeathDriver(){
		if(Time.time >= explosionTime){
			float randX = Random.Range(-0.25f, 0.25f);
			float randY = Random.Range(-0.25f, 0.25f);
			explosion.transform.position = 
				new Vector3(transform.position.x + randX, transform.position.y + randY);
			explosionTime = Time.time + 0.9f;
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "ChargedBeam")
			health -= 2;
		else if(col.gameObject.layer == 10){
			health--;
			
		}
	}

}
