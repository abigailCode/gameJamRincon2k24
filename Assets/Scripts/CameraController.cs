using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // --------------------------------------------------------------------
    // C�DIGO QUE ASOCIADO A LA C�MARA PERMITE MIRAR POR DENTRO DEL SKYBOX
    // --------------------------------------------------------------------
    float rotationX = 0f; // Almacena la rotaci�n del mouse en X
    float rotationY = 0f; // Almacena la rotaci�n del mouse en Y
    [SerializeField] float sensitivity = 5f; // Sensibilidad del movimiento
    void Update()
    {
        // DETALLES SOBRE LAS ROTACIONES:
        // Cuando se rota sobre X, el eje que se FIJA es el HORIZONTAL
        // Cuando se rota sobre Y, el eje que se fija es el VERTICAL
        // Obtenemos la rotaci�n vertical (Y)
        // Cuando nos movemos en horizontal se fija el eje vertical
        rotationY += Input.GetAxis("Mouse X") * sensitivity;
        // Obtenemos la rotaci�n horizontal (X) (multiplicada por -1 porque est� invertida)
        // Cuando nos movemos en vertical se fija el eje horizontal
        rotationX += Input.GetAxis("Mouse Y") * sensitivity * (-1);
        // Aplicamos la rotaci�n a la c�mara bloqueando el giro completo en vertical a 90�
        transform.localEulerAngles = new Vector3(Mathf.Clamp(rotationX, -90, +90), rotationY, 0);
    }
}
