using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    public void OnBagEvent()
    {
        ItemBox.instance.AddManySlot(5);
        gameObject.SetActive(false);
    }
}
