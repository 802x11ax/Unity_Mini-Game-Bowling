using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow)){
            transform.Rotate(Vector3.down, Time.deltaTime * 30f);
        }

        if (Input.GetKey(KeyCode.RightArrow)){
            transform.Rotate(Vector3.up, Time.deltaTime * 30f);
        }

        if (Input.GetKey(KeyCode.UpArrow)){
            if (transform.localScale.z < 2){
                // Increases the scale along the z-axis by a certain amount over time.
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y,
                    transform.localScale.z + (1 * Time.deltaTime));
            }
            else{
                //  If the current scale is 2 or greater, sets the scale exactly to 2 along the z-axis.
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 2);
            }
        }

        if (Input.GetKey(KeyCode.DownArrow)){
            if (transform.localScale.z > 0.1f){
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y,
                    transform.localScale.z - (1 * Time.deltaTime));
            }
            else{
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 0.1f);
            }

        }
    }
}
