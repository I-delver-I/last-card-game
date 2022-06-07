using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageCards : MonoBehaviour
{
    public GameObject card;

    void Start() 
    {
        DisplayCards();
    }

    public void DisplayCards()
    {
        AddCard(0);
    }

    private void AddCard(int rank)
    {
        GameObject c = Instantiate(card, new Vector3(0, 0, 0), Quaternion.identity);
    }
}
