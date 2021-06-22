using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipController : MonoBehaviour
{
    public GameObject helmetPlayerEquipe;
    public GameObject helmetPlayerInventario;
    public GameObject weapomPlayerEquipe;
    public GameObject weapomPlayerInventario;
    public GameObject shieldPlayerEquipe;
    public GameObject armordPlayerEquipe;
    //public GameObject shieldPlayerInventario;
    public GameObject equip;

    private void Start() {
        equip = NetworkManager.instance.equip;
        //weapomPlayerEquipe = equip.transform.Find("EquipeSword").gameObject;
        weapomPlayerInventario = equip.transform.Find("WeapomPlayerInventario").gameObject;
        weapomPlayerInventario.SetActive(false);
    }

    public void RemoveHelmet() {
        helmetPlayerEquipe.SetActive(false);
        helmetPlayerInventario.SetActive(false);
    }
    public void RemoveWeapom() {
        weapomPlayerEquipe.SetActive(false);
        weapomPlayerInventario.SetActive(false);
    }
}
