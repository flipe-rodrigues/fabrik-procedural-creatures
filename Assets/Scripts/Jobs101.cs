using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

namespace ProceduralAnimation
{
    public struct JointUpdateJob : IJobParallelFor
    {
        public NativeArray<JointBhv.Data> JointDataArray;

        public void Execute(int index)
        {
            var data = JointDataArray[index];

            data.Update();

            JointDataArray[index] = data;
        }
    }

    public class JointJobManager : MonoBehaviour
    {
        [SerializeField] private List<JointBhv> _joints;

        private NativeArray<JointBhv.Data> _jointDataArray;
        private JointUpdateJob _job;

        private void Awake()
        {
            _jointDataArray = new NativeArray<JointBhv.Data>(_joints.Count, Allocator.Persistent);

            for (int i = 0; i < _joints.Count; i++)
            {
                _jointDataArray[i] = new JointBhv.Data(_joints[i]);
            }

            _job = new JointUpdateJob
            {
                JointDataArray = _jointDataArray
            };            
        }

        private void Update()
        {
            JobHandle jobHandle = _job.Schedule(_joints.Count, 1);

            jobHandle.Complete();
        }

        private void OnDestroy()
        {
            _jointDataArray.Dispose();
        }
    }
}
