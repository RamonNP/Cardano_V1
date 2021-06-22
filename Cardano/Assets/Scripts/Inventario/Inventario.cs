using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventario : MonoBehaviour
{
    [Header("Itens Detail")]
    public Image imageItem;
    public Text textDefense;
    public Text textAtack;
    private Player player;
    public PlayerController playerController;
    public GameObject inventario;
    public Button[] slot;
    public Image[] iconItem;

    public Text qtdPorcao, qtdMana, qtdFlechaA;
    public int qPorcao, qMana, qFlechaA;

    public List<string> itemInventario;
    public List<GameObject> itensCarregados;

    void Start()
    {
        slot = new Button[inventario.transform.GetChild(0).transform.childCount];
        iconItem = new Image[inventario.transform.GetChild(0).transform.childCount];
        player = gameObject.GetComponent<Player>();
        playerController = gameObject.GetComponent<PlayerController>();
        
        int i = 0;
        foreach (Transform child in inventario.transform.GetChild(0).transform) {
            //print(child.name);
            slot[i] = child.gameObject.GetComponent<Button>();
            iconItem[i] = child.GetChild(0).gameObject.GetComponent<Image>();
            i += 1;
        }
        //carregarInventario();
    }

    public void carregarInventario()
    {
        limparItensCarregados();

        foreach (Button b in slot)
        {
            b.interactable = false;
        }

        foreach (Image i in iconItem)
        {
            i.sprite = null;
            i.gameObject.SetActive(false);
        }

        qtdPorcao.text = "x " + player.qtdPocoes.ToString();
        qtdMana.text = "x" + player.qtdMana.ToString();
        qtdFlechaA.text = "x" + player.qtdFlechas.ToString();


        int s = 0; // ID DO SLOT
        foreach(string i in itemInventario)
        {
            print(i);
            GameObject temp = (GameObject)Instantiate(Resources.Load(i));
            Item itemInfo = temp.GetComponent<Item>();

            itensCarregados.Add(temp);

            slot[s].GetComponent<SlotInventario>().objetoSlot = temp;
            slot[s].interactable = true;
            switch (itemInfo.itemType)
            {
                case ItemType.HELMET : 
                    iconItem[s].sprite = playerController.HelmetSprites[itemInfo.idItem];
                break;
                case ItemType.SWORD : 
                    iconItem[s].sprite = temp.GetComponent<SpriteRenderer>().sprite;
                break;
                case ItemType.BEAST : 
                    iconItem[s].sprite = temp.GetComponent<SpriteRenderer>().sprite;
                break;
            }
            //carregar do resources 
            //iconItem[s].sprite = playerController.imgInventario[itemInfo.idItem];
            iconItem[s].gameObject.SetActive(true);
            s++;
        }
    }

    public void limparItensCarregados()
    {
        foreach (GameObject ic in itensCarregados)
        {
            Destroy(ic);
        }
        itensCarregados.Clear();
    }
    public void dropItem(string path)
    {
        print("Drop "+path);
        string destroy = null;
        foreach (string ic in itemInventario)
        {
            if(ic == path){
                destroy = ic;
            }
        }
        itemInventario.Remove(destroy);
        //Destroy(destroy);
        carregarInventario();
    }
}
