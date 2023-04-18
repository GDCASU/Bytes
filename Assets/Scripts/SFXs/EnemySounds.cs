using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySounds : MonoBehaviour {
    [SerializeField] StaticResource _health;
    int lastHealth;
    bool deathDone = false;
    bool damageDone = false;
    int damage;
    void Awake() => GetComponent<Damageable>().DamageReceived += Damage;

    void Damage(int _damage)
    {
        damage = _damage;
        damageDone = false;
    }


    // Start is called before the first frame update
    void Start() {
        lastHealth = _health.Current;
    }

    // Update is called once per frame
    void Update() {

        if (_health.Current == 0 && ! deathDone) //death noise
        {
            SoundManager.PlaySound(SoundManager.Sound.BalliTurrDeath);
            deathDone = true;
        }

        else if (damage > 0 && ! damageDone) //took damage 
        {
            SoundManager.PlaySound(SoundManager.Sound.BalliTurrDamage);
            damageDone = true;
        }
    }
}
