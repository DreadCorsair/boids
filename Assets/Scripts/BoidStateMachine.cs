using UnityEngine;

namespace Assets.Scripts
{
    public class BoidStateMachine : MonoBehaviour
    {
        public IState State;

        private float _authorityRadius;

        public void Initialize(Vector3 habitatSize, Vector3 rotationDelta, TypeModel.TypeModelItem typeItem, float authorityRadius)
        {
            _authorityRadius = authorityRadius;
            State = new LeaderState(this, habitatSize, rotationDelta, typeItem, authorityRadius);
        }

        public void UpdateLogic()
        {
            State.UpdateLogic();
        }

        private void FixedUpdate()
        {
            State.UpdatePhysics();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _authorityRadius);
        }
    }
}
