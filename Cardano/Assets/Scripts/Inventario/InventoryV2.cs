using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class InventoryV2
{
    public event EventHandler OnItemListChanged;

    public List<ItemV2> itemList;
    private Action<ItemV2> useItemAction;
    public InventorySlot[] inventorySlotArray;

    public InventoryV2(Action<ItemV2> useItemAction, int inventorySlotCount) {
        //Debug.Log("USANDO ITEM");
        this.useItemAction = useItemAction;
        itemList = new List<ItemV2>();

        inventorySlotArray = new InventorySlot[inventorySlotCount];
        for (int i = 0; i < inventorySlotCount; i++) {
            inventorySlotArray[i] = new InventorySlot(i);
        }

        ItensEntity itemJson;
        NetworkManager.instance.itensDatabase.TryGetValue(1, out itemJson);
        AddItem(ItemV2.NewItemv2FromItemJson(itemJson));
        NetworkManager.instance.itensDatabase.TryGetValue(2, out itemJson);
        AddItem(ItemV2.NewItemv2FromItemJson(itemJson));
        NetworkManager.instance.itensDatabase.TryGetValue(3, out itemJson);
        AddItem(ItemV2.NewItemv2FromItemJson(itemJson));
        NetworkManager.instance.itensDatabase.TryGetValue(4, out itemJson);
        AddItem(ItemV2.NewItemv2FromItemJson(itemJson));
        NetworkManager.instance.itensDatabase.TryGetValue(16, out itemJson);
        AddItem(ItemV2.NewItemv2FromItemJson(itemJson));
        NetworkManager.instance.itensDatabase.TryGetValue(6, out itemJson);
        AddItem(ItemV2.NewItemv2FromItemJson(itemJson));
        NetworkManager.instance.itensDatabase.TryGetValue(7, out itemJson);
        AddItem(ItemV2.NewItemv2FromItemJson(itemJson));
        NetworkManager.instance.itensDatabase.TryGetValue(8, out itemJson);
        AddItem(ItemV2.NewItemv2FromItemJson(itemJson));
        NetworkManager.instance.itensDatabase.TryGetValue(9, out itemJson);
        AddItem(ItemV2.NewItemv2FromItemJson(itemJson));
        NetworkManager.instance.itensDatabase.TryGetValue(15, out itemJson);
        AddItem(ItemV2.NewItemv2FromItemJson(itemJson));
        //AddItem(new ItemV2 { itemType = ItemType.SWORD, amount = 1, pathSprite = "Armas/Espadas/203-adaga_ouro", pathItem = "Prefabs/Weapon/Sword/Sword1" });
        //AddItem(new ItemV2 { itemType = ItemType.SWORD, amount = 1, pathSprite = "Armas/Espadas/203-adaga_ouro", pathItem = "Prefabs/Weapon/Sword/Sword1" });
        //AddItem(new ItemV2 { itemType = ItemType.SWORD, amount = 1, pathSprite = "Armas/Espadas/203-adaga_ouro", pathItem = "Prefabs/Weapon/Sword/Sword1" });
        //AddItem(new ItemV2 { itemType = ItemType.SWORD, amount = 1, pathSprite = "Armas/Espadas/203-adaga_ouro", pathItem = "Prefabs/Weapon/Sword/Sword1" });
        //AddItem(new ItemV2 { itemType = ItemType.HEALTH_POTION, amount = 1, pathSprite = "pocoes/HealthPotion", pathItem = "Prefabs/Potion/HealthPotion" });
        //AddItem(new ItemV2 { itemType = ItemType.HEALTH_POTION, amount = 1, pathSprite = "pocoes/HealthPotion", pathItem = "Prefabs/Potion/HealthPotion" });
        //AddItem(new ItemV2 { itemType = ItemType.HEALTH_POTION, amount = 1, pathSprite = "pocoes/HealthPotion", pathItem = "Prefabs/Potion/HealthPotion" });
        //AddItem(new ItemV2 { itemType = ItemType.HEALTH_POTION, amount = 1, pathSprite = "pocoes/HealthPotion", pathItem = "Prefabs/Potion/HealthPotion" });
        //AddItem(new ItemV2 { itemType = ItemType.HEALTH_POTION, amount = 1, pathSprite = "pocoes/HealthPotion", pathItem = "Prefabs/Potion/HealthPotion" });
        //AddItem(new ItemV2 { itemType = ItemType.HEALTH_POTION, amount = 1, pathSprite = "pocoes/HealthPotion", pathItem = "Prefabs/Potion/HealthPotion" });
        //AddItem(new ItemV2 { itemType = ItemType.MANA_POTION, amount = 1, pathSprite = "pocoes/ManaPotion" });
        //AddItem(new ItemV2 { itemType = ItemType.BEAST, amount = 1, pathSprite = "Armas/Bestas/besta azul" });
        //AddItem(new ItemV2 { itemType = ItemType.HAMMER, amount = 1, pathSprite = "Armas/Martelo/martelopequeno_bronze" });

       
        
    }
    public InventorySlot GetEmptyInventorySlot() {
        foreach (InventorySlot inventorySlot in inventorySlotArray) {
            if (inventorySlot.IsEmpty()) {
                return inventorySlot;
            }
        }
        Debug.LogError("Cannot find an empty InventorySlot!");
        return null;
    }

    public InventorySlot GetInventorySlotWithItem(ItemV2 item) {
        foreach (InventorySlot inventorySlot in inventorySlotArray) {
            if (inventorySlot.GetItem() == item) {
                return inventorySlot;
            }
        }
        Debug.LogError("Cannot find Item " + item + " in a InventorySlot!");
        return null;
    }
    public void AddItem(ItemV2 item, InventorySlot inventorySlot) {
        RemoveItem(item);

        itemList.Add(item);
        inventorySlot.SetItem(item);

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }
    public bool AddItem(ItemV2 item) {
        if(GetEmptyInventorySlot() != null) {
            if (item.IsStackable()) {
                bool itemAlreadyInInventory = false;
                foreach (ItemV2 inventoryItem in itemList) {
                    if (inventoryItem.itemType == item.itemType) {
                        inventoryItem.amount += item.amount;
                        itemAlreadyInInventory = true;
                    }
                }
                if (!itemAlreadyInInventory) {
                    itemList.Add(item);
                    GetEmptyInventorySlot().SetItem(item);
                }
            } else {
                itemList.Add(item);
                GetEmptyInventorySlot().SetItem(item);
            }
            OnItemListChanged?.Invoke(this, EventArgs.Empty);
            return true;
        } else {
            return false;
        }
    }

    public void RemoveItem(ItemV2 item) {
        ItemV2 itemInInventory = null;
        if (item.IsStackable()) {
            foreach (ItemV2 inventoryItem in itemList) {
                if (inventoryItem.itemType == item.itemType) {
                    //inventoryItem.amount -= item.amount;
                    item.amount -= 1;
                    inventoryItem.amount = item.amount;
                    itemInInventory = inventoryItem;
                }
            }
            Debug.Log(itemInInventory.amount +"-"+itemInInventory);
            if (itemInInventory != null && itemInInventory.amount <= 0) {
                GetInventorySlotWithItem(itemInInventory).RemoveItem();
                itemList.Remove(itemInInventory);
            }
        } else {
            GetInventorySlotWithItem(item).RemoveItem();
            itemList.Remove(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UseItem(ItemV2 item) {
        
        useItemAction(item);
    }

    public List<ItemV2> GetItemList() {
        return itemList;
    }

    public InventorySlot[] GetInventorySlotArray() {
        return inventorySlotArray;
    }



       /*
     * Represents a single Inventory Slot
     * */
    public class InventorySlot {

        private int index;
        private ItemV2 item;

        public InventorySlot(int index) {
            this.index = index;
        }

        public ItemV2 GetItem() {
            return item;
        }

        public void SetItem(ItemV2 item) {
            this.item = item;
        }

        public void RemoveItem() {
            item = null;
        }

        public bool IsEmpty() {
            return item == null;
        }

    }


}
