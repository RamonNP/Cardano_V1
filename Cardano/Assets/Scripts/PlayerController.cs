using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.UI;

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
    public GameObject targetAim;
    public Transform spawnArraw;
    public GameObject prefabArraw;
    public float speedArraw;
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
    public Vector3 posMouse;
    public Vector3 playPosition;
    private double angle;
    public InventoryV2 inventoryV2;
    public UI_InventoryV2 uI_InventoryV2;
    private PlayerEquipController playerEquipController;
    public GameObject equip;

    [SerializeField] private CharacterEquipment characterEquipment;

    // Start is called before the first frame update
    void Start()
    {
        characterEquipment = GetComponent<CharacterEquipment>();
        NetworkManager.instance.uiCharacterEquipment.SetCharacterEquipment(characterEquipment);
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
        uI_InventoryV2 = NetworkManager.instance.uI_InventoryV2;
        inventoryV2 = new InventoryV2(UseItem, 15);
        uI_InventoryV2.SetInventory(inventoryV2);
        playerEquipController = NetworkManager.instance.playerLocalInstance.GetComponent<PlayerEquipController>();
        equip = NetworkManager.instance.equip;


        print("INSTANTCIO");
        ItensEntity itemJson;
        NetworkManager.instance.itensDatabase.TryGetValue(1, out itemJson);
        ItemWorldV2.SpawnItemWorld(new Vector3(1.7f, 1.2f), ItemV2.NewItemv2FromItemJson(itemJson));
        NetworkManager.instance.itensDatabase.TryGetValue(2, out itemJson);
        ItemWorldV2.SpawnItemWorld(new Vector3(1.7f, 0.9f), ItemV2.NewItemv2FromItemJson(itemJson));
        NetworkManager.instance.itensDatabase.TryGetValue(3, out itemJson);
        ItemWorldV2.SpawnItemWorld(new Vector3(1.7f, 0.3f), ItemV2.NewItemv2FromItemJson(itemJson));
        NetworkManager.instance.itensDatabase.TryGetValue(4, out itemJson);
        ItemWorldV2.SpawnItemWorld(new Vector3(1.7f, 0.3f), ItemV2.NewItemv2FromItemJson(itemJson));
        NetworkManager.instance.itensDatabase.TryGetValue(4, out itemJson);
        ItemWorldV2.SpawnItemWorld(new Vector3(1.7f, 0.3f), ItemV2.NewItemv2FromItemJson(itemJson));
        NetworkManager.instance.itensDatabase.TryGetValue(4, out itemJson);
        ItemWorldV2.SpawnItemWorld(new Vector3(1.7f, 0.3f), ItemV2.NewItemv2FromItemJson(itemJson));
    }
    private void UseItem(ItemV2 item) {
        print("USANDO ITEM"+item.itemType);
        ItensEntity itemJson;
        NetworkManager.instance.itensDatabase.TryGetValue(item.idItem, out itemJson);
        int defense = itemJson.defense;
        int atack = itemJson.atack;
        Sprite sprite = Resources.Load<Sprite>(itemJson.pathSprite);
        switch (item.itemType)
            {
                case ItemType.HELMET : 
                    Item itemhelmet = playerEquipController.helmetPlayerEquipe.GetComponent<Item>();
                    if(item.idItem == itemhelmet.idItem) {
                        print("DESEQUIPAR O ITEM ANTES DE USAR");
                        return;
                    }
                    print(" Este item " + item.idItem + " foi utilizado ");
                    playerEquipController.helmetPlayerEquipe.SetActive(true);
                    playerEquipController.helmetPlayerInventario.SetActive(true);
                    itemhelmet.idItem = item.idItem;
                    itemhelmet.defense = defense;
                    itemhelmet.atack = atack;
                    itemhelmet.itemType = ItemType.HELMET;
                    playerEquipController.helmetPlayerEquipe.GetComponent<SpriteRenderer>().sprite = sprite;
                    playerEquipController.helmetPlayerInventario.GetComponent<Image>().sprite = sprite;
                break;
                case ItemType.SWORD :
                    Item itemWeapom = playerEquipController.helmetPlayerEquipe.GetComponent<Item>(); 
                    print(" Este item " + item.idItem  + " foi utilizado ");
                    playerEquipController.weapomPlayerEquipe.SetActive(true);
                    playerEquipController.weapomPlayerInventario.SetActive(true);
                    itemWeapom.idItem = item.idItem;
                    itemWeapom.defense = defense;
                    itemWeapom.atack = atack;
                    itemWeapom.itemType = ItemType.SWORD;
                    playerEquipController.weapomPlayerEquipe.GetComponent<SpriteRenderer>().sprite = sprite;
                    playerEquipController.weapomPlayerInventario.GetComponent<Image>().sprite = sprite;
                break;
                case ItemType.BEAST : 
                    print(" Este item " + item.idItem  + " foi utilizado ");
                    playerEquipController.weapomPlayerEquipe.SetActive(true);
                    playerEquipController.weapomPlayerInventario.SetActive(true);
                    playerEquipController.weapomPlayerEquipe.GetComponent<Item>().idItem = item.idItem;
                    playerEquipController.weapomPlayerEquipe.GetComponent<Item>().defense = defense;
                    playerEquipController.weapomPlayerEquipe.GetComponent<Item>().atack = atack;
                    playerEquipController.weapomPlayerEquipe.GetComponent<Item>().itemType = ItemType.BEAST;
                    playerEquipController.weapomPlayerEquipe.GetComponent<SpriteRenderer>().sprite = sprite;
                    playerEquipController.weapomPlayerInventario.GetComponent<Image>().sprite = sprite;
                break;
                case ItemType.HEALTH_POTION:
                    FlashGreen();
                    inventoryV2.RemoveItem(item);
                break;
                case ItemType.MANA_POTION:
                    //FlashBlue();
                    inventoryV2.RemoveItem(item);
                break;
            }

        //playerController.usarItemArma(idItem);
    }
    public void FlashGreen() {
        playerAnimator.SetTrigger("healtPotion");
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
            miraPersonagem();
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
    
            if(player.entity.attackTimer == 0) //&& !isWalking)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    string atack;
                    if(GetComponent<PlayerEquipController>().weapomPlayerEquipe.GetComponent<Item>().itemType == ItemType.BEAST) {
                        atack = "attackBeast";
                        playerAnimator.SetTrigger(atack);
                        NetworkManager.instance.EmitAnimation (atack, GetComponent<Player>().entity.id);
                        player.entity.attackTimer = player.entity.cooldown;
                        AttackBeast();
                    } else {
                        atack = "attack";
                        playerAnimator.SetTrigger(atack);
                        NetworkManager.instance.EmitAnimation (atack, GetComponent<Player>().entity.id);
                        player.entity.attackTimer = player.entity.cooldown;
        
                        Attack();
                    }
                }
            }    
            if(Input.GetKeyDown(interactKeySpace)) {
                if(player.itemColetavel != null){

                    //GameObject obj = Instantiate (player.itemColetavel, new Vector3(0,0,0),Quaternion.identity);
                    
                    //sasa
                    print("ALTERAR CAMINHO FIXO");
                    //this.GetComponent<Inventario>().itemInventario.Add(player.itemColetavel.GetComponent<Item>().pathItem);
                    Destroy(player.itemColetavel);
                    player.itemColetavel = null;
                }

            }
            if(canTeleport && tmpRegion != null && Input.GetKeyDown(interactKey)) 
            {
                this.transform.position = tmpRegion.warpLocation.position;
            }     
            if(Input.GetKeyDown(interactKeyI)) 
            {
                //print("Abrir Inventario");
                this.GetComponent<Inventario>().inventario.SetActive(!this.GetComponent<Inventario>().inventario.activeSelf);
                this.GetComponent<Inventario>().carregarInventario();
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                player.attributesPanel.SetActive(!player.attributesPanel.activeSelf);
                //strTxt = GameObject.Find ("StrengthText").GetComponent<Text>();
                //resTxt = GameObject.Find ("ResistenceText").GetComponent<Text>();
                //intTxt = GameObject.Find ("IntelligenceText").GetComponent<Text>();
                //wilTxt = GameObject.Find ("WillpowerText").GetComponent<Text>();
                //pointsTxt = GameObject.Find ("PointsValuesTxt").GetComponent<Text>();
                player.UpdatePoints();
            }
        }
    }

    
    public void miraPersonagem() {
        posMouse = GetCurrentMousePosition(Input.mousePosition);
        targetAim.transform.position = posMouse;
        playPosition =  PersonagemFlip.transform.position;//GetCurrentMousePosition(PersonagemFlip.transform.localScale);
        if(posMouse.x > playPosition.x && lookLeft){
            FlipMouse();
        } else if(posMouse.x <  playPosition.x && !lookLeft) {
            FlipMouse();
        }

        //Vector2 mousePosScreen = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Rigidbody2D rbWeapon = spawnArraw.gameObject.GetComponent<Rigidbody2D>();
        //Vector3 positionSpawArrow = spawnArraw.position;
        //Vector2 lookDir = posMouse - transform.position;
        Vector3 persoPosirtion = PersonagemFlip.transform.position;
        
        if(PersonagemFlip.transform.localScale.x > 0){
        } 
        spawnArraw.transform.localScale = PersonagemFlip.transform.localScale;
        Vector3 aimDrection = (posMouse - persoPosirtion).normalized;
        angle = Math.Atan2(aimDrection.y, aimDrection.x) * Mathf.Rad2Deg;
        
        //angle = Vector3.Angle(spawnArraw.transform.forward, targetAim.transform.forward-spawnArraw.transform.position);
        //print(angle);
        //spawnArraw.transform.rotation =  Vector3.Angle(transform.forward, targetAim.transform.forward);
        spawnArraw.transform.rotation = Quaternion.Euler(0,0,(float)angle);
        GameObject weapon = GetComponent<PlayerEquipController>().weapomPlayerEquipe.GetComponent<Item>().gameObject;
        if(weapon.GetComponent<Item>().itemType == ItemType.BEAST) {
            weapon.transform.localScale = PersonagemFlip.transform.localScale;
            weapon.transform.rotation = Quaternion.Euler(0,0,(float)angle);
        } else {
            //weapon.transform.localScale = PersonagemFlip.transform.localScale;
            weapon.transform.rotation = Quaternion.Euler(0,0,0);
        }
    }
    private Vector3 GetCurrentMousePosition(Vector3 pos)
    {
        var ray = Camera.main.ScreenPointToRay(pos);
        var plane = new Plane(Vector3.forward, Vector3.zero);

        float rayDistance;
        if (plane.Raycast(ray, out rayDistance))
        {
            return ray.GetPoint(rayDistance);
            
        }

        return new Vector3();
    }
    public void FlipMouse() {
        Vector3 theScale = PersonagemFlip.transform.localScale;
        theScale.x *= -1;
        PersonagemFlip.transform.localScale = theScale;
        lookLeft = !lookLeft;
    }

    public void AttackBeast() {
        Vector2 lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //removeFlexa(1);
        GameObject flechaTemp = Instantiate (prefabArraw, spawnArraw.position, spawnArraw.localRotation);
        //flechaTemp.transform.localScale = new Vector3 (flechaTemp.transform.localScale.x * PersonagemFlip.gameObject.transform.localScale.x, flechaTemp.transform.localScale.y, flechaTemp.transform.localScale.z);
        flechaTemp.transform.rotation = Quaternion.Euler(0,0,(float)angle);
        //flechaTemp.GetComponent<Rigidbody2D>().velocity = new Vector2 ( speedArraw * PersonagemFlip.gameObject.transform.localScale.x, 0);
        flechaTemp.GetComponent<Rigidbody2D>().velocity = speedArraw * spawnArraw.right;
        Destroy (flechaTemp, 2f);
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
        if (monster == null)
            return;
 
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
 
            //Debug.Log("Player dmg: " + result.ToString());
            monster.entity.ReceberDano(result);
            monster.entity.target = this.gameObject;
        }
    }

    private void Flip()
    {
        //USADO PELO SERVIDOR PARA ATUALIZAR POSIÇOES DE OUTROS JOGADORES
        Vector3 theScale = PersonagemFlip.transform.localScale;
        theScale.x *= -1;
        PersonagemFlip.transform.localScale = theScale;
        lookLeft = !lookLeft;
    }

    public void ShakeShoot()
    {
        ShakeElapsedTime = ShakeDuration;
    }

    public void VerificaFlipOLD(float x) {


         if (isWalking)
        {
            //playerAnimator.SetFloat("input_x", input_x);
            //playerAnimator.SetFloat("input_y", input_y);
            if (x > 0 && lookLeft == true)
            {
                //Flip();
                //NetworkManager.instance.EmitAnimation ("Flip", x.ToString(), GetComponent<Player>().entity.id);
            } else if(x < 0 && lookLeft == false)
            {
                //Flip();
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