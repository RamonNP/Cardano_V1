using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
 
public class Monster : MonoBehaviour
{
    private bool isWalking;
    public bool lookLeft;


    [Header("Controller")]
    public Entity entity;
    public GameManager manager;
 
    [Header("Patrol")]
    public List<Transform> waypointList;
    public float arrivalDistance = 0.5f;
    public float waitTime = 5;
    public int waypointID;
 
    // Privates
    Transform targetWapoint;
    int currentWaypoint = 0;
    float lastDistanceToTarget = 0f;
    float currentWaitTime = 0f;
 
    [Header("Experience Reward")]
    public int rewardExperience = 10;
    public int lootGoldMin = 0;
    public int lootGoldMax = 10;
 
    [Header("Respawn")]
    public GameObject prefab;
    public bool respawn = true;
    public float respawnTime = 10f;
 
    [Header("UI")]
    public Slider healthSlider;
 
    Rigidbody2D rb2D;
    Animator animator;

    //private NetworkManager networkManager;
 
    private void Start()
    {
        //networkManager = FindObjectOfType(typeof(NetworkManager)) as NetworkManager;
        //networkManager = NetworkManager.instance;
        rb2D = GetComponent<Rigidbody2D>();
        animator = this.transform.GetChild(0).GetComponent<Animator>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
 
        entity.maxHealth = manager.CalculateHealth(entity);
        entity.maxMana = manager.CalculateMana(entity);
        entity.maxStamina = manager.CalculateStamina(entity);
 
        entity.currentHealth = entity.maxHealth;
        entity.currentMana = entity.maxMana;
        entity.currentStamina = entity.maxStamina;
 
        healthSlider.maxValue = entity.maxHealth;
        healthSlider.value = healthSlider.maxValue;
 
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Waypoint"))
        {
            int ID = obj.GetComponent<WaypointID>().ID;
            if (ID == waypointID)
            {
                waypointList.Add(obj.transform);
            }
        }
 
        currentWaitTime = waitTime;
        if(waypointList.Count > 0)
        {
            targetWapoint = waypointList[currentWaypoint];
            lastDistanceToTarget = Vector2.Distance(transform.position, targetWapoint.position);
        }
    }
 
    private void Update()
    {
        if (entity.dead)
            return;
        if(Input.GetKeyDown(KeyCode.Y)) {
            DropItem();
        }
        if (entity.dead)
            return;
 
        if(entity.currentHealth <= 0)
        {
            entity.currentHealth = 0;
            Die();
        }
 
        healthSlider.value = entity.currentHealth;
 
        if (!entity.inCombat && !entity.dead)
        {
            //print("ATIVO O FALSEEEEE");
            GetComponent<BoxCollider2D>().isTrigger = false;
            if(waypointList.Count > 0)
            {
                Patrol();
            }
            else
            {
                animator.SetBool("isWalking", false);
                print("isWalking"+false);
            }
        }
        else
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
            if (entity.attackTimer > 0)
                entity.attackTimer -= Time.deltaTime;
 
            if (entity.attackTimer < 0)
                entity.attackTimer = 0;
 
            if(entity.target != null && entity.inCombat)
            {
                // atacar
                if (!entity.combatCoroutine)
                    StartCoroutine(Attack());
            }
            else
            {
                entity.combatCoroutine = false;
                StopCoroutine(Attack());
            }
        }
    }
 
    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.tag == "Player" && !entity.dead)
        {
            entity.inCombat = true;
            entity.target = collider.gameObject;
            //entity.target.GetComponent<BoxCollider2D>().isTrigger = true;
        }   
    }
 
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            entity.inCombat = false;
            if (entity.target)
            {
                //entity.target.GetComponent<BoxCollider2D>().isTrigger = false;
                entity.target = null;
            }
        }
    }
 
    void Patrol()
    {
        if (entity.dead)
            return;
 
        // calcular a distance do waypoint
        float distanceToTarget = Vector2.Distance(transform.position, targetWapoint.position);
 
        if(distanceToTarget <= arrivalDistance || distanceToTarget >= lastDistanceToTarget)
        {
 
            if(currentWaitTime <= 0)
            {
                currentWaypoint++;
 
                if (currentWaypoint >= waypointList.Count)
                    currentWaypoint = 0;
 
                targetWapoint = waypointList[currentWaypoint];
                lastDistanceToTarget = Vector2.Distance(transform.position, targetWapoint.position);
                animator.SetBool("isWalking", false);
                //print("isWalking Patrol"+false);
 
                currentWaitTime = waitTime;
            }
            else
            {
                currentWaitTime -= Time.deltaTime;
            }
        }
        else
        {
            animator.SetBool("isWalking", true);
            lastDistanceToTarget = distanceToTarget;
        }
 
        Vector2 direction = (targetWapoint.position - transform.position).normalized;
        VerificaFlipPatrol(direction.x);
 
        rb2D.MovePosition(rb2D.position + direction * (entity.speed * Time.fixedDeltaTime));
    }
 
    IEnumerator Attack()
    {
        entity.combatCoroutine = true;
 
        while (true)
        {
            yield return new WaitForSeconds(entity.cooldown);
 
            if (entity.target != null && !entity.target.GetComponent<Player>().entity.dead)
            {
                animator.SetBool("attack", true);
 
                float distance = Vector2.Distance(entity.target.transform.position, transform.position);
 
                if (distance <= entity.attackDistance)
                {
                    int dmg = manager.CalculateDamage(entity, entity.damage);
                    int targetDef = manager.CalculateDefense(entity.target.GetComponent<Player>().entity, entity.target.GetComponent<Player>().entity.defense);
                    int dmgResult = dmg - targetDef;
 
                    if (dmgResult < 0)
                        dmgResult = 0;
 
                    Debug.Log("Inimigo atacou o player, Dmg: " + dmgResult);
                    entity.target.GetComponent<Player>().entity.currentHealth -= dmgResult;
                    NetworkManager.instance.playerLocalInstance.GetComponent<PlayerController>().ShakeShoot();
                    
                }
            }
        }
    }
 
    void Die()
    {
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
 
        Debug.Log("O inimigo morreu: " + entity.name);
        DropItem();
        StopAllCoroutines();
        StartCoroutine(Respawn());
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

    private void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        lookLeft = !lookLeft;
    }
    public void VerificaFlipPatrol(float x) {
         if (x> 0 && lookLeft == true)
            {
                Flip();
            } else if(x < 0 && lookLeft == false)
            {
                Flip();
            }
    }

    public void DropItem() {
        int id = Random.Range(0, 3);
        var loadedObject = Resources.Load("Prefabs/Helmets/" + "Helmet"+id);
        if (loadedObject == null)
        {
            throw new FileNotFoundException("...no file found - please check the configuration");
        }
        Instantiate(loadedObject, this.transform.position, Quaternion.identity);
    }
}