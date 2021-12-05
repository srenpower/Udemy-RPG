using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// removing monobehaviour and adding serializable makes it impossible to add this as a component in unity
// instead it is added as an variable (in this case into BattleManager)
public class BattleMove
{
    public string moveName;
    public int movePower;
    public int moveCost; // determines mp cost for move (for player)
    public AttackEffect theEffect; // what animation plays on attack
}
