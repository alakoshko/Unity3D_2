using UnityEngine;
using System.Collections;

public class SmoothStep : MonoBehaviour
{

    //Цель (пункт Б)
    public Transform target;

    //Стартовая позиция (ось Z)
    private Vector3 _startPos;
    //Конечная позиция (ось Z)
    private Vector3 _endPos;
    // Use this for initialization
    private float delay = 5f;

    void Start()
    {
        //Запоминаем начальную и конечную позиции
        _startPos = transform.position;
        _endPos = target.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position != target.position)
            _endPos = target.position;
        else
            _startPos = _endPos;
        //Новая позиция по оси Z
        //float _z = Mathf.SmoothStep(_startPos, _endPos, Time.time / 2);
        //Debug.Log(_z);
        //Устанавливаем новую позицию

        transform.position = new Vector3(Mathf.SmoothStep(_startPos.x, _endPos.x, Time.time / delay),
            Mathf.SmoothStep(_startPos.y, _endPos.y, Time.time / delay),
            Mathf.SmoothStep(_startPos.z, _endPos.z, Time.time / delay));
    }
}