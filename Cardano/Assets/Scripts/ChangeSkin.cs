using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ChangeSkin : MonoBehaviour
{
    public int currentHair;
    public int currentLegs;
    public int currentCloak;
    public int currentBelt;
    public int currentEyes;
    public int currentBody;

    public GameObject currentHairImage;
    public GameObject currentLegsImage;
    public GameObject currentCloakImage;
    public GameObject currentBeltImage;
    public GameObject currentEyesImage;
    public GameObject currentBodyImage;
    public Sprite[] HairSprites;
    public Sprite[] LegsSprites;
    public Sprite[] CloakSprites;
    public Sprite[] BeltSprites;
    public Sprite[] EyesSprites;
    public Sprite[] BodySprites;
    public Dictionary<string, Sprite> spriteSheet;
    // Start is called before the first frame update
    void Start()
    {
        currentHair = 0;
        currentLegs = 0;
        currentCloak = 0;
        currentBelt = 0;
        currentEyes = 0;
        currentBody = 0;
        HairSprites = Resources.LoadAll<Sprite>("Cabelo") ;
        LegsSprites = Resources.LoadAll<Sprite>("Calca") ;
        CloakSprites = Resources.LoadAll<Sprite>("Capa") ;
        BeltSprites = Resources.LoadAll<Sprite>("Cinto") ;
        EyesSprites = Resources.LoadAll<Sprite>("Olhos") ;
        BodySprites = Resources.LoadAll<Sprite>("Roupa") ;

        spriteSheet = BodySprites.ToDictionary(x=> x.name, x=> x);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NextSkin(string part) {
        switch (part)
        {
            case "HAIR":
                if(currentHair< (HairSprites.Length-1)) {
                    currentHair++;
                    Sprite sp = HairSprites[currentHair];
                    currentHairImage.GetComponent<Image>().sprite = sp;
                }
            break;
            case "LEGS":
                if(currentLegs < (LegsSprites.Length-1)) {
                    currentLegs++;
                    Sprite sp = LegsSprites[currentLegs];
                    currentLegsImage.GetComponent<Image>().sprite = sp;
                }
            break;
            case "CLOAK":
                if(currentCloak < (CloakSprites.Length-1)) {
                    currentCloak++;
                    Sprite sp = CloakSprites[currentCloak];
                    currentCloakImage.GetComponent<Image>().sprite = sp;
                }
            break;
            case "BELT":
                if(currentBelt < (BeltSprites.Length-1)) {
                    currentBelt++;
                    Sprite sp = BeltSprites[currentBelt];
                    currentBeltImage.GetComponent<Image>().sprite = sp;
                }
            break;
            case "EYES":
                if(currentEyes < (EyesSprites.Length-1)) {
                    currentEyes++;
                    Sprite sp = EyesSprites[currentEyes];
                    currentEyesImage.GetComponent<Image>().sprite = sp;
                }
            break;
            case "BODY":
                if(currentBody < (BodySprites.Length-1)) {
                    currentBody++;
                    Sprite sp = BodySprites[currentBody];
                    currentBodyImage.GetComponent<Image>().sprite = sp;
                }
            break;
        }
    }
    public void PreviousSkin(string part) {
        switch (part)
        {
            case "HAIR":
                 if(currentHair > 0) {
                    currentHair--;
                    Sprite sp = HairSprites[currentHair];
                    currentHairImage.GetComponent<Image>().sprite = sp;
                }
            break;
            case "LEGS":
                 if(currentLegs > 0) {
                    currentLegs--;
                    Sprite sp = LegsSprites[currentLegs];
                    currentLegsImage.GetComponent<Image>().sprite = sp;
                }
            break;
            case "CLOAK":
                 if(currentCloak > 0) {
                    currentCloak--;
                    Sprite sp = CloakSprites[currentCloak];
                    currentCloakImage.GetComponent<Image>().sprite = sp;
                }
            break;
            case "BELT":
                 if(currentBelt > 0) {
                    currentBelt--;
                    Sprite sp = BeltSprites[currentBelt];
                    currentBeltImage.GetComponent<Image>().sprite = sp;
                }
            break;
            case "EYES":
                 if(currentEyes > 0) {
                    currentEyes--;
                    Sprite sp = EyesSprites[currentEyes];
                    currentEyesImage.GetComponent<Image>().sprite = sp;
                }
            break;
            case "BODY":
                if(currentBody > 0) {
                    currentBody--;
                    Sprite sp = BodySprites[currentBody];
                    currentBodyImage.GetComponent<Image>().sprite = sp;
                }
            break;
        }
        
    }

}
