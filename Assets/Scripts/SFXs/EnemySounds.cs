using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySounds : MonoBehaviour {
    OpponentDetection _detection;
    bool shoot;
    Rigidbody _opponentRB;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (shoot == null)  {
            shoot = false;
        }
        else if (_detection.OpponentDamageable.TryGetComponent(out _opponentRB)) {
            shoot = true;
        }
    }
}
