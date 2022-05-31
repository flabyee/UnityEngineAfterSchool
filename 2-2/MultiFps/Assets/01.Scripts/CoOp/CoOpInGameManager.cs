using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoOpInGameManager : InGameManager
{
    public override void StartCallback()
    {
        if(listOrder == 0)
        {
            CoOpZombieGenerator.Instance.StartGenerate();
        }
    }
}
