using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Monster : MonoBehaviour
{
    public static Action<Monster> OnEnemyDeath = delegate {  };
    [SerializeField]    private MonsterType monsterType;
    public GameObject PersonagemFlip;
    public bool lookLeft;
    public Entity entity;
    public GameManager manager;
    public Animator animator;
    Rigidbody2D rb2D;
    public Vector3 homePosition;

    public float speed;
    public float maxRange;
    public float minRange;
    public bool flashActive;
    [Header("UI")]
    public Slider healthSlider;
    [Header("Experience Reward")]
    public int rewardExperience = 10;
    public string[] dropList;
    public float respawnTime = 10f;
    public GameObject prefab;

    public MonsterType GetMonsterType { get => monsterType;}

    // Start is called before the first frame update
    void Start()
    {
        homePosition = new Vector3(transform.position.x,transform.position.y,transform.position.z);
        rb2D = GetComponent<Rigidbody2D>();
        animator = this.transform.GetChild(0).GetComponent<Animator>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();

        entity.maxHealth = manager.CalculateHealth(entity);
        entity.maxMana = manager.CalculateMana(entity);
        entity.maxStamina = manager.CalculateStamina(entity);
 
        entity.CurrentHealth = entity.maxHealth;
        entity.currentMana = entity.maxMana;
        entity.currentStamina = entity.maxStamina;
 
        healthSlider.maxValue = entity.maxHealth;
        healthSlider.value = healthSlider.maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (entity.dead)
            return;
        if(Input.GetKeyDown(KeyCode.Y)) {
            DropItem();
        }
        if(entity.target != null) {
            float distance = Vector3.Distance(entity.target.transform.position, transform.position);
            //print(distance);
            if(distance <= maxRange && distance >= minRange) {
                FollowPlayer();
                //print("Folow");
            } else  if (distance >= maxRange){
                //print("home");
                GoHome();
            } else if(distance <= minRange) {
                if(entity.attackTimer == 0) {
                    animator.SetTrigger("attack");
                    entity.attackTimer = entity.cooldown;
                }
            }
        }

        //ATACK TIMER INICIO 
        if (entity.attackTimer > 0)
            entity.attackTimer -= Time.deltaTime;
 
        if (entity.attackTimer < 0)
            entity.attackTimer = 0;
        //ATACK TIMER FIM 

        if(entity.CurrentHealth <= 0)
        {
            entity.ReceberDano(0);
            Die();
        }
 
        healthSlider.value = entity.CurrentHealth;
    }
    public void GoHome() {
        float distance = Vector3.Distance(homePosition, transform.position);
        //Debug.Log(Mathf.Round(distance));
        //print(distance);
        if(distance < 0.1f){
            animator.SetBool("isWalking", false);
        } else {
            Vector2 direction = (homePosition - transform.position).normalized;
            Vector2 vc2 = homePosition;
            rb2D.MovePosition(rb2D.position + direction * (entity.speed * Time.fixedDeltaTime));
            VerificaFlipPatrol(direction.x);
        }

    }

    public void FollowPlayer() {
        animator.SetBool("isWalking", true);
        Vector2 direction = (entity.target.transform.position - transform.position).normalized;
        rb2D.MovePosition(rb2D.position + direction * (entity.speed * Time.fixedDeltaTime));
        VerificaFlipPatrol(direction.x);
    }

    private void OnTriggerEnter2D(Collider2D collision2d) {
        //print(collision2d.gameObject.tag + "ENTER");
        switch (collision2d.gameObject.tag)
        {
            case "Player":
                //print(collision2d.gameObject.tag + "ENTER");
                entity.target = collision2d.gameObject.GetComponent<Player>().gameObject;
            break;
            
        }
    }
 
    private void OnTriggerExit2D(Collider2D collider)
    {
        /*
        print(collider.gameObject.tag + "EXIT");
        if (collider.tag == "Player")
        {
            entity.inCombat = false;
            if (entity.target)
            {
                //entity.target.GetComponent<BoxCollider2D>().isTrigger = false;
                entity.target = null;
            }
        } */
    }
    private void Flip()
    {
        Vector3 theScale = PersonagemFlip.transform.localScale;
        theScale.x *= -1;
        PersonagemFlip.transform.localScale = theScale;
        lookLeft = !lookLeft;
    }
    public void VerificaFlipPatrol(float x) {
         if (x > 0 && lookLeft == true)
            {
                Flip();
            } else if(x < 0 && lookLeft == false)
            {
                Flip();
            }
    }
    void Die()
    {
        OnEnemyDeath?.Invoke(this);
        GetComponent<BoxCollider2D>().isTrigger = true;
        //entity.target.GetComponent<BoxCollider2D>().isTrigger = false;
        entity.dead = true;
        entity.inCombat = false;
        entity.target = null;
 
        animator.SetBool("isWalking", false);
        animator.SetBool("dead", true);

 
        // add exp no player
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.GainExp(rewardExperience);
 
        //Debug.Log("O inimigo morreu: " + entity.name);
        DropItem();
        StopAllCoroutines();
        Destroy(this.gameObject, 1);
        //StartCoroutine(Respawn());
    }
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        animator.SetBool("dead", false);
        yield return new WaitForSeconds(1f);
        GameObject newMonster = Instantiate(prefab, transform.position, Quaternion.identity, null);
        newMonster.name = prefab.name;
        newMonster.GetComponent<Monster>().entity.dead = false;
        newMonster.GetComponent<Monster>().entity.combatCoroutine = false;
        Destroy(this.gameObject);
    }
    public void DropItem() {
        int id = UnityEngine.Random.Range(1, dropList.Length);
        ItensEntity itemJson;
        NetworkManager.instance.itensDatabase.TryGetValue(id, out itemJson);
        //print("ID DO ITEM CORRIGIR ERRO aconse de vez enquando nupointer"+ id);
        ItemV2 item = ItemV2.NewItemv2FromItemJson(itemJson);
        ItemWorldV2.DropItem(this.transform.position, item);
        //var loadedObject = Resources.Load(dropList[id]);
        /*if(id == 1){
            id = Random.Range(0, 3);
            loadedObject = Resources.Load("Prefabs/Helmets/" + "Helmet"+id);
        } else if(id == 2){
            loadedObject = Resources.Load("Prefabs/Helmets/" + "Helmet"+id);
        } else {
            loadedObject = Resources.Load("Prefabs/Helmets/" + "Helmet"+id);
        }  */

        //Instantiate(loadedObject, this.transform.position, Quaternion.identity);
    }
}
public enum MonsterType
{
    Dragon = 0,
    Balrog = 1,
    Wraith = 2,
    Ent = 3,
    Manticore = 4,
    King = 5,
    Minotaur = 6,
    Cyclop = 7,
    Demon = 8
}
