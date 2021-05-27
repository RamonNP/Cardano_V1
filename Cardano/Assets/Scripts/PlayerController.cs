using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
 [RequireComponent(typeof(Rigidbody2D))]
 [RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    public bool lookLeft;

[HideInInspector] public Player player;
    public Animator playerAnimator;
    float input_x = 0;
    float input_y = 0;
    bool isWalking = false;
 
    Rigidbody2D rb2D;
    Vector2 movement = Vector2.zero;
    [Header("Interact")]
    public KeyCode interactKey = KeyCode.E;
    bool canTeleport = false;
    Region tmpRegion;
 
    // Start is called before the first frame update
    void Start()
    {
        isWalking = false; 
        rb2D = GetComponent<Rigidbody2D>(); 
        player = GetComponent<Player>();
    }
 
    // Update is called once per frame
    void Update()
    {
        input_x = Input.GetAxisRaw("Horizontal");
        input_y = Input.GetAxisRaw("Vertical");
        isWalking = (input_x != 0 || input_y != 0);
        movement = new Vector2(input_x, input_y);
        /*
        if (isWalking)
        {
            playerAnimator.SetFloat("input_x", input_x);
            playerAnimator.SetFloat("input_y", input_y);
        }*/ 
        VerificaFlip();
        playerAnimator.SetBool("isWalking", isWalking);
 
        if (player.entity.attackTimer < 0)
            player.entity.attackTimer = 0;
        else
            player.entity.attackTimer -= Time.deltaTime;
 
        if(player.entity.attackTimer == 0 && !isWalking)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                playerAnimator.SetTrigger("attack");
                player.entity.attackTimer = player.entity.cooldown;
 
                Attack();
            }
        }    
        if(canTeleport && tmpRegion != null && Input.GetKeyDown(interactKey)) 
        {
            this.transform.position = tmpRegion.warpLocation.position;
        }     
    }
    
    private void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + movement * player.entity.speed * Time.fixedDeltaTime);
    }
 
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Enemy")
        {
            player.entity.target = collider.transform.gameObject;
        }
 
        if(collider.tag == "Teleport")
        {
            tmpRegion = collider.GetComponent<Teleport>().region;
            canTeleport = true;
        }
    }
 
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
        {
            player.entity.target = null;
        }
 
        if (collider.tag == "Teleport")
        {
            tmpRegion = null;
            canTeleport = false;
        }
    }
 
    void Attack()
    {
        if (player.entity.target == null)
            return;
 
        Monster monster = player.entity.target.GetComponent<Monster>();
 
        if (monster.entity.dead)
        {
            player.entity.target = null;
            return;
        }
 
        float distance = Vector2.Distance(transform.position, player.entity.target.transform.position);
 
        if(distance <= player.entity.attackDistance)
        {
            int dmg = player.manager.CalculateDamage(player.entity, player.entity.damage);
            int enemyDef = player.manager.CalculateDefense(monster.entity, monster.entity.defense);
            int result = dmg - enemyDef;
 
            if (result < 0)
                result = 0;
 
            Debug.Log("Player dmg: " + result.ToString());
            monster.entity.currentHealth -= result;
            monster.entity.target = this.gameObject;
        }
    }

    private void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        lookLeft = !lookLeft;
    }
    public void VerificaFlip() {
         if (isWalking)
        {
            //playerAnimator.SetFloat("input_x", input_x);
            //playerAnimator.SetFloat("input_y", input_y);
            if (input_x > 0 && lookLeft == true)
            {
                Flip();
            } else if(input_x < 0 && lookLeft == false)
            {
                Flip();
            }
        }
    }
}