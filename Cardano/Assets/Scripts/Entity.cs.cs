using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Entity
{
  [Header("Name")]
    public string name;
    public string id;
    public bool isLocalPlayer;
    public int level;
 
    [Header("Health")]
    private int currentHealth;
    public int maxHealth;
 
    [Header("Mana")]
    public int currentMana;
    public int maxMana;
 
    [Header("Stamina")]
    public int currentStamina;
    public int maxStamina;
 
    [Header("Stats")]
    public int strength = 1;
    public int resistence = 1;
    public int intelligence = 1;
    public int willpower = 1;
    public int damage = 1;
    public int defense = 1;
    public float speed = 2f;
    public int points = 0;
    public int weapomEquip;
    public int helmetEquip;
    public int armorEquip;
    public int shieldEquip;
 
    [Header("Combat")]
    public float attackDistance = 0.5f;
    public float attackTimer = 1;
    public float cooldown = 2;
    public bool inCombat = false;
    public GameObject target;
    public bool combatCoroutine = false;
    public bool dead = false;
    public bool flashActive;
    [SerializeField]
    public float flashLenght = 0f;
    public float flashCounter = 0f;
 
    [Header("Component")]
    public AudioSource entityAudio;

    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }

    public void ReceberDano(int dano ) {
      flashActive = true;
      flashCounter = flashLenght;
      //Debug.Log(flashLenght +"--"+flashCounter);
      currentHealth -= dano;
      if(currentHealth < 0) {
        currentHealth = 0;
      }
    }
    public void ReceberHp(int hp ) {
       currentHealth += hp;
    }
    

}