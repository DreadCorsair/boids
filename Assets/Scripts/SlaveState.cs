using UnityEngine;

namespace Assets.Scripts
{
    public class SlaveState : AuthorityDistributor, IState, IChangeType
    {
        private const float ROTATION_SPEED_FACTOR = 20f;
        private const float COHESION_RADIUS = 10f;
        private const float SEPARATION_RADIUS = 5f;
        private const float SEPARATION_COEF = 1.20f;
        private const float MAX_SPEED = 6f;

        private BoidStateMachine _stateMachine;

        private Transform _transform;
        private Rigidbody _rigidbody;
        private Collider _thisCollider;

        private Vector3 _velocity;
        public TypeModel.TypeModelItem TypeItem { get; private set; }

        public override float Authority { get; protected set; }
        private float _authorityRadius;

        public SlaveState(BoidStateMachine stateMachine, float authorityRadius, TypeModel.TypeModelItem typeItem)
        {
            _stateMachine = stateMachine;
            _authorityRadius = authorityRadius;
            _transform = stateMachine.transform;
            _rigidbody = stateMachine.GetComponent<Rigidbody>();
            _thisCollider = stateMachine.GetComponent<Collider>();
            SetType(typeItem);
        }

        public void UpdatePhysics()
        {
            var position = _rigidbody.position + _velocity * Time.deltaTime;
            _rigidbody.MovePosition(position);

            var direction = Quaternion.LookRotation(_velocity);
            var rotation = Quaternion.RotateTowards(_rigidbody.rotation, direction, ROTATION_SPEED_FACTOR * Time.deltaTime);
            _rigidbody.MoveRotation(rotation);
        }

        public void UpdateLogic()
        {
            UpdateVelocity();
            DistributeAuthority(_transform, _authorityRadius, TypeItem);
        }

        public void DistributeAuthority()
        {
            throw new System.NotImplementedException();
        }

        public bool TryChangeType(TypeModel.TypeModelItem typeItem, float autority)
        {
            if (autority > Authority)
            {
                Authority = autority;
                SetType(typeItem);

                return true;
            }

            return false;
        }

        private void UpdateVelocity()
        {
            _velocity = Vector3.zero;
            var cohesion = Vector3.zero;
            var separation = Vector3.zero;
            var alignment = Vector3.zero;

            var cohesionCount = 0;
            var separationCount = 0;
            var position = _rigidbody.position;

            var boids = Physics.OverlapSphere(_transform.position, COHESION_RADIUS);
            foreach (var boid in boids)
            {
                if (boid.GetComponent<IChangeType>().TypeItem != TypeItem) continue;

                cohesion += boid.transform.position;
                cohesionCount++;

                var direction = position - boid.transform.position;
                var distance = direction.magnitude;
                if (boid.Equals(_thisCollider) || distance > SEPARATION_RADIUS) continue;
                separation += direction / distance;
                separationCount++;

                var behaviorComponent = boid.GetComponent<IState>();
                if (behaviorComponent is SlaveState)
                {
                    alignment += (behaviorComponent as SlaveState)._velocity;
                }
            }

            if (cohesionCount > 0)
                cohesion = cohesion / cohesionCount - position;
            if (separationCount > 0)
            {
                separation = (separation / separationCount) * SEPARATION_COEF;
                alignment /= separationCount;
            }

            _velocity = Vector3.ClampMagnitude(cohesion + separation + alignment, MAX_SPEED);
        }

        private void SetType(TypeModel.TypeModelItem typeItem)
        {
            TypeItem = typeItem;

            var meshRenderer = _stateMachine.GetComponent<MeshRenderer>();
            meshRenderer.material.color = TypeItem.Color;
        }


    }
}
