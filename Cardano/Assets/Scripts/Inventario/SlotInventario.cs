using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotInventario : MonoBehaviour
{
    public GameObject objetoSlot;
    //private _GameController _GameController;
    private PainelItemInfo painelItemInfo;

    [Header("Id Slots")]
    public int idSlot;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(UsarItem);
        //_GameController = FindObjectOfType(typeof(_GameController)) as _GameController;
        //painelItemInfo = FindObjectOfType(typeof(PainelItemInfo)) as PainelItemInfo;
        painelItemInfo = NetworkManager.instance.playerLocalInstance.GetComponent<PainelItemInfo>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UsarItem() {
         print("Usei Item");
        if (objetoSlot != null) {
            //objetoSlot.SendMessage("usarItem", SendMessageOptions.DontRequireReceiver);

            painelItemInfo.objetoSlot=objetoSlot;
            painelItemInfo.idSlot = idSlot;

            painelItemInfo.carregarInfoItem();
            //_GameController.openItemInfo();

        }
    }
}
