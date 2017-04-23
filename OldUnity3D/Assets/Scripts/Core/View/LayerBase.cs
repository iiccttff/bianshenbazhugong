using System;
using System.Collections.Generic;
using UnityEngine;

public class LayerBase : UIBase
{
    /// <summary>
    /// layer中child z方向间隔默认值
    /// </summary>
    public const float DEFAULT_Z_SPACE_BETWEEN_CHILD = 20f;
    /// <summary>
    /// z方向上所占用的空间
    /// </summary>
    /// <returns></returns>
    public virtual float zSpace
    {
        get
        {
            return DEFAULT_Z_SPACE_BETWEEN_CHILD;
        }
    }
}
