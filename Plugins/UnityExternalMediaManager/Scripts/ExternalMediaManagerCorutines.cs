using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityExternalMediaManager
{
    public class ExternalMediaManagerCorutines : MonoBehaviour
    {
        /**
         * Add this script to scene to init GO.
         */

        public static GameObject GO;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void Awake()
        {
            if (gameObject != GO)
            {
                GO = gameObject;
            }
        }
    }
}
