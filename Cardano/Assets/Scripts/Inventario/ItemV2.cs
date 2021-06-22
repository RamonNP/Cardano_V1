using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[Serializable]
public class ItemV2
{
    public ItemType itemType;
    public int amount;
    public int idItem;

    public string pathSprite;
    public string pathItem;
    private PlayerEquipController playerEquipController;
    private PlayerController playerController;
    public Sprite GetSprite(int id) {
        ItensEntity itemJson;
        NetworkManager.instance.itensDatabase.TryGetValue(id, out itemJson);
        Sprite var = Resources.Load<Sprite>(itemJson.pathSprite);
        /*
        switch (itemType) {
        default:
        case ItemType.SWORD:        return ItemAssets.Instance.swordSprite;
        case ItemType.HEALTH_POTION: return ItemAssets.Instance.healthPotionSprite;
        case ItemType.MANA_POTION:   return ItemAssets.Instance.manaPotionSprite;
        case ItemType.COIN:         return ItemAssets.Instance.coinSprite;
        }  */
        //Debug.Log("IMPLEMENTAR"+var);
        return var;
    }

    public Color GetColor() {
        switch (itemType) {
        default:
        case ItemType.SWORD:        return new Color(1, 1, 1);
        case ItemType.HEALTH_POTION: return new Color(1, 0, 0);
        case ItemType.MANA_POTION:   return new Color(0, 0, 1);
        case ItemType.COIN:         return new Color(1, 1, 0);
        //case ItemType.Medkit:       return new Color(1, 0, 1);
        }
    }

    public bool IsStackable() {
        switch (itemType) {
        default:
        case ItemType.COIN:
        case ItemType.HEALTH_POTION:
        case ItemType.MANA_POTION:
            return true;
        case ItemType.SWORD:
        case ItemType.ARMOR:
        case ItemType.SHIELD:
        case ItemType.BEAST:
        case ItemType.HAMMER:
        case ItemType.HELMET:
        //case ItemType.Medkit:
            return false;
        }
    }
    public static ItemV2 NewItemv2FromItemJson(ItensEntity itemJson) {
        ItemV2 newItem = new ItemV2();
        newItem.itemType =  itemJson.itemType;
        newItem.pathItem = itemJson.pathItem;
        newItem.pathSprite = itemJson.pathSprite;
        newItem.idItem = itemJson.idItem;
        newItem.amount = 1;
        return newItem;
    }
    public CharacterEquipment.EquipSlot GetEquipSlot() {
        switch (itemType) {
        default:
        case ItemType.SHIELD:
            return CharacterEquipment.EquipSlot.Shield;
        case ItemType.ARMOR:
            return CharacterEquipment.EquipSlot.Armor;
        case ItemType.HELMET:
            return CharacterEquipment.EquipSlot.Helmet;
        case ItemType.BEAST:
        case ItemType.HAMMER:
        case ItemType.SWORD:
            return CharacterEquipment.EquipSlot.Weapon;
        }
    }
     public void UsarItem()
    {
         NetworkManager networkManager = NetworkManager.instance;
        if(networkManager != null) {
            playerController = networkManager.playerLocalInstance.GetComponent<PlayerController>();
            playerEquipController = networkManager.playerLocalInstance.GetComponent<PlayerEquipController>();
        } 
        ItensEntity itemJson;
        NetworkManager.instance.itensDatabase.TryGetValue(idItem, out itemJson);
        int defense = itemJson.defense;
        int atack = itemJson.atack;
        switch (itemType)
            {
                case ItemType.HELMET : 
                    playerEquipController.helmetPlayerEquipe.SetActive(true);
                    playerEquipController.helmetPlayerInventario.SetActive(true);
                    playerEquipController.helmetPlayerEquipe.GetComponent<Item>().idItem = idItem;
                    playerEquipController.helmetPlayerEquipe.GetComponent<Item>().defense = defense;
                    playerEquipController.helmetPlayerEquipe.GetComponent<Item>().atack = atack;
                    playerEquipController.helmetPlayerEquipe.GetComponent<Item>().itemType = ItemType.HELMET;
                    playerEquipController.helmetPlayerEquipe.GetComponent<SpriteRenderer>().sprite = GetSprite(idItem);
                    playerEquipController.helmetPlayerInventario.GetComponent<Image>().sprite = GetSprite(idItem);
                break;
                case ItemType.SWORD : 
                    playerEquipController.weapomPlayerEquipe.SetActive(true);
                    playerEquipController.weapomPlayerInventario.SetActive(true);
                    playerEquipController.weapomPlayerEquipe.GetComponent<Item>().idItem = idItem;
                    playerEquipController.weapomPlayerEquipe.GetComponent<Item>().defense = defense;
                    playerEquipController.weapomPlayerEquipe.GetComponent<Item>().atack = atack;
                    playerEquipController.weapomPlayerEquipe.GetComponent<Item>().itemType = ItemType.SWORD;
                    playerEquipController.weapomPlayerEquipe.GetComponent<SpriteRenderer>().sprite = GetSprite(idItem);
                    playerEquipController.weapomPlayerInventario.GetComponent<Image>().sprite = GetSprite(idItem);
                break;
                case ItemType.BEAST : 
                    playerEquipController.weapomPlayerEquipe.SetActive(true);
                    playerEquipController.weapomPlayerInventario.SetActive(true);
                    playerEquipController.weapomPlayerEquipe.GetComponent<Item>().idItem = idItem;
                    playerEquipController.weapomPlayerEquipe.GetComponent<Item>().defense = defense;
                    playerEquipController.weapomPlayerEquipe.GetComponent<Item>().atack = atack;
                    playerEquipController.weapomPlayerEquipe.GetComponent<Item>().itemType = ItemType.BEAST;
                    playerEquipController.weapomPlayerEquipe.GetComponent<SpriteRenderer>().sprite = GetSprite(idItem);
                    playerEquipController.weapomPlayerInventario.GetComponent<Image>().sprite = GetSprite(idItem);
                break;
                case ItemType.SHIELD : 
                    playerEquipController.shieldPlayerEquipe.SetActive(true);
                    //playerEquipController.weapomPlayerInventario.SetActive(true);
                    playerEquipController.shieldPlayerEquipe.GetComponent<Item>().idItem = idItem;
                    playerEquipController.shieldPlayerEquipe.GetComponent<Item>().defense = defense;
                    playerEquipController.shieldPlayerEquipe.GetComponent<Item>().atack = atack;
                    playerEquipController.shieldPlayerEquipe.GetComponent<Item>().itemType = ItemType.SHIELD;
                    playerEquipController.shieldPlayerEquipe.GetComponent<SpriteRenderer>().sprite = GetSprite(idItem);
                    //playerEquipController.shieldPlayerEquipe.GetComponent<Image>().sprite = GetSprite(idItem);
                break;
                case ItemType.ARMOR : 
                    playerEquipController.armordPlayerEquipe.SetActive(true);
                    //playerEquipController.weapomPlayerInventario.SetActive(true);
                    playerEquipController.armordPlayerEquipe.GetComponent<Item>().idItem = idItem;
                    playerEquipController.armordPlayerEquipe.GetComponent<Item>().defense = defense;
                    playerEquipController.armordPlayerEquipe.GetComponent<Item>().atack = atack;
                    playerEquipController.armordPlayerEquipe.GetComponent<Item>().itemType = ItemType.SHIELD;
                    playerEquipController.armordPlayerEquipe.GetComponent<SpriteRenderer>().sprite = GetSprite(idItem);
                    //playerEquipController.shieldPlayerEquipe.GetComponent<Image>().sprite = GetSprite(idItem);
                break;
            }

        playerController.usarItemArma(idItem);

    }

}
