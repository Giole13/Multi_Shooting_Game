using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 거너
public class Guner : Player
{
    protected override void Init()
    {
        stats = new Stats(10, 1, 15f);
    }
}
