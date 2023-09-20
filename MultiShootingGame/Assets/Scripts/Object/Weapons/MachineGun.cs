using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : Gun
{
    public override void SettingGun()
    {
        gunSpec = new GunSpecifications(1, 30f, 0.2f, 100);
    }
}
