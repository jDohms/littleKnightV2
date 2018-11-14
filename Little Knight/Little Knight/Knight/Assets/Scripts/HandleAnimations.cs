using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleAnimations : MonoBehaviour {

    public List<AnimationsBase> AnimationsList = new List<AnimationsBase>();
}	

[System.Serializable]
public class AnimationsBase
{
    public string playerAnimName;
    public string enemAnimName;
    public float offset;
}
