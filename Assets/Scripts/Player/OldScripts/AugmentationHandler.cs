using System.Collections;
using UnityEngine;

public class AugmentationHandler : MonoBehaviour
{
    public StaticResource Battery => _battery;
    public ManeuverQueue MQueue { get; private set; }

    [SerializeField] StaticResource _battery;
    [SerializeField] float _batteryRegenDelay;
    [SerializeField] float _batteryRegenRate;
    [SerializeField] Transform _augmentationContainer;
    [SerializeField] Augmentation[] _augmentations = new Augmentation[3];
    bool _isBatteryDepleted;
    Coroutine _regenRoutine;
    WaitForSeconds _delayWait;
    WaitForSeconds _regenWait;
    bool _isSceneLoading = true;
    
    void Awake()
    {
        Battery.Drained += CheckBattery;
        MQueue = GetComponent<ManeuverQueue>();
        _delayWait = new WaitForSeconds(_batteryRegenDelay);
        _regenWait = new WaitForSeconds(1f / _batteryRegenRate);
    }

    void Start()
    {
        for (int i = 0; i < _augmentations.Length; i++)
        {
            Augmentation equippedAugmentation = _augmentations[i];
            if (equippedAugmentation)
            {
                equippedAugmentation.Interact(gameObject);
            }
        }
        _isSceneLoading = false;
    }

    void OnValidate()
    {
        _delayWait = new WaitForSeconds(_batteryRegenDelay);
        _regenWait = new WaitForSeconds(1f / _batteryRegenRate);
    }

    public void TriggerAugmentation(int index, bool inputPressed)
    {
        if (_isBatteryDepleted)
            return;

        _augmentations[index]?.Trigger(inputPressed);
    }

    public void TakeAugmentation(Augmentation augmentation)
    {
        if (_isSceneLoading)
        {
            for (int i = 0; i < _augmentations.Length; i++)
            {
                augmentation.transform.SetParent(_augmentationContainer.transform);
                augmentation.transform.localPosition = Vector3.zero;
                augmentation.transform.localRotation = Quaternion.identity;
                augmentation.Equip(gameObject);
            }
            return;
        }

        // TODO: Program a UI to select which augment to replace when full.
        for (int i = 0; i < _augmentations.Length; i++)
        {
            Augmentation equippedAugmentation = _augmentations[i];
            if (!equippedAugmentation)
            {
                _augmentations[i] = augmentation;
            }
            else if (equippedAugmentation.GetType() == augmentation.GetType())
            {
                break;
            }
            else if (i == _augmentations.Length - 1)
            {
                equippedAugmentation.transform.SetParent(null);
                equippedAugmentation.transform.position = augmentation.transform.position;
                equippedAugmentation.transform.rotation = augmentation.transform.rotation;
                equippedAugmentation.Unequip();

                _augmentations[i] = augmentation;
            }
            else
                continue;

            augmentation.transform.SetParent(_augmentationContainer.transform);
            augmentation.transform.localPosition = Vector3.zero;
            augmentation.transform.localRotation = Quaternion.identity;
            augmentation.Equip(gameObject);
            break;
        }
    }

    void CheckBattery(int batteryAmount)
    {
        if (batteryAmount == _battery.Max)
            return;

        if (batteryAmount == 0)
            _isBatteryDepleted = true;

        if (_regenRoutine != null)
            StopCoroutine(_regenRoutine);

        _regenRoutine = StartCoroutine(RegenerateBattery());
    }

    IEnumerator RegenerateBattery()
    {
        yield return _delayWait;

        while (_battery.Current < _battery.Max)
        {
            yield return _regenWait;
            _battery.Fill(1);
        }

        _isBatteryDepleted = false;
    }
}
