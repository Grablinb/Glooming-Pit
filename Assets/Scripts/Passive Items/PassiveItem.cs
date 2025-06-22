using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem : MonoBehaviour
{

    protected PlayerStats player;
    public PassiveItemScriptableObject passiveItemData;

    protected virtual void ApplyModifier()
    {
        //применение значение буста к нужной статистике в наследственных классах
    }

    void Start()

    {
        player = FindObjectOfType<PlayerStats>();
        ApplyModifier();
    
    }
}
