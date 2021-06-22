/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CharacterEquipment : MonoBehaviour {
    
    [SerializeField] private Transform pfUI_Item;

    private Transform itemContainer;
    private UI_CharacterEquipmentSlot weaponSlot;
    private UI_CharacterEquipmentSlot helmetSlot;
    private UI_CharacterEquipmentSlot armorSlot;
    private UI_CharacterEquipmentSlot shieldSlot;
    private CharacterEquipment characterEquipment;

    private void Awake() {
        itemContainer = transform.Find("itemContainer");
        weaponSlot = transform.Find("weaponSlot").GetComponent<UI_CharacterEquipmentSlot>();
        helmetSlot = transform.Find("helmetSlot").GetComponent<UI_CharacterEquipmentSlot>();
        armorSlot = transform.Find("armorSlot").GetComponent<UI_CharacterEquipmentSlot>();
        shieldSlot = transform.Find("shieldSlot").GetComponent<UI_CharacterEquipmentSlot>();

        weaponSlot.OnItemDropped += WeaponSlot_OnItemDropped;
        helmetSlot.OnItemDropped += HelmetSlot_OnItemDropped;
        armorSlot.OnItemDropped += ArmorSlot_OnItemDropped;
        shieldSlot.OnItemDropped += ShieldSlot_OnItemDropped;
    }

    private void ArmorSlot_OnItemDropped(object sender, UI_CharacterEquipmentSlot.OnItemDroppedEventArgs e) {
        // Item dropped in Armor slot
        characterEquipment.TryEquipItem(CharacterEquipment.EquipSlot.Armor, e.item);
    }

    private void HelmetSlot_OnItemDropped(object sender, UI_CharacterEquipmentSlot.OnItemDroppedEventArgs e) {
        // Item dropped in Helmet slot
        characterEquipment.TryEquipItem(CharacterEquipment.EquipSlot.Helmet, e.item);
    }

    private void WeaponSlot_OnItemDropped(object sender, UI_CharacterEquipmentSlot.OnItemDroppedEventArgs e) {
        // Item dropped in weapon slot
        //print(characterEquipment);
        //print(e.item);
        //print(CharacterEquipment.EquipSlot);
        characterEquipment.TryEquipItem(CharacterEquipment.EquipSlot.Weapon, e.item);
    }
    private void ShieldSlot_OnItemDropped(object sender, UI_CharacterEquipmentSlot.OnItemDroppedEventArgs e) {
        // Item dropped in weapon slot
        //print(characterEquipment);
        //print(e.item);
        //print(CharacterEquipment.EquipSlot);
        characterEquipment.TryEquipItem(CharacterEquipment.EquipSlot.Shield, e.item);
    }

    public void SetCharacterEquipment(CharacterEquipment characterEquipment) {
        this.characterEquipment = characterEquipment;
        UpdateVisual();

        characterEquipment.OnEquipmentChanged += CharacterEquipment_OnEquipmentChanged;
    }

    private void CharacterEquipment_OnEquipmentChanged(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        foreach (Transform child in itemContainer) {
            Destroy(child.gameObject);
        }

        ItemV2 weaponItem = characterEquipment.GetWeaponItem();
        if (weaponItem != null) {
            Transform uiItemTransform = Instantiate(pfUI_Item, itemContainer);
            uiItemTransform.GetComponent<RectTransform>().anchoredPosition = weaponSlot.GetComponent<RectTransform>().anchoredPosition;
            uiItemTransform.localScale = Vector3.one * 1.5f;
            uiItemTransform.GetComponent<CanvasGroup>().blocksRaycasts = false;
            UI_Item uiItem = uiItemTransform.GetComponent<UI_Item>();
            uiItem.SetItem(weaponItem);
            weaponSlot.transform.Find("emptyImage").gameObject.SetActive(false);
        } else {
            weaponSlot.transform.Find("emptyImage").gameObject.SetActive(true);
        }

        ItemV2 armorItem = characterEquipment.GetArmorItem();
        if (armorItem != null) {
            Transform uiItemTransform = Instantiate(pfUI_Item, itemContainer);
            uiItemTransform.GetComponent<RectTransform>().anchoredPosition = armorSlot.GetComponent<RectTransform>().anchoredPosition;
            uiItemTransform.localScale = Vector3.one * 1.5f;
            uiItemTransform.GetComponent<CanvasGroup>().blocksRaycasts = false;
            UI_Item uiItem = uiItemTransform.GetComponent<UI_Item>();
            uiItem.SetItem(armorItem);
            armorSlot.transform.Find("emptyImage").gameObject.SetActive(false);
        } else {
            armorSlot.transform.Find("emptyImage").gameObject.SetActive(true);
        }

        ItemV2 helmetItem = characterEquipment.GetHelmetItem();
        if (helmetItem != null) {
            Transform uiItemTransform = Instantiate(pfUI_Item, itemContainer);
            uiItemTransform.GetComponent<RectTransform>().anchoredPosition = helmetSlot.GetComponent<RectTransform>().anchoredPosition;
            uiItemTransform.localScale = Vector3.one * 1.5f;
            uiItemTransform.GetComponent<CanvasGroup>().blocksRaycasts = false;
            UI_Item uiItem = uiItemTransform.GetComponent<UI_Item>();
            uiItem.SetItem(helmetItem);
            helmetSlot.transform.Find("emptyImage").gameObject.SetActive(false);
        } else {
            helmetSlot.transform.Find("emptyImage").gameObject.SetActive(true);
        }
        ItemV2 shieldItem = characterEquipment.GetShieldItem();
        if (shieldItem != null) {
            Transform uiItemTransform = Instantiate(pfUI_Item, itemContainer);
            uiItemTransform.GetComponent<RectTransform>().anchoredPosition = shieldSlot.GetComponent<RectTransform>().anchoredPosition;
            uiItemTransform.localScale = Vector3.one * 1.5f;
            uiItemTransform.GetComponent<CanvasGroup>().blocksRaycasts = false;
            UI_Item uiItem = uiItemTransform.GetComponent<UI_Item>();
            uiItem.SetItem(shieldItem);
            shieldSlot.transform.Find("emptyImage").gameObject.SetActive(false);
        } else {
            shieldSlot.transform.Find("emptyImage").gameObject.SetActive(true);
        }
    }

}
