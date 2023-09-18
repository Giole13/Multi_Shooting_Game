using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : Gun
{

    public override void SettingGun()
    {
        bulletSpeed = 30f;
        FiredDelayTime = 0.2f;
        IsFire2 = true;
        GunDamage = 1;
    }
}
