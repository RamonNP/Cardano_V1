using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{ 
   public Entity entity;
   public GameObject itemColetavel;
 
    [Header("Player Regen System")]
    public bool regenHPEnabled = true;
    public float regenHPTime = 5f;
    public int regenHPValue = 5;
    public bool regenMPEnabled = true;
    public float regenMPTime = 10f;
    public int regenMPValue = 5;
 
    [Header("Game Manager")]
    public GameManager manager;
 
    [Header("Player Inventario")]
    public int qtdPocoes;
    public int qtdMana;
    public int qtdFlechas;
    [Header("Player Shortcuts")]
    public KeyCode attributesKey = KeyCode.C;
 
    [Header("Player UI Panels")]
    public GameObject attributesPanel;
     
    public float ShakeElapsedTime = 0f;
    [Header("Player Name")]
    public Text nameText;

    [Header("Player UI")]
    public Slider health;
    public Slider mana;
    public Slider stamina;
    public Slider exp;
    public Text expText;
    public Text levelText;
    public Text strTxt;
    public Text resTxt;
    public Text intTxt;
    public Text wilTxt;
    public Button strPositiveBtn;
    public Button resPositiveBtn;
    public Button intPositiveBtn;
    public Button wilPositiveBtn;
    public Button strNegativeBtn;
    public Button resNegativeBtn;
    public Button intNegativeBtn;
    public Button wilNegativeBtn;
    public Text pointsTxt;
 
    [Header("Exp")]
    public int currentExp;
    public int expBase;
    public int expLeft;
    public float expMod;
    public GameObject levelUpFX;
    public AudioClip levelUpSound;
    public int givePoints = 5;
 
    [Header("Respawn")]
    public float respawnTime = 5;
    public GameObject prefab;
 
    void Start()
    {
        health = GameObject.FindGameObjectWithTag ("HPSLider").GetComponent<Slider>();
        mana = GameObject.Find("MPSLider").GetComponent<Slider>();
        stamina = GameObject.Find("StaminaSLider").GetComponent<Slider>();
        exp = GameObject.Find("XpSLider").GetComponent<Slider>();

        expText = GameObject.Find ("ExpText").GetComponent<Text>();
        levelText = GameObject.Find ("LevelText").GetComponent<Text>();
        
        

        if (manager == null){
            manager = FindObjectOfType(typeof(GameManager)) as GameManager;
        }
 
        entity.maxHealth = manager.CalculateHealth(entity);
        entity.maxMana = manager.CalculateMana(entity);
        entity.maxStamina = manager.CalculateStamina(entity);
 
        entity.CurrentHealth = entity.maxHealth;
        entity.currentMana = entity.maxMana;
        entity.currentStamina = entity.maxStamina;
 
        health.maxValue = entity.maxHealth;
        health.value = health.maxValue;
 
        mana.maxValue = entity.maxMana;
        mana.value = mana.maxValue;
 
        stamina.maxValue = entity.maxStamina;
        stamina.value = stamina.maxValue;
 
        exp.value = currentExp;
        exp.maxValue = expLeft;
 
        expText.text = String.Format("Exp: {0}/{1}", currentExp, expLeft);
        levelText.text = entity.level.ToString();
 
        // iniciar o regenhealth
        StartCoroutine(RegenHealth());
        StartCoroutine(RegenMana());
 
        
        SetupUIButtons();
    }
 
    private void Update()
    {
        if (entity.dead)
            return;
        //if(!entity.flashActive)
        //StartCoroutine(FlashEnum());
        if (entity.CurrentHealth <= 0)
        {
            Die();
        }
        calcularEquipamentos();
        
 
        health.value = entity.CurrentHealth;
        mana.value = entity.currentMana;
        stamina.value = entity.currentStamina;
 
        exp.value = currentExp;
        exp.maxValue = expLeft;
 
        expText.text = String.Format("Exp: {0}/{1}", currentExp, expLeft);
        levelText.text = entity.level.ToString();
    }
    IEnumerator FlashEnum() {
        if(entity.flashActive) {
            entity.flashActive = false;
            ChangeColor(GetComponent<PlayerController>().PersonagemFlip, false);
            yield return new WaitForSeconds(1f);
            ChangeColor(GetComponent<PlayerController>().PersonagemFlip, false);
            yield return new WaitForSeconds(1);
            
        }

    }
    public void  Flash() {
        if(entity.flashActive) {

            if(entity.flashCounter > entity.flashLenght*.99f) {
                ChangeColor(GetComponent<PlayerController>().PersonagemFlip, true);
            } else if(entity.flashCounter > entity.flashLenght*.82f) {
                ChangeColor(GetComponent<PlayerController>().PersonagemFlip, false);
            } else if(entity.flashCounter > entity.flashLenght*.66f) {
                ChangeColor(GetComponent<PlayerController>().PersonagemFlip, true);
            } else if(entity.flashCounter > entity.flashLenght*.49f) {
                ChangeColor(GetComponent<PlayerController>().PersonagemFlip, false);
            } else if(entity.flashCounter > entity.flashLenght*.33f) {
                ChangeColor(GetComponent<PlayerController>().PersonagemFlip, true);
            } else if(entity.flashCounter > entity.flashLenght*.16f) {
                ChangeColor(GetComponent<PlayerController>().PersonagemFlip, false);
            } else if(entity.flashCounter > 0) {
                ChangeColor(GetComponent<PlayerController>().PersonagemFlip, true);
            } else {
                //implementar percorrer sprits e muda a cor
                entity.flashActive = false;
            }
            entity.flashCounter -= Time.deltaTime;
        }
    }
    private void ChangeColor(GameObject obj, bool flash) {
        foreach ( Transform child in obj.transform) {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            print(child.gameObject.name + " - "+ flash +"-"+sr);
            if(sr != null){
                if(flash) {
                    print(child.gameObject.name + "Normal");
                    sr.color = new Color(255,255,255,255);
                } else {
                    print(child.gameObject.name + "Vermelho");
                    sr.color = new Color(255,0,0,255);
                }
            }
            ChangeColor(child.gameObject, flash);
        }
    }
    private void calcularEquipamentos() {
        GameObject weapomPlayerEquipe = GetComponent<PlayerEquipController>().weapomPlayerEquipe;
        GameObject helmetPlayerEquipe = GetComponent<PlayerEquipController>().helmetPlayerEquipe;
        int atack = 0;
        if(weapomPlayerEquipe.activeSelf) {
            atack += weapomPlayerEquipe.GetComponent<Item>().atack;
        }
        if(helmetPlayerEquipe.activeSelf) {
            atack += helmetPlayerEquipe.GetComponent<Item>().atack;
        }
        entity.damage = atack;
    }
 
    IEnumerator RegenHealth()
    {
        while (true) // loop infinito
        {
            if (regenHPEnabled)
            {
                if (entity.CurrentHealth < entity.maxHealth)
                {
                    Debug.LogFormat("Recuperando HP do jogador");
                    //entity.CurrentHealth += regenHPValue;
                    entity.ReceberHp(regenHPValue);
                    yield return new WaitForSeconds(regenHPTime);
                }
                else
                {
                    yield return null;
                }
            }
            else
            {
                yield return null;
            }
        }
    }
 
    IEnumerator RegenMana()
    {
        while (true) // loop infinito
        {
            if (regenHPEnabled)
            {
                if (entity.currentMana < entity.maxMana)
                {
                    Debug.LogFormat("Recuperando MP do jogador");
                    entity.currentMana += regenMPValue;
                    yield return new WaitForSeconds(regenMPTime);
                }
                else
                {
                    yield return null;
                }
            }
            else
            {
                yield return null;
            }
        }
    }
 
    void Die()
    {
        entity.CurrentHealth = 0;
        entity.dead = true;
        entity.target = null;
 
        StopAllCoroutines();
        StartCoroutine(Respawn());
    }
 
    IEnumerator Respawn()
    {
        GetComponent<PlayerController>().enabled = false;
 
        yield return new WaitForSeconds(respawnTime);
 
        GameObject newPlayer = Instantiate(prefab, transform.position, transform.rotation, null);
        newPlayer.name = prefab.name;
        newPlayer.GetComponent<Player>().entity.dead = false;
        newPlayer.GetComponent<Player>().entity.combatCoroutine = false;
        newPlayer.GetComponent<PlayerController>().enabled = true;
 
        Destroy(this.gameObject);
    }
 
    public void GainExp(int amount)
    {
        //Debug.Log(amount);
        currentExp += amount;
        if (currentExp >= expLeft)
        {
            LevelUp();
        }
    }
 
    public void LevelUp()
    {
        currentExp -= expLeft;
        entity.level++;
        entity.points += givePoints;
        UpdatePoints();
 
        entity.CurrentHealth = entity.maxHealth;
 
        float newExp = Mathf.Pow((float)expMod, entity.level);
        expLeft = (int)Mathf.Floor((float)expBase * newExp);
 
        entity.entityAudio.PlayOneShot(levelUpSound);
        GameObject g = Instantiate(levelUpFX, this.gameObject.transform);
        Destroy(g,2);
    }
 
    public void UpdatePoints()
    {
        strTxt.text = entity.strength.ToString();
        resTxt.text = entity.resistence.ToString();
        intTxt.text = entity.intelligence.ToString();
        wilTxt.text = entity.willpower.ToString();
        pointsTxt.text = entity.points.ToString();
    }
 
    public void SetupUIButtons()
    {
        strPositiveBtn.onClick.AddListener(() => AddPoints(1));
        resPositiveBtn.onClick.AddListener(() => AddPoints(2));
        intPositiveBtn.onClick.AddListener(() => AddPoints(3));
        wilPositiveBtn.onClick.AddListener(() => AddPoints(4));
 
        strNegativeBtn.onClick.AddListener(() => RemovePoints(1));
        resNegativeBtn.onClick.AddListener(() => RemovePoints(2));
        intNegativeBtn.onClick.AddListener(() => RemovePoints(3));
        wilNegativeBtn.onClick.AddListener(() => RemovePoints(4)); 
    }
 
    public void AddPoints(int index)
    {
        if(entity.points > 0)
        {
            if (index == 1) // str
                entity.strength++;
            else if (index == 2)
                entity.resistence++;
            else if (index == 3)
                entity.intelligence++;
            else if (index == 4)
                entity.willpower++;
 
            entity.points--;
            UpdatePoints();
        }
    }
 
    public void RemovePoints(int index)
    {
        if (entity.points > 0)
        {
            if (index == 1 && entity.strength > 0)
                entity.strength--;
            else if (index == 2 && entity.resistence > 0)
                entity.resistence--;
            else if (index == 3 && entity.intelligence > 0)
                entity.intelligence--;
            else if (index == 4 && entity.willpower > 0)
                entity.willpower--;
 
            entity.points++;
            UpdatePoints();
        }
    }
 
}