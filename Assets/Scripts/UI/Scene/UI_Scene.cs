using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene : UI_Base
{
    public override void Init()
    {
        // Sorting ����
        Managers.UI.SetCanvas(gameObject, false);
    }
}

