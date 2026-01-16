using UnityEngine;
using UnityEngine.Rendering;

public class DoorController : MonoBehaviour
{
    // Kuvat oven eri tiloille
    [SerializeField]
    Sprite ClosedDoorSprite;
    [SerializeField]
    Sprite OpenDoorSprite;
    [SerializeField]
    Sprite LockedSprite;
    [SerializeField]
    Sprite UnlockedSprite;
    public enum OvenTila
    {
        Auki,
        Kiinni,
        Lukossa
    }
    public enum OvenToiminto
    {
        Sulje,
        Lukitse,
        Avaa,
        AvaaLukko
    }
    OvenTila oven_tila;

    BoxCollider2D colliderComp;

    // Näitä värejä käytetään lukkosymbolin piirtämiseen.
    public static Color lockedColor;
    public static Color openColor;

    SpriteRenderer doorSprite; // Oven kuva
    SpriteRenderer lockSprite; // Lapsi gameobjectissa oleva lukon kuva

    // Debug ui
    [SerializeField]
    bool ShowDebugUI;
    [SerializeField]
    int DebugFontSize = 32;


    void Start()
    {
        doorSprite = GetComponent<SpriteRenderer>();
        colliderComp = GetComponent<BoxCollider2D>();
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        if (sprites.Length == 2 && sprites[0] == doorSprite)
        {
            lockSprite = sprites[1];
        }

        
        lockedColor = new Color(1.0f, 0.63f, 0.23f);
        openColor = new Color(0.5f, 0.8f, 1.0f);


        // TODO
        // missä tilassa ovi on kun peli alkaa?
        oven_tila = OvenTila.Lukossa;
    }

    /// <summary>
    /// Oveen kohdistuu jokin toiminto joka muuttaa sen tilaa
    /// </summary>
    public void ReceiveAction(OvenToiminto oven_toiminto)
    {
        switch (oven_toiminto)
        {
            case OvenToiminto.AvaaLukko:
                if (oven_tila == OvenTila.Lukossa)
                {
                    oven_tila = OvenTila.Kiinni;
                    UnlockDoor();
                }
                break;
            case OvenToiminto.Avaa:
                if (oven_tila == OvenTila.Kiinni)
                {
                    oven_tila = OvenTila.Auki;
                    OpenDoor();
                }
                break;
            case OvenToiminto.Sulje:
                if (oven_tila == OvenTila.Auki)
                {
                    oven_tila = OvenTila.Kiinni;
                    CloseDoor();
                }
                break;
            case OvenToiminto.Lukitse:
                if (oven_tila == OvenTila.Kiinni)
                {
                    oven_tila = OvenTila.Lukossa;
                    LockDoor();
                }
                break;
        }
    }

    // Kun tulee toiminto, sen perusteella kutsutaan jotakin
    // näistä funktioista että oven tila muuttuu

    /// <summary>
    /// Vaihtaa oven kuvan avoimeksi oveksi
    /// ja laittaa törmäysalueen pois päältä
    /// </summary>
    private void OpenDoor()
    {
        doorSprite.sprite = OpenDoorSprite;
        colliderComp.isTrigger = true;
    }

    /// <summary>
    /// Vaihtaa oven kuvan suljetuksi oveksi
    /// ja laittaa törmäysalueen päälle
    /// </summary>
    private void CloseDoor()
    {
        doorSprite.sprite = ClosedDoorSprite;
        colliderComp.isTrigger = false;
    }

    /// <summary>
    /// Vaihtaa lukkosymbolin lukituksi ja
    /// vaihtaa sen värin
    /// </summary>
    private void LockDoor()
    {
        lockSprite.sprite = LockedSprite;
        lockSprite.color = lockedColor;
    }

    /// <summary>
    /// Vaihtaa lukkosymbolin avatuksi ja
    /// vaihtaa sen värin
    /// </summary>
    private void UnlockDoor()
    {
        lockSprite.sprite = UnlockedSprite;
        lockSprite.color = openColor;
    }

    // *********************************
    // Unityssä on välittömän käyttöliittymän
    // järjestelmä, jolla voi piirtää 
    // nappeja ja tekstiä koodin avulla.
    // Se on kätevä erilaisten oikoteiden ja
    // testaamisen apuvälineiden kehittämiseen.
    // Tässä sitä on käytetty tekemään napit, joilla
    // voi testata että oven eri toiminnot
    // toimivat oikein.


  

    // Unity kutsuu tätä funktiota kaiken muun piirtämisen jälkeen
    // Sen sisällä voi piirtää käyttöliittymää
    private void OnGUI()
    {
        if (ShowDebugUI == false)
        {
            return;
        }
        GUIStyle buttonStyle = GUI.skin.GetStyle("button");
        GUIStyle labelStyle = GUI.skin.GetStyle("label");
        buttonStyle.fontSize = DebugFontSize;
        labelStyle.fontSize = DebugFontSize;
        Rect guiRect = GetGuiRect();
        GUILayout.BeginArea(guiRect);
        
        GUILayout.Label("Door");
        if (GUILayout.Button("Open"))
        {
            OpenDoor();
        }
        if (GUILayout.Button("Close"))
        {
            CloseDoor();
        }
        if (GUILayout.Button("Lock"))
        {
            LockDoor();
        }
        if (GUILayout.Button( "Unlock"))
        {
            UnlockDoor();
        }
        
        GUILayout.EndArea();
    }

    // Näiden kahden funktion avulla ei tarvitse itse
    // määrittää jokaisen napin paikkaa, vaan ne
    // ladotaan automaattisesti allekkain.
   

    private Rect GetGuiRect()
    {
        Vector3 buttonPos = transform.position;
        buttonPos.x += 1;
        buttonPos.y -= 0.25f;
        // Tällä tavalla voi laskea paikan jossa GameObject näkyy
        // ruudulla ja piirtää käyttöliittymän sen kohdalle.
        // WorldToScreenPoint antaa Y koordinaatin väärin päin
        // tai GUI koodi ymmärtää sen väärin,
        // ja siksi se pitä vähentää ruudun korkeudesta.
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(buttonPos);
        float screenHeight = Screen.height;
        return new Rect(screenPoint.x, screenHeight - screenPoint.y, 
            DebugFontSize * 8,  // Leveys ja korkeus niin että varmasti mahtuu
            DebugFontSize * 100);
    }    
}
