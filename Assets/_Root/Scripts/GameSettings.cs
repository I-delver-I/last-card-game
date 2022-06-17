using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Setting", menuName = "Game settings")]
public class GameSettings : ScriptableObject
{
    public int BotsCount = 1;
    public int InitialCardsCount = 4;
    public int MaximalScore = 20;
}
