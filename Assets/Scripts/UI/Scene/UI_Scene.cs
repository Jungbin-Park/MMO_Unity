using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene : UI_Base
{
    public virtual void Init()
    {
        // Sorting ¾ÈÇÔ
        Managers.UI.SetCanvas(gameObject, false);
    }
}

