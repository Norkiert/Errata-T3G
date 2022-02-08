using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Portals
{
    public class Portal : MonoBehaviour
    {
        [Header("Main Settings")]
        [SerializeField] private Portal linkedPortal;
        [SerializeField, Required] private MeshRenderer screen;
        [SerializeField, Min(0)] private int recursionLimit = 5;

        [Header("Advanced Settings")]
        [SerializeField] private float nearClipOffset = 0.05f;
        [SerializeField] private float nearClipLimit = 0.2f;

        // Private variables
        private RenderTexture viewTexture;
        private Camera portalCam;
        private Camera playerCam;
        private List<PortalTraveller> trackedTravellers;
        private MeshFilter screenMeshFilter;

        private void Awake()
        {
            playerCam = Camera.main;
            portalCam = GetComponentInChildren<Camera>();
            portalCam.enabled = false;
            trackedTravellers = new List<PortalTraveller>();
            screenMeshFilter = screen.GetComponent<MeshFilter>();
            screen.material.SetInt("displayMask", 1);

            if (linkedPortal == null)
                this.enabled = false;
        }

        private void LateUpdate()
        {
            HandleTravellers();
        }

        private void HandleTravellers()
        {
            for (int i = 0; i < trackedTravellers.Count; i++)
            {
                PortalTraveller traveller = trackedTravellers[i];
                Transform travellerT = traveller.transform;
                var m = linkedPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix * travellerT.localToWorldMatrix;

                Vector3 offsetFromPortal = travellerT.position - transform.position;
                int portalSide = System.Math.Sign(Vector3.Dot(offsetFromPortal, transform.forward));
                int portalSideOld = System.Math.Sign(Vector3.Dot(traveller.previousOffsetFromPortal, transform.forward));

                // Teleport the traveller if it has crossed from one side of the portal to the other
                if (portalSide != portalSideOld)
                {
                    var positionOld = travellerT.position;
                    var rotOld = travellerT.rotation;
                    traveller.Teleport(transform, linkedPortal.transform, m.GetColumn(3), m.rotation);
                    traveller.graphicsClone.transform.SetPositionAndRotation(positionOld, rotOld);

                    // Can't rely on OnTriggerEnter/Exit to be called next frame since it depends on when FixedUpdate runs
                    linkedPortal.OnTravellerEnterPortal(traveller);
                    trackedTravellers.RemoveAt(i);
                    i--;

                }
                else
                {
                    traveller.graphicsClone.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);

                    //UpdateSliceParams (traveller);
                    traveller.previousOffsetFromPortal = offsetFromPortal;
                }
            }
        }

        public void SetLinkedPortal(Portal portal)
        {
            linkedPortal = portal;
            this.enabled = linkedPortal != null;
        }

        public void SetPlayerCamera(Camera camera)
        {
            playerCam = camera;
            viewTexture = null;
        }

        public void SetCameraBackgroundOnSkyBox()
        {
            portalCam.clearFlags = CameraClearFlags.Skybox;
        }
        public void SetCameraBackgroundOnSolidColor(Color color)
        {
            portalCam.clearFlags = CameraClearFlags.SolidColor;
            portalCam.backgroundColor = color;
        }

        #region -render-

        /// <summary>
        /// Called before any portal cameras are rendered for the current frame
        /// </summary>
        public void PrePortalRender()
        {
            foreach (var traveller in trackedTravellers)
                UpdateSliceParams(traveller);
        }

        /// <summary>
        /// Manually render the camera attached to this portal
        /// Called after PrePortalRender, and before PostPortalRender
        /// </summary>
        public void Render()
        {
            // Skip rendering the view from this portal if player is not looking at the linked portal
            if (!CameraUtility.VisibleFromCamera(linkedPortal.screen, playerCam))
                return;

            CreateViewTexture();

            var localToWorldMatrix = playerCam.transform.localToWorldMatrix;
            var renderPositions = new Vector3[recursionLimit];
            var renderRotations = new Quaternion[recursionLimit];

            int startIndex = 0;
            portalCam.projectionMatrix = playerCam.projectionMatrix;
            for (int i = 0; i < recursionLimit; i++)
            {
                if (i > 0)
                {
                    // No need for recursive rendering if linked portal is not visible through this portal
                    if (!CameraUtility.BoundsOverlap(screenMeshFilter, linkedPortal.screenMeshFilter, portalCam))
                        break;
                }

                localToWorldMatrix = transform.localToWorldMatrix * linkedPortal.transform.worldToLocalMatrix * localToWorldMatrix;
                int renderOrderIndex = recursionLimit - i - 1;
                renderPositions[renderOrderIndex] = localToWorldMatrix.GetColumn(3);
                renderRotations[renderOrderIndex] = localToWorldMatrix.rotation;

                portalCam.transform.SetPositionAndRotation(renderPositions[renderOrderIndex], renderRotations[renderOrderIndex]);
                startIndex = renderOrderIndex;
            }

            // Hide screen so that camera can see through portal
            screen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            linkedPortal.screen.material.SetInt("displayMask", 0);

            for (int i = startIndex; i < recursionLimit; i++)
            {
                portalCam.transform.SetPositionAndRotation(renderPositions[i], renderRotations[i]);
                SetNearClipPlane();
                HandleClipping();
                portalCam.Render();

                if (i == startIndex)
                    linkedPortal.screen.material.SetInt("displayMask", 1);
            }

            // Unhide objects hidden at start of render
            screen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }

        private void HandleClipping()
        {
            // There are two main graphical issues when slicing travellers
            // 1. Tiny sliver of mesh drawn on backside of portal
            //    Ideally the oblique clip plane would sort this out, but even with 0 offset, tiny sliver still visible
            // 2. Tiny seam between the sliced mesh, and the rest of the model drawn onto the portal screen
            // This function tries to address these issues by modifying the slice parameters when rendering the view from the portal
            // Would be great if this could be fixed more elegantly, but this is the best I can figure out for now
            const float hideDst = -1000;
            const float showDst = 1000;
            float screenThickness = linkedPortal.ProtectScreenFromClipping(portalCam.transform.position);

            foreach (var traveller in trackedTravellers)
            {
                if (SameSideOfPortal(traveller.transform.position, PortalCamPos))
                {
                    // Addresses issue 1
                    traveller.SetSliceOffsetDst(hideDst, false);
                }
                else
                {
                    // Addresses issue 2
                    traveller.SetSliceOffsetDst(showDst, false);
                }

                // Ensure clone is properly sliced, in case it's visible through this portal:
                int cloneSideOfLinkedPortal = -SideOfPortal(traveller.transform.position);
                bool camSameSideAsClone = linkedPortal.SideOfPortal(PortalCamPos) == cloneSideOfLinkedPortal;

                if (camSameSideAsClone)
                    traveller.SetSliceOffsetDst(screenThickness, true);
                else
                    traveller.SetSliceOffsetDst(-screenThickness, true);
            }

            var offsetFromPortalToCam = PortalCamPos - transform.position;
            foreach (var linkedTraveller in linkedPortal.trackedTravellers)
            {
                var travellerPos = linkedTraveller.graphicsObject.transform.position;
                var clonePos = linkedTraveller.graphicsClone.transform.position;

                // Handle clone of linked portal coming through this portal:
                bool cloneOnSameSideAsCam = linkedPortal.SideOfPortal(travellerPos) != SideOfPortal(PortalCamPos);
                if (cloneOnSameSideAsCam)
                {
                    // Addresses issue 1
                    linkedTraveller.SetSliceOffsetDst(hideDst, true);
                }
                else
                {
                    // Addresses issue 2
                    linkedTraveller.SetSliceOffsetDst(showDst, true);
                }

                // Ensure traveller of linked portal is properly sliced, in case it's visible through this portal:
                bool camSameSideAsTraveller = linkedPortal.SameSideOfPortal(linkedTraveller.transform.position, PortalCamPos);
                if (camSameSideAsTraveller)
                    linkedTraveller.SetSliceOffsetDst(screenThickness, false);
                else
                    linkedTraveller.SetSliceOffsetDst(-screenThickness, false);
            }
        }

        /// <summary>
        /// Called once all portals have been rendered, but before the player camera renders
        /// </summary>
        public void PostPortalRender()
        {
            foreach (var traveller in trackedTravellers)
                UpdateSliceParams(traveller);

            ProtectScreenFromClipping(playerCam.transform.position);
        }
        private void CreateViewTexture()
        {
            if (viewTexture == null || viewTexture.width != Screen.width || viewTexture.height != Screen.height)
            {
                if (viewTexture != null)
                    viewTexture.Release();

                viewTexture = new RenderTexture(Screen.width, Screen.height, 0);

                // Render the view from the portal camera to the view texture
                portalCam.targetTexture = viewTexture;

                // Display the view texture on the screen of the linked portal
                linkedPortal.screen.material.SetTexture("_MainTex", viewTexture);
            }
        }

        /// <summary>
        /// Sets the thickness of the portal screen so as not to clip with camera near plane when player goes through
        /// </summary>
        private float ProtectScreenFromClipping(Vector3 viewPoint)
        {
            float halfHeight = playerCam.nearClipPlane * Mathf.Tan(playerCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float halfWidth = halfHeight * playerCam.aspect;
            float dstToNearClipPlaneCorner = new Vector3(halfWidth, halfHeight, playerCam.nearClipPlane).magnitude;
            float screenThickness = dstToNearClipPlaneCorner;

            Transform screenT = screen.transform;
            bool camFacingSameDirAsPortal = Vector3.Dot(transform.forward, transform.position - viewPoint) > 0;
            screenT.localScale = new Vector3(screenT.localScale.x, screenT.localScale.y, screenThickness);
            screenT.localPosition = Vector3.forward * screenThickness * ((camFacingSameDirAsPortal) ? 0.5f : -0.5f);
            return screenThickness;
        }

        private void UpdateSliceParams(PortalTraveller traveller)
        {
            // Calculate slice normal
            int side = SideOfPortal(traveller.transform.position);
            Vector3 sliceNormal = transform.forward * -side;
            Vector3 cloneSliceNormal = linkedPortal.transform.forward * side;

            // Calculate slice centre
            Vector3 slicePos = transform.position;
            Vector3 cloneSlicePos = linkedPortal.transform.position;

            // Adjust slice offset so that when player standing on other side of portal to the object, the slice doesn't clip through
            float sliceOffsetDst = 0;
            float cloneSliceOffsetDst = 0;
            float screenThickness = screen.transform.localScale.z;

            bool playerSameSideAsTraveller = SameSideOfPortal(playerCam.transform.position, traveller.transform.position);
            if (!playerSameSideAsTraveller)
                sliceOffsetDst = -screenThickness;

            bool playerSameSideAsCloneAppearing = side != linkedPortal.SideOfPortal(playerCam.transform.position);
            if (!playerSameSideAsCloneAppearing)
                cloneSliceOffsetDst = -screenThickness;

            // Apply parameters
            for (int i = 0; i < traveller.originalMaterials.Length; i++)
            {
                traveller.originalMaterials[i].SetVector("sliceCentre", slicePos);
                traveller.originalMaterials[i].SetVector("sliceNormal", sliceNormal);
                traveller.originalMaterials[i].SetFloat("sliceOffsetDst", sliceOffsetDst);

                traveller.cloneMaterials[i].SetVector("sliceCentre", cloneSlicePos);
                traveller.cloneMaterials[i].SetVector("sliceNormal", cloneSliceNormal);
                traveller.cloneMaterials[i].SetFloat("sliceOffsetDst", cloneSliceOffsetDst);

            }

        }

        /// <summary>
        /// Use custom projection matrix to align portal camera's near clip plane with the surface of the portal
        /// Note that this affects precision of the depth buffer, which can cause issues with effects like screenspace AO
        /// </summary>
        private void SetNearClipPlane()
        {
            // Learning resource:
            // http://www.terathon.com/lengyel/Lengyel-Oblique.pdf
            Transform clipPlane = transform;
            int dot = System.Math.Sign(Vector3.Dot(clipPlane.forward, transform.position - portalCam.transform.position));

            Vector3 camSpacePos = portalCam.worldToCameraMatrix.MultiplyPoint(clipPlane.position);
            Vector3 camSpaceNormal = portalCam.worldToCameraMatrix.MultiplyVector(clipPlane.forward) * dot;
            float camSpaceDst = -Vector3.Dot(camSpacePos, camSpaceNormal) + nearClipOffset;

            // Don't use oblique clip plane if very close to portal as it seems this can cause some visual artifacts
            if (Mathf.Abs(camSpaceDst) > nearClipLimit)
            {
                Vector4 clipPlaneCameraSpace = new Vector4(camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, camSpaceDst);

                // Update projection based on new clip plane
                // Calculate matrix with player cam so that player camera settings (fov, etc) are used
                portalCam.projectionMatrix = playerCam.CalculateObliqueMatrix(clipPlaneCameraSpace);
            }
            else
                portalCam.projectionMatrix = playerCam.projectionMatrix;
        }

        #endregion

        private void OnTravellerEnterPortal(PortalTraveller traveller)
        {
            if (!trackedTravellers.Contains(traveller))
            {
                traveller.EnterPortalThreshold();
                traveller.previousOffsetFromPortal = traveller.transform.position - transform.position;
                trackedTravellers.Add(traveller);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var traveller = other.GetComponent<PortalTraveller>();
            if (traveller)
                OnTravellerEnterPortal(traveller);
        }

        private void OnTriggerExit(Collider other)
        {
            var traveller = other.GetComponent<PortalTraveller>();
            if (traveller && trackedTravellers.Contains(traveller))
            {
                traveller.ExitPortalThreshold();
                trackedTravellers.Remove(traveller);
            }
        }


        private int SideOfPortal(Vector3 pos) => System.Math.Sign(Vector3.Dot(pos - transform.position, transform.forward));

        private bool SameSideOfPortal(Vector3 posA, Vector3 posB) => SideOfPortal(posA) == SideOfPortal(posB);

        private Vector3 PortalCamPos => portalCam.transform.position;

        private void OnValidate()
        {
            if (linkedPortal != null)
                linkedPortal.linkedPortal = this;
        }
    }
}