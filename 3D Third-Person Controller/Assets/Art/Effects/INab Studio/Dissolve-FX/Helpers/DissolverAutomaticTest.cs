using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INab.Dissolve
{
    [RequireComponent(typeof(Dissolver))]
    public class DissolverAutomaticTest : MonoBehaviour
    {
        public Dissolver dissolver;
        public Dissolver.DissolveState lastState;
        public float startDelay = 2f; // Adjust the delay time as desired
        public bool onEnableStart = false;
        public bool onlyDissolve = false;
        public bool onlyDissolveMaterialize = false;

        private void Start()
        {
            dissolver = GetComponent<Dissolver>();
            lastState = dissolver.initialState;
            if(!onEnableStart)StartCoroutine(StartDelayedUpdate());
        }

        private void OnEnable()
        {
            if(onEnableStart)StartCoroutine(StartDelayedUpdate());
        }

        private IEnumerator StartDelayedUpdate()
        {
            yield return new WaitForSeconds(startDelay);

            if (onlyDissolve)
            {
                dissolver.Dissolve();

                yield return null; // Wait for the next frame
            }
            else if (onlyDissolveMaterialize)
            {
                dissolver.Dissolve();
                yield return new WaitForSeconds(dissolver.duration);
                dissolver.Materialize();

            }
            else
            {

                while (true)
                {


                    if (dissolver.currentState == lastState)
                    {
                        if (dissolver.currentState == Dissolver.DissolveState.Dissolved)
                        {
                            dissolver.Materialize();

                            lastState = Dissolver.DissolveState.Materialized;
                        }
                        else
                        {
                            dissolver.Dissolve();
                            lastState = Dissolver.DissolveState.Dissolved;
                        }
                    }

                    yield return null; // Wait for the next frame
                }
            }

        }
    }
}