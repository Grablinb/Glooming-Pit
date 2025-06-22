using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeController : WeaponController {
    protected override void Start()
    {
        base.Start();
    }
    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedKnife = Instantiate(weaponData.Prefab);
        spawnedKnife.transform.position = transform.position; //позиция как у этого же объекта, который является дочерним игроку
        spawnedKnife.GetComponent<KnifeBehaviour>().DirectionChecker(pm.lastMovedVector); //направление ножа
        spawnedKnife.transform.parent = transform;
    }
}
