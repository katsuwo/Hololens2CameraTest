using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {
    [SerializeField] private float initialXAngle;
    [SerializeField] private float initialYAngle;
    [SerializeField] private float initialZAngle;

    private float _currentXAngle;
    private float _currentYAngle;
    private float _currentZAngle;
    
    // Start is called before the first frame update
    void Start() {
        _currentXAngle = initialXAngle;
        _currentYAngle = initialYAngle;
        _currentZAngle = initialZAngle;
    }

    // Update is called once per frame
    void Update() {
        _currentXAngle = _currentXAngle += 1.0f;
        _currentYAngle = _currentYAngle += 1.0f;
        _currentZAngle = _currentZAngle += 1.0f;

        if (_currentXAngle >= 360.0f) _currentXAngle = 0f;
        if (_currentYAngle >= 360.0f) _currentYAngle = 0f;
        if (_currentZAngle >= 360.0f) _currentZAngle = 0f;

        var newRotation = new Vector3(_currentXAngle, _currentYAngle, _currentZAngle);
        gameObject.transform.eulerAngles = newRotation;
    }
}
