using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INab.Dissolve
{
    [RequireComponent(typeof(Dissolver))]
    public class DecalDissolve : MonoBehaviour
    {
        public Dissolver dissolver;

        private void OnEnable()
        {
            if (dissolver == null) dissolver = GetComponent<Dissolver>();
            dissolver.Dissolve();
        }
    }
}