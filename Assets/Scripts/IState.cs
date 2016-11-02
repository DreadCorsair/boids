using UnityEngine;

namespace Assets.Scripts
{
    public interface IState
    {
        void UpdatePhysics();
        void UpdateLogic();
    }
}
