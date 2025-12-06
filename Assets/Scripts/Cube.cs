using System;
using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private ColorChanger _colorChanger;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private float minTimeAfterCollision = 2f;
    [SerializeField] private float maxTimeAfterCollision = 5f;

    private Color _defaultColor;
    private bool _hasLanded = false;

    public event Action<Cube> ReadyToBePooled;

    private void Awake()
    {
        _defaultColor = _renderer.material.color;
    }

    public void Activate(Vector3 position)
    {
        _hasLanded = false;
        this.transform.position = position;
        SetColor(_defaultColor);
        _rigidBody.linearVelocity = Vector3.zero;
        _rigidBody.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
        gameObject.SetActive(true);
    }

    private IEnumerator MessageAfterDelay(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        ReadyToBePooled?.Invoke(this);
    }

    private void SetColor(Color newColor)
    {
        _renderer.material.color = newColor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasLanded == false && collision.gameObject.tag == "Obstacle")
        {
            _hasLanded = true;
            _colorChanger.ChangeColorToRandom(_renderer.material);
            StartCoroutine(MessageAfterDelay(UnityEngine.Random.Range(minTimeAfterCollision, maxTimeAfterCollision)));
        }
    }
}
