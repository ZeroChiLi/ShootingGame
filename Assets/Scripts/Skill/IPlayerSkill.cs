using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerSkill 
{
    bool Init(object context);
    void Update(float curTime);
    bool Play();
    bool ReadyToPlay();
}
