using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroler : MonoBehaviour
{
    [SerializeField] float backgroundScrollerSpeed;
    Material myMaterial;
    Vector2 offSet;
    // Start is called before the first frame update
    void Start()
    {
        myMaterial = GetComponent<Renderer>().material;
        offSet = new Vector2(0, backgroundScrollerSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        myMaterial.mainTextureOffset += offSet * Time.deltaTime;
    }
}
