using MyBox;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRController))]
public class ControllerBeat : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private XRController controller;
    [SerializeField, ReadOnly]
    private XRGrabInteractable interactable;
    [SerializeField, ReadOnly]
    private XRDirectInteractor interactor;

    [SerializeField]
    private int skipBeat = 0;
    private int beat = 0;
    [SerializeField]
    private float beatTime = 0.1f;
    private float beatTimer = 0;
    [SerializeField]
    private float beatImpuleAmplitude = 0.1f;
    [SerializeField]
    private float beatImpulseTime = 0.05f;

    public bool GotTheBeat { get => beatTimer > 0; }

    private void OnEnable()
    {
        Events.beat.AddListener(Beat);
        Events.preBeat.AddListener(PreBeat);
        interactor.onSelectEnter.AddListener(OnSelect);
        interactor.onSelectExit.AddListener(OnDeselect);
    }

    private void OnSelect(XRBaseInteractable _interactable)
    {
        if (_interactable.gameObject.TryGetComponent(out Fixer fixer))
        {
            fixer.controller = this;
            AudioPlayer.Instance.Play(SoundType.Reject, transform.position);
        }
    }

    private void OnDeselect(XRBaseInteractable _interactable)
    {
        if (_interactable.gameObject.TryGetComponent(out Fixer fixer))
        {
            fixer.controller = null;
            AudioPlayer.Instance.Play(SoundType.Reject, transform.position);
        }
    }

    private void OnDisable()
    {
        Events.beat.RemoveListener(Beat);
        Events.preBeat.RemoveListener(PreBeat);
        interactor.onSelectEnter.RemoveListener(OnSelect);
        interactor.onSelectExit.RemoveListener(OnDeselect);
    }


    private void Update()
    {
        if (beatTimer > 0)
            beatTimer -= Time.deltaTime;
    }

    private void Beat()
    {
        if (++beat > skipBeat)
            beat = 0;
        else
            return;

        controller.SendHapticImpulse(beatImpuleAmplitude, beatImpulseTime);
    }

    private void PreBeat(float buffer)
    {
        beatTimer = beatTime + buffer;
    }

    private void Reset()
    {
        controller = GetComponent<XRController>();
        interactor = GetComponent<XRDirectInteractor>();
    }
}
