using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
 [RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    public bool lookLeft;
    public Sprite[] HelmetSprites;

    //[HideInInspector] 
    [Header("Atalhos HUD")]
    public GameObject inventory;
    [Header("Cinemachine Camera Shake")]
    public CinemachineVirtualCamera VirtualCamera;
    public CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    public float ShakeDuration = 0.3f;          // Time the Camera Shake effect will last
    public float ShakeAmplitude = 3.2f;         // Cinemachine Noise Profile Parameter
    public float ShakeFrequency = 3.0f;         // Cinemachine Noise Profile Parameter

    internal void usarItemArma(int idItem)
    {
        print("Implementar troca de arma ID:"+idItem);
    }

    public float ShakeElapsedTime = 0f;
    [Header("Player")]
    public Player player;
    public GameObject PersonagemFlip;
    public Animator playerAnimator;
    float input_x = 0;
    float input_y = 0;
    public bool isWalking = false;
 
    Rigidbody2D rb2D;
    Vector2 movement = Vector2.zero;
    [Header("Interact")]
    public KeyCode interactKey = KeyCode.E;
    public KeyCode interactKeyI = KeyCode.I;
    public KeyCode interactKeySpace = KeyCode.Space;
    bool canTeleport = false;
    Region tmpRegion;
    private bool lastWalking;
 
    // Start is called before the first frame update
    void Start()
    {
        HelmetSprites = Resources.LoadAll<Sprite>("Helmet") ;
        //PlayerController new_player = Instantiate (NetworkManager.instance.playerPrefab, new Vector3 (0, 0, 0), Quaternion.identity).GetComponent<PlayerController> ();
        isWalking = false; 
        rb2D = GetComponent<Rigidbody2D>(); 
        player = GetComponent<Player>();
        // Get Virtual Camera Noise Profile
        if (VirtualCamera != null){
            virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>(); 
        }
        print(VirtualCamera);
    }
 
    // Update is called once per frame
    void Update()
    {
        if(player.entity.isLocalPlayer) {
            cameraShake();
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
    
            if (player.entity.attackTimer <= 0)
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
            if(Input.GetKeyDown(interactKeySpace)) {
                if(player.itemColetavel != null){

                    //GameObject obj = Instantiate (player.itemColetavel, new Vector3(0,0,0),Quaternion.identity);
                    
                    //sasa
                    print("ALTERAR CAMINHO FIXO");
                    this.GetComponent<Inventario>().itemInventario.Add(player.itemColetavel.GetComponent<Item>().pathItem);
                    Destroy(player.itemColetavel);
                    player.itemColetavel = null;
                }

            }
            if(canTeleport && tmpRegion != null && Input.GetKeyDown(interactKey)) 
            {
                this.transform.position = tmpRegion.warpLocation.position;
            }     
            if(Input.GetKeyDown(interactKeyI) && this.GetComponent<Inventario>().inventario.active == false) 
            {
                //print("Abrir Inventario");
                this.GetComponent<Inventario>().inventario.SetActive(true);
                this.GetComponent<Inventario>().carregarInventario();
            } else if(Input.GetKeyDown(interactKeyI) && this.GetComponent<Inventario>().inventario.active == true){
                this.GetComponent<Inventario>().limparItensCarregados();
                this.GetComponent<Inventario>().inventario.SetActive(false);

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
        if (collider.tag == "Item")
        {
            player.itemColetavel = collider.gameObject;
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
        if (collider.tag == "Item")
        {
            player.itemColetavel = null;
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
        Vector3 theScale = PersonagemFlip.transform.localScale;
        theScale.x *= -1;
        PersonagemFlip.transform.localScale = theScale;
        lookLeft = !lookLeft;
    }

    public void ShakeShoot()
    {
        ShakeElapsedTime = ShakeDuration;
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
     public void cameraShake() {
         // If the Cinemachine componet is not set, avoid update
        if (VirtualCamera != null && virtualCameraNoise != null)
        {
            // If Camera Shake effect is still playing
            if (ShakeElapsedTime > 0)
            {
                // Set Cinemachine Camera Noise parameters
                virtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
                virtualCameraNoise.m_FrequencyGain = ShakeFrequency;

                // Update Shake Timer
                ShakeElapsedTime -= Time.deltaTime;
            }
            else
            {
                // If Camera Shake effect is over, reset variables
                virtualCameraNoise.m_AmplitudeGain = 0f;
                ShakeElapsedTime = 0f;
            }
        }
    }
    public void OpenInventory() {
        //inventory.SetActive(true);
        if(inventory.active == false) 
        {
            //print("Abrir Inventario");
            this.GetComponent<Inventario>().inventario.SetActive(true);
            this.GetComponent<Inventario>().carregarInventario();
        } else if(inventory.active == true){
            this.GetComponent<Inventario>().limparItensCarregados();
            this.GetComponent<Inventario>().inventario.SetActive(false);

        } 
    }
}