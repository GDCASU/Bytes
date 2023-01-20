using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentationHandler : MonoBehaviour
{
    [SerializeField] StaticResource _battery;
    [SerializeField] float _batteryRegenDelay;
    [SerializeField] float _batteryRegenRate;
    [SerializeField] Transform _augmentationContainer;
    [SerializeField] Augmentation[] _augmentations = new Augmentation[3];
    bool _isBatteryDepleted;
    Coroutine _regenRoutine;
    WaitForSeconds _delayWait;
    WaitForSeconds _regenWait;

    public StaticResource Battery => _battery;

    void Awake()
    {
        Battery.Drained += CheckBattery;
        _delayWait = new WaitForSeconds(_batteryRegenDelay);
        _regenWait = new WaitForSeconds(1f / _batteryRegenRate);

        for (int i = 0; i < _augmentations.Length; i++)
        {
            Augmentation equippedAugmentation = _augmentations[i];
            if (equippedAugmentation)
                equippedAugmentation.Equip(gameObject);
        }
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
        for (int i = 0; i < _augmentations.Length; i++)
        {
            Augmentation equippedAugmentation = _augmentations[i];
            if (!equippedAugmentation)
            {
                _augmentations[i] = augmentation;
            }
            else if (equippedAugmentation.GetType() != augmentation.GetType())
            {
                equippedAugmentation.Unequip();
                equippedAugmentation.transform.SetParent(null);
                equippedAugmentation.transform.position = augmentation.transform.position;
                equippedAugmentation.transform.rotation = augmentation.transform.rotation;

                _augmentations[i] = augmentation;
            }
            else
                return;

            augmentation.transform.SetParent(_augmentationContainer.transform);
            augmentation.transform.localPosition = Vector3.zero;
            augmentation.transform.rotation = Quaternion.identity;
            augmentation.Equip(gameObject);
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
