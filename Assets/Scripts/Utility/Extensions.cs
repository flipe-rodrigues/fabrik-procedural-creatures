using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    public static class Extensions
    {
        public static T GetFirstComponentInParents<T>(this MonoBehaviour monoBehaviour) where T : Component
        {
            Transform transform = monoBehaviour.transform;

            while (transform.parent != null)
            {
                if (transform.TryGetComponent<T>(out T component))
                {
                    return component;
                }

                transform = transform.parent;
            }

            return null;
        }

        public static T[] GetComponentsInParentsExclusively<T>(this MonoBehaviour monoBehaviour) where T : Component
        {
            return monoBehaviour.GetComponentsInParent<T>().Where(c => c.transform != monoBehaviour.transform).ToArray();
        }

        public static T[] GetComponentsInChildrenExclusively<T>(this MonoBehaviour monoBehaviour) where T : Component
        {
            return monoBehaviour.GetComponentsInChildren<T>().Where(c => c.transform != monoBehaviour.transform).ToArray();
        }

        public static T[] GetComponentsInImmediateParent<T>(this MonoBehaviour monoBehaviour) where T : Component
        {
            return monoBehaviour.GetComponentsInParent<T>().Where(c => c.transform == monoBehaviour.transform.parent).ToArray();
        }

        public static T[] GetComponentsInImmediateChildren<T>(this MonoBehaviour monoBehaviour) where T : Component
        {
            return monoBehaviour.GetComponentsInChildren<T>().Where(c => c.transform.parent == monoBehaviour.transform).ToArray();
        }

        public static Vector3 Mean(this Vector3[] vectors)
        {
            Vector3 mean = Vector3.zero;

            foreach (Vector3 vector in vectors)
            {
                mean += vector / vectors.Length;
            }

            return mean;
        }

        public static Vector3 Mean(this IEnumerable<Vector3> vectors)
        {
            Vector3 mean = Vector3.zero;

            foreach (Vector3 vector in vectors)
            {
                mean += vector / vectors.Count();
            }

            return mean;
        }

        public static Vector3 MidPoint(this IEnumerable<Vector3> vectors)
        {
            Vector3 median = new Vector3();

            median.x = vectors.Select(v => v.x).Distinct().ToArray().Average();
            median.y = vectors.Select(v => v.y).Distinct().ToArray().Average();
            median.z = vectors.Select(v => v.z).Distinct().ToArray().Average();

            return median;
        }

        public static float Median(this IEnumerable<float> floats)
        {
            int count = floats.Count();

            floats = floats.OrderBy(f => f);

            int midPoint = count / 2;

            if (count % 2 == 0)
                return (floats.ElementAt(midPoint - 1) + floats.ElementAt(midPoint)) / 2f;
            else
                return floats.ElementAt(midPoint);
        }

        public static Vector3 ElementWiseMultiplication(this Vector3 v, Vector3 u)
        {
            return new Vector3(v.x * u.x, v.y * u.y, v.z * u.z);
        }

        public static Vector3 ClampBetween(this Vector3 v, Vector3 min, Vector3 max)
        {
            return new Vector3(Mathf.Clamp(v.x, min.x, max.x), Mathf.Clamp(v.y, min.y, max.y), Mathf.Clamp(v.z, min.z, max.z));
        }

        public static Color SetAlpha(this Color color, float alpha)
        {
            color.a = alpha;

            return color;
        }

        public static Vector3 AverageTentativePosition(this JointBhv[] joints)
        {
            Vector3 average = Vector3.zero;

            foreach(JointBhv joint in joints)
            {
                average += joint.TentativePosition / joints.Length;
            }

            return average;
        }
    }
}
