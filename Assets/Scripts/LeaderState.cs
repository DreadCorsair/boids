using UnityEngine;

namespace Assets.Scripts
{
    public class LeaderState : AuthorityDistributor, IState, IChangeType
    {
        private readonly BoidStateMachine _stateMachine;

        private const float MOVE_SPEED_FACTOR = 1f;
        private const float ROTATE_SPEED_FACTOR = 10f;
        private const float AVOIDING_COLLISION_SPEED_FACTOR = 30;

        private readonly Transform _transform;
        private Rigidbody _rigidbody;
        private Vector3 _habitatSize;
        private Vector3 _rotationDelta;
        public TypeModel.TypeModelItem TypeItem { get; private set; }

        private Quaternion _direction;

        public override float Authority { get; protected set; }

        private readonly float _authorityRadius;

        public LeaderState(BoidStateMachine stateMachine, Vector3 habitatSize, Vector3 rotationDelta, TypeModel.TypeModelItem typeItem, float authorityRadius)
        {
            _stateMachine = stateMachine;
            _transform = stateMachine.transform;
            _rigidbody = stateMachine.GetComponent<Rigidbody>();
            _habitatSize = habitatSize;
            _rotationDelta = rotationDelta;
            Authority = CalculateAuthority();
            _authorityRadius = authorityRadius;
            SetType(typeItem);
        }

        public void UpdatePhysics()
        {
            _rigidbody.MovePosition(_transform.position + _transform.forward * MOVE_SPEED_FACTOR * Time.deltaTime);

            var direction = IsCloseToBorder() ? Quaternion.LookRotation(-_transform.position) : _direction;
            var speed = IsCloseToBorder() ? AVOIDING_COLLISION_SPEED_FACTOR : ROTATE_SPEED_FACTOR;
            _rigidbody.MoveRotation(Quaternion.RotateTowards(_transform.rotation, direction, speed * Time.deltaTime));
        }

        public void UpdateLogic()
        {
            ChangeDirection();
            DistributeAuthority(_transform, _authorityRadius, TypeItem);
        }

        public bool TryChangeType(TypeModel.TypeModelItem typeItem, float autority)
        {
            if (autority > Authority)
            {
                Authority = autority;
                SetType(typeItem);

                _stateMachine.State = new SlaveState(_stateMachine, _authorityRadius, TypeItem);

                return true;
            }

            return false;
        }

        private void ChangeDirection()
        {
            var euler = _transform.rotation.eulerAngles;
            euler.x += Random.Range(-_rotationDelta.x, _rotationDelta.x);
            euler.y += Random.Range(-_rotationDelta.y, _rotationDelta.y);
            euler.z += Random.Range(-_rotationDelta.z, _rotationDelta.z);
            _direction = Quaternion.Euler(euler);
        }

        private bool IsCloseToBorder()
        {
            return
                _transform.position.x > _habitatSize.x || _transform.position.x < -_habitatSize.x ||
                _transform.position.y > _habitatSize.y || _transform.position.y < -_habitatSize.y ||
                _transform.position.z > _habitatSize.z || _transform.position.z < -_habitatSize.z;
        }

        private void SetType(TypeModel.TypeModelItem typeItem)
        {
            TypeItem = typeItem;

            var meshRenderer = _stateMachine.GetComponent<MeshRenderer>();
            meshRenderer.material.color = TypeItem.Color;
        }

        private float CalculateAuthority()
        {
            return 1f / 3f * Mathf.PI * Mathf.Sqrt(_transform.localScale.x / 2f) * _transform.localScale.z;
        }
    }
}
