using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // --------------------------------------------------------------------
    // CÓDIGO QUE ASOCIADO A LA CÁMARA PERMITE MIRAR POR DENTRO DEL SKYBOX
    // --------------------------------------------------------------------
    float rotationX = 0f; // Almacena la rotación del mouse en X
    float rotationY = 0f; // Almacena la rotación del mouse en Y
    [SerializeField] float sensitivity = 5f; // Sensibilidad del movimiento
    void Update()
    {
        // DETALLES SOBRE LAS ROTACIONES:
        // Cuando se rota sobre X, el eje que se FIJA es el HORIZONTAL
        // Cuando se rota sobre Y, el eje que se fija es el VERTICAL
        // Obtenemos la rotación vertical (Y)
        // Cuando nos movemos en horizontal se fija el eje vertical
        rotationY += Input.GetAxis("Mouse X") * sensitivity;
        // Obtenemos la rotación horizontal (X) (multiplicada por -1 porque está invertida)
        // Cuando nos movemos en vertical se fija el eje horizontal
        rotationX += Input.GetAxis("Mouse Y") * sensitivity * (-1);
        // Aplicamos la rotación a la cámara bloqueando el giro completo en vertical a 90º
        transform.localEulerAngles = new Vector3(Mathf.Clamp(rotationX, -90, +90), rotationY, 0);
    }
}
