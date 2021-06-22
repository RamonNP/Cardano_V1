using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_InventoryV2 : MonoBehaviour
{

    [SerializeField] private Transform pfUI_Item;
    public InventoryV2 inventory;
    public Transform itemSlotContainer;
    public  Transform itemSlotTemplate;

    public bool slot1;
    public GameObject bag1;
    public bool slot2;
    public GameObject bag2;
    public bool slot3;
    public GameObject bag3;
    private Player player;

    private void Awake() {
        itemSlotContainer = transform.Find("itemSlotContainer");
        itemSlotTemplate = itemSlotContainer.transform.Find("ItemSlotTemplate");
        print(itemSlotTemplate);
    }

    public void SetPlayer(Player player) {
        this.player = player;
    }

    public void SetInventory(InventoryV2 inventory) {
        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;
    //print("SetInventory ");
        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e) {
    //print("Inventory_OnItemListChanged ");
        RefreshInventoryItems();
    }
    public Transform uiItemTransform;
    private void RefreshInventoryItems() {
        foreach (Transform child in itemSlotContainer) {
            if (child == itemSlotTemplate) continue;
            if (child.transform.gameObject.name == "ItemSlottesteapagar") continue;
            Destroy(child.gameObject);
        }

        int x = 0;
        int y = 0;
        float itemSlotCellSize = 62f;
        float baseInvenarioX = 230;
        float baseInvenarioY = 0;
        int indice = 0;
        //baseInvenarioX = 230;
        print("LIST SIZE " +inventory.GetInventorySlotArray().Length);
        foreach (InventoryV2.InventorySlot inventorySlot in inventory.GetInventorySlotArray()) {
            ItemV2 item = inventorySlot.GetItem();

            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            
            
            itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () => {
                // Use item
                inventory.UseItem(item);
            };
            itemSlotRectTransform.GetComponent<Button_UI>().MouseRightClickFunc = () => {
                if(ItemWorldV2.ItemEquipedCheck(item.idItem)) {
                    return;
                }
                // Drop item
                ItemV2 duplicateItem = new ItemV2 { idItem = item.idItem, itemType = item.itemType, amount = 1, pathItem = item.pathItem, pathSprite = item.pathSprite };
                inventory.RemoveItem(item);
                Vector3 p = NetworkManager.instance.playerLocalInstance.transform.position;
                Vector3 v3 = new Vector3(p.x, p.y, p.z);
                //print("COMENTADO DROP ITEM" +NetworkManager.instance.playerLocalInstance.transform.position);
                ItemWorldV2.DropItem(v3, duplicateItem);
                //print("COMENTADO DROP ITEM" +NetworkManager.instance.playerLocalInstance.transform.localPosition);
            }; 

            itemSlotRectTransform.anchoredPosition = new Vector2(baseInvenarioX + (x * itemSlotCellSize), baseInvenarioY + (-y * itemSlotCellSize));
            

            GameObject inteInstanciado = null;
            if (!inventorySlot.IsEmpty()) {
                // Not Empty, has Item
                uiItemTransform = Instantiate(pfUI_Item, itemSlotContainer);
                inteInstanciado = uiItemTransform.gameObject;
                //print("BUGOSOOOOOOOO"+uiItemTransform);
                uiItemTransform.GetComponent<RectTransform>().anchoredPosition = itemSlotRectTransform.anchoredPosition;
                UI_Item uiItem = uiItemTransform.GetComponent<UI_Item>();
                //print(item.idItem);
                uiItem.SetItem(item);
                uiItem.SetSprite(item.GetSprite(item.idItem));
                //print(item.idItem+"INDICE"+indice);
            } /*else {
                inteInstanciado = new GameObject();
            } */

            InventoryV2.InventorySlot tmpInventorySlot = inventorySlot;

            UI_ItemSlot uiItemSlot = itemSlotRectTransform.GetComponent<UI_ItemSlot>();
            uiItemSlot.SetOnDropAction(() => {
                // Dropped on this UI Item Slot
                ItemV2 draggedItem = UI_ItemDrag.Instance.GetItem();
                inventory.AddItem(draggedItem, tmpInventorySlot);
            });

            //Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
            //Sprite var = Resources.Load<Sprite>(item.pathSprite);
            //print("IDITEM" +item.idItem);
            //image.sprite = item.GetSprite(item.idItem);//var;//item.GetSprite(); //Resources.Load(i)


            
            Text uiText = itemSlotRectTransform.Find("amountText").GetComponent<Text>();
            if (item!= null && item.amount > 1) {
                uiText.text = (item.amount.ToString());
            } else {
                uiText.text = ("");
            } 

            DesativarInventario(indice, itemSlotRectTransform.gameObject, inteInstanciado);
            x++;
            if (x >= 3) {
                x = 0;
                y++;
            }
            indice++;
            if(indice == 15) {
                y = 0;
                baseInvenarioX = 0;
            }
            if(indice == 30) {
                y = 0;
                baseInvenarioX = 0;
                baseInvenarioY = -350;
            }
        }
    }
    public void AtivarDesativarBag(int indice) {

        if(indice == 1) {
            if(slot1){
                bag1.SetActive(false);
                slot1 = false;
            } else {
                bag1.SetActive(true);
                slot1 = true;
            }
        } else  if(indice == 2) {
            if(slot2){
                bag2.SetActive(false);
                slot2 = false;
            } else {
                bag2.SetActive(true);
                slot2 = true;
            }

        } else  if(indice == 3) {
            if(slot3){
                bag3.SetActive(false);
                slot3 = false;
            } else {
                bag3.SetActive(true);
                slot3 = true;
            }

        }
        RefreshInventoryItems();
    }

    public void DesativarInventario(int slot, GameObject obj, GameObject obj2 ) {
        if(!slot1 && slot < 15) {
            obj.SetActive(false);
            if(obj2 != null)
            obj2.SetActive(false);
            bag1.SetActive(false);
            //print(slot+obj.gameObject.name+obj2.gameObject.name);
        } else if(slot1 && slot < 15) {
            obj.SetActive(true);
            if(obj2 != null)
            obj2.SetActive(true);
            bag1.SetActive(true);
        }

        if(!slot2 && slot > 14 && slot < 30) {
            obj.SetActive(false);
            if(obj2 != null)
            obj2.SetActive(false);
            bag2.SetActive(false);
            //print(slot+obj.gameObject.name+obj2.gameObject.name);
        } else if(slot2 && slot >14 && slot < 30) {
            obj.SetActive(true);
            if(obj2 != null)
            obj2.SetActive(true);
            bag2.SetActive(true);
        }

        if(!slot3 && slot > 29 ) {
            obj.SetActive(false);
            if(obj2 != null)
            obj2.SetActive(false);
            bag3.SetActive(false);
            //print(slot+obj.gameObject.name+obj2.gameObject.name);
        } else if(slot3 && slot > 29) {
            obj.SetActive(true);
            if(obj2 != null)
            obj2.SetActive(true);
            bag3.SetActive(true);
        }
    }

}
