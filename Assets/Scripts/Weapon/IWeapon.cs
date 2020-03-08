using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    bool Fire();
    WeaponConfig Config { get; }

}
