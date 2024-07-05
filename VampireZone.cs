using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VampireZone : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private int _stealingHealth;
    [SerializeField] private float _radius;
    [SerializeField] private Color _changingColor;
    [SerializeField] private float _activatedTime;
    
    private Color _defaultColor;
    private Coroutine _coroutine;
    private Image _image;
    private Enemy _enemy;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _defaultColor = _image.color;
    }

    private void Update()
    {
        KeyCode key = KeyCode.RightControl;

        if (Input.GetKeyDown(key))
        {
            _image.color = _changingColor;

            HandleColliders();
        }
    }

    private IEnumerator StealHealth()
    {
        WaitForSeconds waitTime = new WaitForSeconds(_activatedTime / _stealingHealth);

        for (int i = 0; i < _stealingHealth; i++)
        {
            yield return waitTime;

            _enemy.TakeDamage(1);
            _player.Heal(1);
        }

        _image.color = _defaultColor;
        _coroutine = null;
        _enemy = null;
    }

    private void HandleColliders()
    {
        if (_coroutine == null)
        {
            _enemy = GetEnemy();

            if (_enemy != null)
            {
                _coroutine = StartCoroutine(StealHealth());
            }
        }
    }

    private Enemy GetEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _radius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.TryGetComponent(out Enemy enemy))
            {
                return enemy;
            }
        }

        return null;
    }
}