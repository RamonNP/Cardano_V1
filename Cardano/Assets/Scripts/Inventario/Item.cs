using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
[Serializable]
public enum ItemType
{
    HELMET = 0,
    ARMOR = 1,
    SWORD = 2,
    BEAST = 3,
    HAMMER = 4,
    SHIELD = 5,
    MANA_POTION = 6,
    HEALTH_POTION = 7,
    COIN
}
[Serializable]
public class Item : MonoBehaviour
{
    public static event Action<Item> OnItemCollected = delegate { };
    private PlayerController playerController;
    private PlayerEquipController playerEquipController;

//###ID;NOME;TIPO;PATH_PREFAB;PATH_SPRITE;DEFESA;ATAQUE;$
    [Header("Id Item")]
    public int idItem;
    public string itemName;
    public ItemType itemType;
    public string pathItem;
    public string pathSprit;
    public int defense;
    public int atack;


    // Start is called before the first frame update
    void Start()
    {
        NetworkManager networkManager = NetworkManager.instance;
        if(networkManager != null) {
            playerController = networkManager.playerLocalInstance.GetComponent<PlayerController>();
            playerEquipController = networkManager.playerLocalInstance.GetComponent<PlayerEquipController>();
        } 

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void usarItem()
    {
         switch (itemType)
            {
                case ItemType.HELMET : 
                    print(" Este item " + idItem + " foi utilizado ");
                    playerEquipController.helmetPlayerEquipe.SetActive(true);
                    playerEquipController.helmetPlayerInventario.SetActive(true);
                    playerEquipController.helmetPlayerEquipe.GetComponent<Item>().idItem = idItem;
                    playerEquipController.helmetPlayerEquipe.GetComponent<Item>().defense = defense;
                    playerEquipController.helmetPlayerEquipe.GetComponent<Item>().atack = atack;
                    playerEquipController.helmetPlayerEquipe.GetComponent<Item>().itemType = ItemType.HELMET;
                    playerEquipController.helmetPlayerEquipe.GetComponent<SpriteRenderer>().sprite = this.GetComponent<SpriteRenderer>().sprite;
                    playerEquipController.helmetPlayerInventario.GetComponent<Image>().sprite = this.GetComponent<SpriteRenderer>().sprite;
                break;
                case ItemType.SWORD : 
                    print(" Este item " + idItem + " foi utilizado ");
                    playerEquipController.weapomPlayerEquipe.SetActive(true);
                    playerEquipController.weapomPlayerInventario.SetActive(true);
                    playerEquipController.weapomPlayerEquipe.GetComponent<Item>().idItem = idItem;
                    playerEquipController.weapomPlayerEquipe.GetComponent<Item>().defense = defense;
                    playerEquipController.weapomPlayerEquipe.GetComponent<Item>().atack = atack;
                    playerEquipController.weapomPlayerEquipe.GetComponent<Item>().itemType = ItemType.SWORD;
                    playerEquipController.weapomPlayerEquipe.GetComponent<SpriteRenderer>().sprite = this.GetComponent<SpriteRenderer>().sprite;
                    playerEquipController.weapomPlayerInventario.GetComponent<Image>().sprite = this.GetComponent<SpriteRenderer>().sprite;
                break;
                case ItemType.BEAST : 
                    print(" Este item " + idItem + " foi utilizado ");
                    playerEquipController.weapomPlayerEquipe.SetActive(true);
                    playerEquipController.weapomPlayerInventario.SetActive(true);
                    playerEquipController.weapomPlayerEquipe.GetComponent<Item>().idItem = idItem;
                    playerEquipController.weapomPlayerEquipe.GetComponent<Item>().defense = defense;
                    playerEquipController.weapomPlayerEquipe.GetComponent<Item>().atack = atack;
                    playerEquipController.weapomPlayerEquipe.GetComponent<Item>().itemType = ItemType.BEAST;
                    playerEquipController.weapomPlayerEquipe.GetComponent<SpriteRenderer>().sprite = this.GetComponent<SpriteRenderer>().sprite;
                    playerEquipController.weapomPlayerInventario.GetComponent<Image>().sprite = this.GetComponent<SpriteRenderer>().sprite;
                break;
            }

        playerController.usarItemArma(idItem);

    }
    public void removerItem()
    {
        switch (itemType)
            {
                case ItemType.HELMET : 
                    playerEquipController.helmetPlayerEquipe.SetActive(false);
                    playerEquipController.helmetPlayerInventario.SetActive(false);
                break;
                case ItemType.SWORD : 
                    playerEquipController.weapomPlayerEquipe.SetActive(false);
                    playerEquipController.weapomPlayerInventario.SetActive(false);
                break;
                case ItemType.BEAST : 
                    playerEquipController.weapomPlayerEquipe.SetActive(false);
                    playerEquipController.weapomPlayerInventario.SetActive(false);
                break;
            }
    }
    private void OnTriggerEnter2D(Collider2D collider) {
        //print(collider.tag);
        if (collider.tag == "AlvoPlayer")
        {
            //player.itemColetavel = collider.gameObject;
            ItemWorldV2 itemWorld = gameObject.GetComponent<ItemWorldV2>();
            if (itemWorld != null) {
                // Touching Item
                //print("ITEM ADICIONADO "+ itemWorld.item.idItem);
                if(NetworkManager.instance.playerLocalInstance.GetComponent<PlayerController>().inventoryV2_1.AddItem(itemWorld.GetItem())){
                    OnItemCollected?.Invoke(this);
                    itemWorld.DestroySelf();
                } else {
                    Debug.Log("Inventário Cheio IMPLEMTAR NOVOS INVENTARIOS");
                }
            }
        }
    }
}

