using UnityEngine;

public class Lute : MonoBehaviour
{
    [SerializeField] private float _offset;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _shotPoint;    
    [SerializeField] private float _startTimeBtwShots;

    private float _timeBtwShots;

    void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + _offset);

        if (_timeBtwShots <= 0)
        {
            if (Input.GetMouseButton(0))
            {
                Instantiate(_bullet, _shotPoint.position, transform.rotation);
                _timeBtwShots = _startTimeBtwShots;
            }
        }
        else
        {
            _timeBtwShots -= Time.deltaTime;
        }
    }
}
