using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        #region bindings

        [SerializeField]
        private BoidStateMachine _boidPrototype;
        [SerializeField]
        private int _poolCapacity;
        [SerializeField]
        private float _maxUpdateTime;

        [SerializeField]
        private int _boidsAmount;
        [SerializeField]
        private Vector3 _habitatSize;
        [SerializeField]
        private Vector3 _leaderRotationDelta;
        [SerializeField]
        private TypeModel _typeModel;
        [SerializeField]
        private float _authorityRadius;
        [SerializeField]
        private float _scaleVariationPercent;

        #endregion

        private BoidFactory _boidFactory;
        private BoidsManager _boidsManager;
        private Habitat _habitat;

        private Transform _poolTransform;

        private void Awake()
        {
            _poolTransform = new GameObject("Pool").transform;
            _poolTransform.SetParent(transform);
            _boidFactory = new BoidFactory(_boidPrototype, _poolCapacity, _poolTransform);

            _boidsManager = new GameObject("BoidsManager", typeof(BoidsManager)).GetComponent<BoidsManager>();
            _boidsManager.transform.SetParent(transform);

            _habitat = new GameObject("Habitat", typeof(Habitat)).GetComponent<Habitat>();
            _habitat.transform.SetParent(transform);

            Reset();
        }

        public void Generate()
        {
            Reset();

            for (var i = 0; i < _boidsAmount; i++)
            {
                var boid = _boidFactory.CreateBoid();
                boid.transform.position = _habitat.GetRandomPosition();
                boid.transform.localScale = GenerateScale(boid.transform.localScale);
                boid.transform.SetParent(_habitat.transform);
                boid.Initialize(_habitat.GetExtents(), _leaderRotationDelta, GenerateTypeItem(), CalculateAuthorityRadius());
                _boidsManager.AddObject(boid);
            }

            _boidsManager.StartProcess();
        }

        private void Reset()
        {
            _boidsManager.StopProcess();
            _boidsManager.Initialize(_maxUpdateTime);
            _boidFactory.ReleaseBoids(_boidsManager.ReturnObjects());
            _habitat.Initialize(_habitatSize);
        }

        private TypeModel.TypeModelItem GenerateTypeItem()
        {
            var index = Random.Range(0, _typeModel.Length - 1);
            return _typeModel.GetItem(index);
        }

        private float CalculateAuthorityRadius()
        {
            //TODO
            return _authorityRadius;
        }

        private Vector3 GenerateScale(Vector3 originalScale)
        {
            var min = _scaleVariationPercent * originalScale;
            var x = Random.Range(min.x, originalScale.x);
            var y = Random.Range(min.y, originalScale.y);
            var z = Random.Range(min.z, originalScale.z);

            return new Vector3(x, y, z);
        }
    }
}
