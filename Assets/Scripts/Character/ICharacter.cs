using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    public event Action Enabled;
    public event Action Disabled;
    public event Action Started;
    public event Action Updated;
}

