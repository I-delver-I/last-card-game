using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool tileRevealed = false;
    public Sprite originalSprite;
    public Sprite hiddenSprite;

    public void OnMouseDown() 
    {
        print("You pressed on tile");

        if (tileRevealed)
            hideCard();
        else
            revealCard();
    }

    public void hideCard()
    {
        GetComponent<SpriteRenderer>().sprite = hiddenSprite;
        tileRevealed = false;
    }

    public void revealCard()
    {
        GetComponent<SpriteRenderer>().sprite = originalSprite;
        tileRevealed = true;
    }
}
