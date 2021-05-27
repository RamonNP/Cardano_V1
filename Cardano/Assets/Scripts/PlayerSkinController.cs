using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkinController : MonoBehaviour
{
    public GameObject changeSkinPainel;
    private ChangeSkin changeSkin;
    public GameObject SkCabelo;
    public GameObject SkCalca;
    public GameObject SkCapa;
    public GameObject SkCinto;
    public GameObject SkOlhos;
    public GameObject SkBody;
    private bool skinAtivo;
    // Start is called before the first frame update
    void Start()
    {
        //changeSkinPainel.SetActive(false); 
        changeSkin = changeSkinPainel.GetComponent<ChangeSkin>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.P))
        {
            skinAtivo = !skinAtivo;
            changeSkinPainel.SetActive(skinAtivo);
        }
        SkCabelo.GetComponent<SpriteRenderer>().sprite = changeSkin.currentHairImage.GetComponent<Image>().sprite;
        SkCalca.GetComponent<SpriteRenderer>().sprite = changeSkin.currentLegsImage.GetComponent<Image>().sprite;
        SkCapa.GetComponent<SpriteRenderer>().sprite = changeSkin.currentCloakImage.GetComponent<Image>().sprite;
        SkCinto.GetComponent<SpriteRenderer>().sprite = changeSkin.currentBeltImage.GetComponent<Image>().sprite;
        SkOlhos.GetComponent<SpriteRenderer>().sprite = changeSkin.currentEyesImage.GetComponent<Image>().sprite;
        SkBody.GetComponent<SpriteRenderer>().sprite = changeSkin.currentBodyImage.GetComponent<Image>().sprite;
    }
}
