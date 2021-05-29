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
    private bool lastWalking;
 
    // Start is called before the first frame update
    void Start()
    {
        //PlayerController new_player = Instantiate (NetworkManager.instance.playerPrefab, new Vector3 (0, 0, 0), Quaternion.identity).GetComponent<PlayerController> ();
        isWalking = false; 
        rb2D = GetComponent<Rigidbody2D>(); 
        player = GetComponent<Player>();
    }
 
    // Update is called once per frame
    void Update()
    {
        if(player.entity.isLocalPlayer) {

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
            VerificaFlip(input_x);
            playerAnimator.SetBool("isWalking", isWalking);
            if(isWalking && (lastWalking != isWalking)){
                NetworkManager.instance.EmitAnimation ("isWalking", GetComponent<Player>().entity.id);
                lastWalking = isWalking;
            } else if (lastWalking != isWalking){
                NetworkManager.instance.EmitAnimation ("Idle", GetComponent<Player>().entity.id);
                lastWalking = isWalking;
            }
    
            if (player.entity.attackTimer < 0)
                player.entity.attackTimer = 0;
            else
                player.entity.attackTimer -= Time.deltaTime;
    
            if(player.entity.attackTimer == 0 && !isWalking)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    playerAnimator.SetTrigger("attack");
                    NetworkManager.instance.EmitAnimation ("attack", GetComponent<Player>().entity.id);
                    player.entity.attackTimer = player.entity.cooldown;
    
                    Attack();
                }
            }    
            if(canTeleport && tmpRegion != null && Input.GetKeyDown(interactKey)) 
            {
                this.transform.position = tmpRegion.warpLocation.position;
            }     
        }
    }
    public void UpdateAnimator(string animation){
        switch (animation)
        {
            case "attack":
                playerAnimator.SetTrigger("attack");
                break;
            case "isWalking":
                isWalking = true;
                playerAnimator.SetBool("isWalking", true);
                break;
            case "Idle":
                playerAnimator.SetBool("isWalking", false);
                isWalking = false;
                break;
        }
    }
    public void UpdateStatustoServer()
	{
		NetworkManager.instance.EmitPosAndRot (transform.position, transform.rotation, input_x);

		//NetworkManager.instance.EmitAnimation ("IsWalk");


	}


	public void UpdatePosAndRot(Vector3 _pos, Quaternion _rot, float direction)
	{
		transform.position = _pos;
		transform.rotation = _rot;

        Vector3 theScale = transform.localScale;
        if(direction < 0 && theScale.x > 0) {
            Flip();
        } else  if(direction > 0 && theScale.x < 0) {
            Flip();
        }

	}
    private void FixedUpdate()
    {
        if(player.entity.isLocalPlayer) {
            rb2D.MovePosition(rb2D.position + movement * player.entity.speed * Time.fixedDeltaTime);
            UpdateStatustoServer();
        }
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
        //NetworkManager.instance.SendPingToServer();
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
    public void VerificaFlip(float x) {


         if (isWalking)
        {
            //playerAnimator.SetFloat("input_x", input_x);
            //playerAnimator.SetFloat("input_y", input_y);
            if (x > 0 && lookLeft == true)
            {
                Flip();
                //NetworkManager.instance.EmitAnimation ("Flip", x.ToString(), GetComponent<Player>().entity.id);
            } else if(x < 0 && lookLeft == false)
            {
                Flip();
                //NetworkManager.instance.EmitAnimation ("Flip", x.ToString(), GetComponent<Player>().entity.id);
            }
        }
    }
}