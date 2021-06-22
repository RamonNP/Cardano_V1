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
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler {

    public void OnDrop(PointerEventData eventData) {
        Debug.Log("EVENTO "+eventData.pointerEnter.gameObject.name);
        Debug.Log("OBJETO LOCAL "+gameObject.name);
        //Debug.Log("OnDrop"+eventData.pointerDrag.gameObject.name);
        if (eventData.pointerDrag != null) {
            //eventData.pointerDrag.GetComponent<>();
            print(eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition);
            print( GetComponent<RectTransform>().anchoredPosition);
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        }
    }

}
