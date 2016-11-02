using UnityEngine;

namespace Assets.Scripts
{
    public abstract class AuthorityDistributor
    {
        public abstract float Authority { get; protected set; }

        public virtual void DistributeAuthority(Transform origin, float radius, TypeModel.TypeModelItem typeItem)
        {
            var boids = Physics.OverlapSphere(origin.position, radius);
            foreach (var boid in boids)
            {
                var behaviorComponent = boid.gameObject.GetComponent<BoidStateMachine>().State;
                if (behaviorComponent is IChangeType)
                {
                    (behaviorComponent as IChangeType).TryChangeType(typeItem, Authority);
                }
            }
        }
    }
}
