/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipment : MonoBehaviour {

    public event EventHandler OnEquipmentChanged;

    public enum EquipSlot {
        Helmet,
        Armor,
        Weapon,

        Shield
    }

    private Player player;

    private ItemV2 weaponItem;
    private ItemV2 helmetItem;
    private ItemV2 armorItem;
    private ItemV2 shieldItem;

    private void Awake() {
        player = GetComponent<Player>();
    }

    public ItemV2 GetWeaponItem() {
        return weaponItem;
    }

    public ItemV2 GetHelmetItem() {
        return helmetItem;
    }

    public ItemV2 GetArmorItem() {
        return armorItem;
    }
    public ItemV2 GetShieldItem() {
        return shieldItem;
    }

    private void SetWeaponItem(ItemV2 weaponItem) {
        this.weaponItem = weaponItem;
        //player.SetEquipment(weaponItem.itemType);
        weaponItem.UsarItem();
        Debug.Log("IMPLEMENTAR");
        OnEquipmentChanged?.Invoke(this, EventArgs.Empty);
    }

    private void SetHelmetItem(ItemV2 helmetItem) {
        this.helmetItem = helmetItem;
        helmetItem.UsarItem();
        //player.SetEquipment(helmetItem.itemType);
        OnEquipmentChanged?.Invoke(this, EventArgs.Empty);
        Debug.Log("IMPLEMENTAR");
    }

    private void SetArmorItem(ItemV2 armorItem) {
        this.armorItem = armorItem;
        armorItem.UsarItem();
        //player.SetEquipment(armorItem.itemType);
        OnEquipmentChanged?.Invoke(this, EventArgs.Empty);
        Debug.Log("IMPLEMENTAR");
    }
    private void SetShieldItem(ItemV2 shieldItem) {
        this.shieldItem = shieldItem;
        shieldItem.UsarItem();
        //player.SetEquipment(armorItem.itemType);
        OnEquipmentChanged?.Invoke(this, EventArgs.Empty);
        Debug.Log("IMPLEMENTAR");
    }

    public void TryEquipItem(EquipSlot equipSlot, ItemV2 item) {
        if (equipSlot == item.GetEquipSlot()) {
            // Item matches this EquipSlot
            switch (equipSlot) {
            default:
            case EquipSlot.Armor:   SetArmorItem(item);     break;
            case EquipSlot.Helmet:  SetHelmetItem(item);    break;
            case EquipSlot.Weapon:  SetWeaponItem(item);    break;
            case EquipSlot.Shield:  SetShieldItem(item);    break;
            }
        }
    }

}
