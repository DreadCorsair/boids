using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class BehavioralObject : MonoBehaviour, IBehavioralObject
{
    private IBehavioralObject _behaviour;

    public void UpdateBehavior()
    {
        _behaviour.UpdateBehavior();
    }
}
