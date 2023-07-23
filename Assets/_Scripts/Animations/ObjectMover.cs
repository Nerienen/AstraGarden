using ProjectUtils.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ProjectUtils.Helpers.Transitions;

public class ObjectMover : MonoBehaviour
{
    public event Action OnAnimationFinished;

    public static ObjectMover Instance { get; private set; }

    bool _isMovingObject;
    Transform _transformToMove;
    Vector3 _targetPosition;
    Vector3 _initialPosition;
    float _movingDuration;
    AnimationCurve _movingCurve;
    float _movingPercentage;
    float _movingPercentageEvaluated;
    bool _hasFinishedMoving;

    bool _isRotatingObject;
    Transform _transformToRotate;
    Quaternion _targetRotation;
    Quaternion _initialRotation;
    float _rotationDuration;
    AnimationCurve _rotationCurve;
    float _rotationPercentage;
    float _rotationPercentageEvaluated;
    bool _hasFinishedRotating;

    private void Awake()
    {
        #region Singleton declaration
        if (Instance != null)
        {
            Debug.LogWarning($"Another instance of {nameof(ObjectMover)} exists. This one has been eliminated");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        #endregion
    }

    private void Update()
    {
        if (_isMovingObject) DoMoveToTargetPosition();
        if (_isRotatingObject) DoRotateToTargetRotation();

        if (_isMovingObject && _isRotatingObject && _hasFinishedMoving && _hasFinishedRotating)
        {
            EndAnimation();
        }
    }

    void DoMoveToTargetPosition()
    {
        _movingPercentage += (1f / _movingDuration) * Time.deltaTime;
        _movingPercentage = Mathf.Max(_movingPercentage, 1f);

        _movingPercentageEvaluated = _movingCurve.Evaluate(_movingPercentage);

        _transformToMove.position = Vector3.Lerp(_initialPosition, _targetPosition, _movingPercentageEvaluated);

        if (_movingPercentageEvaluated >= 1)
        {
            _hasFinishedMoving = true;
        }
    }

    void DoRotateToTargetRotation()
    {
        _rotationPercentage += (1f / _rotationDuration) * Time.deltaTime;
        _rotationPercentage = Mathf.Max(_rotationPercentage, 1f);

        _rotationPercentageEvaluated = _rotationCurve.Evaluate(_rotationPercentage);

        _transformToRotate.rotation = Quaternion.Slerp(_initialRotation, _targetRotation, _rotationPercentageEvaluated);

        if (_rotationPercentageEvaluated >= 1)
        {
            _hasFinishedRotating = true;
        }
    }

    private void EndAnimation()
    {
        _isMovingObject = false;
        _isRotatingObject = false;
        _hasFinishedMoving = false;
        _hasFinishedRotating = false;

        OnAnimationFinished?.Invoke();
    }

    public void MoveToTargetPosition(Transform transformToMove, Vector3 targetPosition, float duration, AnimationCurve curve)
    {
        _isMovingObject = true;
        _transformToMove = transformToMove;
        _targetPosition = targetPosition;
        _movingDuration = duration;
        _movingCurve = curve;
        _initialPosition = _transformToMove.position;
    }

    public void RotateToTargetRotation(Transform transformToRotate, Quaternion destinationRotation, float duration, AnimationCurve curve)
    {
        _isRotatingObject = true;
        _transformToRotate = transformToRotate;
        _targetRotation = destinationRotation;
        _rotationDuration = duration;
        _rotationCurve = curve;
        _initialRotation = transformToRotate.rotation;
    }
}
