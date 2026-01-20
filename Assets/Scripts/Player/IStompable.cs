using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStompable
{
    public void PlayerLanded(PlayerController player);
    public void PlayerLeft(PlayerController player);
}
