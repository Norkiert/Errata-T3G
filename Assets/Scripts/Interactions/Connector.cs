using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Logic;
using Audio;

[RequireComponent(typeof(Rigidbody))]
public class Connector : MonoBehaviour, ILogicBoolOutput
{
    public enum ConType { Male, Female }
    public enum CableColor { White, Red, Green, Yellow, Blue }

    [field: Header("Settings")]

    [field: SerializeField] public ConType ConnectionType { get; private set; } = ConType.Male;
    [field: SerializeField, OnValueChanged(nameof(UpdateConnectorColor))] public CableColor ConnectionColor { get; private set; } = CableColor.White;

    [SerializeField] private bool makeConnectionKinematic = false;
    private bool wasConnectionKinematic;

    [SerializeField] private bool hideInteractableWhenIsConnected = false;

    [field: SerializeField] public Connector ConnectedTo { get; private set; }


    [Header("Object to set")]
    [SerializeField, Required] private Transform connectionPoint;
    [SerializeField] private MeshRenderer collorRenderer;
    [SerializeField] private AudioClipSO connectSound;
    [SerializeField] private ParticleSystem sparksParticle;
    [SerializeField] private AudioClipSO sparksSound;


    private FixedJoint fixedJoint;
    public Rigidbody Rigidbody { get; private set; }

    public Vector3 ConnectionPosition => connectionPoint ? connectionPoint.position : transform.position;
    public Quaternion ConnectionRotation => connectionPoint ? connectionPoint.rotation : transform.rotation;
    public Quaternion RotationOffset => connectionPoint ? connectionPoint.localRotation : Quaternion.Euler(Vector3.zero);

    public bool IsConnected => ConnectedTo != null;
    public bool IsConnectedRight => IsConnected && ConnectionColor == ConnectedTo.ConnectionColor;
    public bool LogicValue => IsConnectedRight;



    private void Awake()
    {
        Rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        UpdateConnectorColor();

        if (ConnectedTo != null)
            Connect(ConnectedTo, false);
    }

    private void OnDisable() => Disconnect(false);

    public void SetAsConnectedTo(Connector secondConnector)
    {
        ConnectedTo = secondConnector;
        wasConnectionKinematic = secondConnector.Rigidbody.isKinematic;
    }

    public void Connect(Connector secondConnector, bool playSound)
    {
        if (secondConnector == null)
        {
            Debug.LogWarning("Attempt to connect null");
            return;
        }

        if (IsConnectedRight)
            Disconnect(true);

        secondConnector.transform.rotation = ConnectionRotation * secondConnector.RotationOffset;
        secondConnector.transform.position = ConnectionPosition - (secondConnector.ConnectionPosition - secondConnector.transform.position);

        fixedJoint = gameObject.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = secondConnector.Rigidbody;

        secondConnector.SetAsConnectedTo(this);
        wasConnectionKinematic = secondConnector.Rigidbody.isKinematic;
        if (makeConnectionKinematic)
            secondConnector.Rigidbody.isKinematic = true;
        ConnectedTo = secondConnector;

        // connectSound
        if (playSound && connectSound)
            AudioManager.PlaySFX(connectSound, transform.position);

        // sparks on inncretc connection
        if (incorrectSparksC == null && sparksParticle && IsConnected && !IsConnectedRight)
        {
            incorrectSparksC = IncorrectSparks();
            StartCoroutine(incorrectSparksC);
        }

        // disable outline on select
        if (hideInteractableWhenIsConnected)
        {
            if (TryGetComponent(out HighlightableOnSelect highlightableOnSelect))
                highlightableOnSelect.enabled = false;
            if (TryGetComponent(out Collider collider))
                collider.enabled = false;
        }
    }

    public void Disconnect(bool playSound, Connector onlyThis = null)
    {
        if (ConnectedTo == null || onlyThis != null && onlyThis != ConnectedTo)
            return;

        Destroy(fixedJoint);

        // important to dont make recusrion
        Connector toDisconect = ConnectedTo;
        ConnectedTo = null;
        if (makeConnectionKinematic)
            toDisconect.Rigidbody.isKinematic = wasConnectionKinematic;
        toDisconect.Disconnect(this);

        // connectSound
        if (playSound && connectSound)
            AudioManager.PlaySFX(connectSound, transform.position);

        // sparks on inncretc connection
        if (sparksParticle)
        {
            sparksParticle.Stop();
            sparksParticle.Clear();
        }

        // enable outline on select
        if (hideInteractableWhenIsConnected)
        {
            if (TryGetComponent(out HighlightableOnSelect highlightableOnSelect))
                highlightableOnSelect.enabled = true;
            if (TryGetComponent(out Collider collider))
                collider.enabled = true;
        }
    }


    private IEnumerator incorrectSparksC;
    private IEnumerator IncorrectSparks()
    {
        while (incorrectSparksC != null && sparksParticle && IsConnected && !IsConnectedRight)
        {
            sparksParticle.Play();
            if (sparksSound)
                AudioManager.PlaySFX(sparksSound, transform.position);

            yield return new WaitForSeconds(Random.Range(0.6f, 0.8f));
        }
        incorrectSparksC = null;
    }

    private void UpdateConnectorColor()
    {
        if (collorRenderer == null)
            return;

        Color color = MaterialColor(ConnectionColor);
        MaterialPropertyBlock probs = new MaterialPropertyBlock();
        collorRenderer.GetPropertyBlock(probs);
        probs.SetColor("_BaseColor", color);
        collorRenderer.SetPropertyBlock(probs);
    }

    private Color MaterialColor(CableColor cableColor) => cableColor switch
    {
        CableColor.White => Color.white,
        CableColor.Red => Color.red,
        CableColor.Green => Color.green,
        CableColor.Yellow => Color.yellow,
        CableColor.Blue => Color.blue,
        _ => Color.clear
    };
}
