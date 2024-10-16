using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class Exit : MonoBehaviour
    {
        public BossAct.BossUpdate bossUpdate;
        void Start()
        {
            bossUpdate = GetComponent<BossAct.BossUpdate>();
        }
        void Update()
        {

        }
        public void ExitPresentState()
        {

        }
    }
}

