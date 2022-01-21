using UnityEngine;

namespace Portals
{
    public class MainCamera : MonoBehaviour
    {
        private Portal[] portals;

        private void Awake()
        {
            GetPortals();
        }

        public void GetPortals() => portals = FindObjectsOfType<Portal>();

        private void LateUpdate()
        {
            for (int i = 0; i < portals.Length; i++)
                if (portals[i] && portals[i].enabled)
                    portals[i].PrePortalRender();

            for (int i = 0; i < portals.Length; i++)
                if (portals[i] && portals[i].enabled)
                    portals[i].Render();

            for (int i = 0; i < portals.Length; i++)
                if (portals[i] && portals[i].enabled)
                    portals[i].PostPortalRender();
        }
    }
}