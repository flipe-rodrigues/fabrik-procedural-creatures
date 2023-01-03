using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralAnimation
{
    public class PulsatingChainBhv : MonoBehaviour
    {
        public AnimationCurve curve;
        public AnimationCurve riseCurve;
        public AnimationCurve fallCurve;
        [ColorUsage(false, true)]
        public Color emissionColor = Color.white;
        public float speed = 100;
        public float riseSpeed = 100;
        public float fallSpeed = 25;

        private ChainBhv _chain;

        private void Awake()
        {
            _chain = this.GetComponent<ChainBhv>();
        }

        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(this.PulsateCoroutine());

                //StartCoroutine(this.FadeIn());

                //StartCoroutine(this.FadeOut());
            }
        }

        private IEnumerator PulsateCoroutine()
        {
            for (int i = 0; i < _chain.Links.Length; i++)
            {
                _chain.Links[i].fadeSpeed = fallSpeed;

                float lerp = 0;

                while (lerp <= 1)
                {
                    Color lerpColor = Color.Lerp(Color.clear, emissionColor, curve.Evaluate(lerp));

                    _chain.Links[i].MeshRenderer.material.SetColor("_EmissionColor", lerpColor);

                    lerp += Time.deltaTime * riseSpeed;

                    yield return null;
                }

                _chain.Links[i].MeshRenderer.material.SetColor("_EmissionColor", emissionColor);
            }

            //for (int i = 0; i < _chain.Links.Length; i++)
            //{
            //    float lerp = 0;

            //    while (lerp <= 1)
            //    {
            //        Color lerpColor = Color.Lerp(emissionColor, Color.clear, curve.Evaluate(lerp));

            //        _chain.Links[i].MeshRenderer.material.SetColor("_EmissionColor", lerpColor);

            //        lerp += Time.deltaTime * fallSpeed;

            //        yield return null;
            //    }

            //    _chain.Links[i].MeshRenderer.material.SetColor("_EmissionColor", Color.clear);
            //}
        }

        private IEnumerator FadeIn()
        {
            for (int i = 0; i < _chain.Links.Length; i++)
            {
                float lerp = 0;

                while (lerp <= 1)
                {
                    Color lerpColor = Color.Lerp(Color.clear, emissionColor, riseCurve.Evaluate(lerp));

                    _chain.Links[i].MeshRenderer.material.SetColor("_EmissionColor", lerpColor);

                    lerp += Time.deltaTime * riseSpeed;

                    yield return null;
                }
            }
        }

        private IEnumerator FadeOut()
        {
            for (int i = 0; i < _chain.Links.Length; i++)
            {
                float lerp = 0;

                while (lerp <= 1)
                {
                    Color lerpColor = Color.Lerp(emissionColor, Color.clear, fallCurve.Evaluate(lerp));

                    _chain.Links[i].MeshRenderer.material.SetColor("_EmissionColor", lerpColor);

                    lerp += Time.deltaTime * fallSpeed;

                    yield return null;
                }

                _chain.Links[i].MeshRenderer.material.SetColor("_EmissionColor", Color.clear);
            }
        }
    }
}
