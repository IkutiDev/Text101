﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    Normal,
    GainHealth,
    GainGun,
    GainLoot,
    FightGuard,
    FightLeader,
    Hit,
    Fail,
    Branching,
    TimeLimit,
    Start
}