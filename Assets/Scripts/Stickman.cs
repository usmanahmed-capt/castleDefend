using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stickman : MonoBehaviour
{
    public void OnClickStickMan()
    {
        if(DataManager.Instance.gData.DemolationPurchased)
        {
            UIManager.Instance.OnClickBomberman();
            SpawnManager.Instance.stickMan.Remove(gameObject);
            Destroy(gameObject);
        }
      
    }
}
