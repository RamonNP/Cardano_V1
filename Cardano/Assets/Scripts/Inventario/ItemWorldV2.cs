using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemWorldV2 : MonoBehaviour {

    public static Item SpawnItemWorld(Vector3 position, ItemV2 item) {

        var v = Instantiate(Resources.Load(item.pathItem), position, Quaternion.identity) as GameObject;

        v.GetComponent<BoxCollider2D>().isTrigger = true;
        Item itemWorld = v.GetComponent<Item>();
        ItemWorldV2 item3 = v.GetComponent<ItemWorldV2>();
        item3.item = item;
        ItensEntity itemJson;
        NetworkManager.instance.itensDatabase.TryGetValue(1, out itemJson);
        item3.name = itemJson.itemName;
        //item3. = itemJson.itemName;
        item3.name = itemJson.itemName;
        v.GetComponent<SpriteRenderer>().sprite = item.GetSprite(item.idItem);
        //itemWorld.SetItem(item);

        return itemWorld;
    }

    public static Item DropItem(Vector3 dropPosition, ItemV2 item) {
        Vector3 randomDir = GetRandomDir();
        Item itemWorld = SpawnItemWorld(dropPosition + randomDir * 0.3f, item);
        //itemWorld.GetComponent<Rigidbody2D>().AddForce(randomDir * 40f, ForceMode2D.Impulse);
        return itemWorld;
    }
    public static bool ItemEquipedCheck(int id){
        Item itemhelmet = NetworkManager.instance.playerLocalInstance.GetComponent<PlayerEquipController>().helmetPlayerEquipe.GetComponent<Item>();
        Item itemWeapom = NetworkManager.instance.playerLocalInstance.GetComponent<PlayerEquipController>().weapomPlayerEquipe.GetComponent<Item>();
        if(id == itemhelmet.idItem) {
            print("DESEQUIPAR O ITEM ANTES DE USAR");
            return true;
        } else  if(id == itemWeapom.idItem) {
            print("DESEQUIPAR O ITEM ANTES DE USAR");
            return true;
        }
        return false;
    }

    public static Vector3 GetRandomDir() {
        return new Vector3(UnityEngine.Random.Range(-1f,1f), UnityEngine.Random.Range(-1f,1f)).normalized;
    }

    public ItemV2 item;
    private SpriteRenderer spriteRenderer;
    //private Light2D light2D;
    private TextMeshPro textMeshPro;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //light2D = transform.Find("Light").GetComponent<Light2D>();
        //textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
    }

    public void SetItem(ItemV2 item) {
        this.item = item;
        spriteRenderer.sprite = item.GetSprite(item.idItem);
        //light2D.color = item.GetColor();
        if (item.amount > 1) {
            textMeshPro.SetText(item.amount.ToString());
        } else {
            textMeshPro.SetText("");
        }
    }

    public ItemV2 GetItem() {
        return item;
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }

}
