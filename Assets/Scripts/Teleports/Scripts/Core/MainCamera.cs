using UnityEngine;

namespace Portals
{
    public class MainCamera : MonoBehaviour
    {
        [SerializeField] private Portal[] portals;

        private void Awake()
        {
            UpdatePortals();
        }

        public void UpdatePortals()
        {
            portals = FindObjectsOfType<Portal>();
            Camera camera = GetComponent<Camera>();
            foreach (Portal portal in portals)
                portal.SetPlayerCamera(camera);
        }

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