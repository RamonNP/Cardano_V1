using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PainelItemInfo : MonoBehaviour
{
    //private _GameController _GameController;
    private int idArma;
    private int aprimoramento;

    [Header("Id Slots")]
    public int idSlot;

    [Header("GameObjets")]
    public GameObject objetoSlot;
    public GameObject[] aprimoramentos;

    [Header("HUD")]
    public Image imgItem;
    public Sprite imgItem2;
    public Text nomeItem;
    public Text atackValue;
    public Text defenseValue;

    [Header("Botoes")]
    public Button btnAprimorar;
    public Button btnEquipar;
    public Button btnExcluir;

    // Start is called before the first frame update
    void Start()
    {
        //_GameController = FindObjectOfType(typeof(_GameController)) as _GameController;
    }

    // Update is called once per frame
    void Update()
    {

    }



    public void carregarInfoItem() {

        Item itemInfo = objetoSlot.GetComponent<Item>();
        idArma = itemInfo.idItem;
        imgItem.sprite = objetoSlot.GetComponent<SpriteRenderer>().sprite;
        imgItem2 = objetoSlot.GetComponent<SpriteRenderer>().sprite;
        nomeItem.text = itemInfo.itemName;
        atackValue.text = itemInfo.atack.ToString();
        defenseValue.text = itemInfo.defense.ToString();
        
        carregarAprimoramento();

        if (idSlot==0) {
            btnEquipar.interactable = false;
            btnExcluir.interactable = false;
        }
        else
        {
            //int idClasseArma = _GameController.idClasseArma[idArma];
            //int idClassePersonagem = _GameController.idClasse[_GameController.idPersonagem];

            //if (idClasseArma== idClassePersonagem)
            //{
            //   btnEquipar.interactable = true;
            //}
            //else {
            //    btnEquipar.interactable = false;
            //}



        }
        btnEquipar.interactable = true;
        btnExcluir.interactable = true;



    }


         public void  botaoAprimorar()
    {
        //_GameController.aprimorarArma(idArma);
        carregarAprimoramento();

    }



    public void botaoEquipar()
    {
        objetoSlot.SendMessage("usarItem", SendMessageOptions.DontRequireReceiver);
        //_GameController.swap(idSlot);

    }
    public void removerEquipamento()
    {
        objetoSlot.SendMessage("removerItem", SendMessageOptions.DontRequireReceiver);
        objetoSlot = null;
        //_GameController.swap(idSlot);

    }





    public void dropItem()
    {
        GetComponent<Inventario>().dropItem(objetoSlot.GetComponent<Item>().pathItem);
        removerEquipamento();
    }



    public void carregarAprimoramento() {
/*
         aprimoramento = _GameController.aprimoramentoArma[idArma];

        if (aprimoramento >= 10) {
            btnAprimorar.interactable = false;
        }
        else
        {
            btnAprimorar.interactable = true;
        }


        foreach (GameObject a in aprimoramentos)
        {

            a.SetActive(false);
        }
        for (int i = 0; i < aprimoramento; i++)
        {

            aprimoramentos[i].SetActive(true);

        }*/
    } 

}
