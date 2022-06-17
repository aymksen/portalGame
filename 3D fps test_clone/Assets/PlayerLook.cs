using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerLook : MonoBehaviour
{
    [Header("References")]
   
    [SerializeField] private float sensX = 100f;
    [SerializeField] private float sensY = 100f;

    [SerializeField] Transform cam = null;
    [SerializeField] Transform orientation = null;

    [SerializeField] public Joystick joystick2;
   

    float mouseX;
    float mouseY;

    float multiplier = 0.01f;

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()


    {
        /*if(Touchscreen.current.touches.count>0 && Touchscreen.current.touches[0].isInProgress)
        {
            mouseX = Touchscreen.current.touches[0].delta.ReadValue().x;
            mouseY = Touchscreen.current.touches[0].delta.ReadValue().y;
        }
        

        if (EvenSystem.current.IsPointerOverGameObject(Input.GetAxisRaw("Mouse X")) || EvenSystem.current.IsPointerOverGameObject(Input.GetAxisRaw("Mouse Y")))
            return;
        
        mouseX = TouchField.touchDist.x;
        mouseY = TouchField.touchDist.y;
        */

        // mouseY = Input.touches[0].deltaPosition.y;

        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        //mouseX = joystick2.Horizontal ;
        //mouseY = joystick2.Vertical ;

        yRotation += mouseX * sensX * multiplier;
            xRotation -= mouseY * sensY * multiplier;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        
    }
}
