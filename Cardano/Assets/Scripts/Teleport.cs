using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Region region;

    private Fade fade;
    void Start()
    {
        fade = FindObjectOfType(typeof(Fade)) as Fade;
    }

    public void interacao(GameObject player){
        StartCoroutine("acionarPorta", player);
    }
    IEnumerator acionarPorta(GameObject player) {
        fade.fadeIn();
        yield return new WaitWhile(() => fade.fume.color.a < 0.9f);
        player.transform.position = region.warpLocation.position;
        yield return new WaitForSeconds(1f);
        fade.fadeOut();
    }
}