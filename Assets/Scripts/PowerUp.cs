using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
   public float moveSpeed = 0f;
    [SerializeField] public float param_value = 0f;
    [SerializeField] public string power_up_type ="DOUBLE_FIRE";
    // Start is called before the first frame update
   
    private void Move() {
        var deltaY =  Time.deltaTime * -moveSpeed;
        transform.position = new Vector2(transform.position.x, transform.position.y + deltaY);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
