using UnityEngine;

public class RotSign : MonoBehaviour
{
    [SerializeField]
    private Transform plane;

    // Update is called once per frame
    void Update()
    {
        //Если нажата клавиша "влево" или "вправо" (чтобы исключить ноль)
        if (Input.GetButton("Horizontal"))
        {
            //Позиция по оси X в диапазоне от -500 до 500
            float _x = Mathf.Clamp(transform.position.x + Input.GetAxis("Horizontal") * 10 * Time.deltaTime, -7, 7);
            //Позиция по оси Z в диапазоне от -500 до 500
            float _z = Mathf.Clamp(transform.position.z + Input.GetAxis("Vertical") * 10 * Time.deltaTime, -7, 7);
            //Установка новой позиции
            transform.position = new Vector3(_x, transform.position.y, _z);
        }
    }

  
}