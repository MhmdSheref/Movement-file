using System;
using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _rollSpeed = 5;
    [NonSerialized]public int DiceResult = 1;
    private int rollsleft;
    public bool _IsMoving = false;
    [NonSerialized] public bool grounded;
    private float _gravSpeed = 10;
    public AudioSource audioSource;
    public bool onIce;
    private float _slideSpeed = 0.1f;


    // Start is called before the first frame update
    private void Awake()
    {
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        grounded = (Physics.Raycast(transform.position, Vector3.down, out hit, 1));
        if (!grounded && !_IsMoving)
        {
            transform.position += Time.deltaTime * _gravSpeed * Vector3.down;
        }

        if (grounded)
        {
            onIce = hit.transform.CompareTag("ice");
        }
        else
        {
            onIce = false;
        }
        
        
        if (_IsMoving) return;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) StartCoroutine(Roll(Vector3.forward));
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) StartCoroutine(Roll(Vector3.left));
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) StartCoroutine(Roll(Vector3.back));
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) StartCoroutine(Roll(Vector3.right));

        if (transform.up == Vector3.up) DiceResult = 1;
        if (transform.right == Vector3.up) DiceResult = 2;
        if (transform.forward == Vector3.up) DiceResult = 3;
        if (-transform.forward == Vector3.up) DiceResult = 4;
        if (-transform.right == Vector3.up) DiceResult = 5;
        if (-transform.up == Vector3.up) DiceResult = 6;
        
        

    }

    private void FixedUpdate()
    {
        
        

    }

    IEnumerator Roll(Vector3 dir)
    {
        for (int e = 0; e < DiceResult; e++)
        {
            grounded = (Physics.Raycast(transform.position, Vector3.down, 1f));
            
            if (!_IsMoving && grounded && !onIce)
            {
                var anchor = transform.position + (Vector3.down + dir) * 0.5f;
                var axis = Vector3.Cross(Vector3.up, dir);
            
                
                _IsMoving = true;
                
                
                for (int i = 0; i < (90/_rollSpeed); i++)
                {
                    transform.RotateAround(anchor,axis,_rollSpeed);
                    if (i == 90/_rollSpeed/2)
                    {
                        audioSource.Play();
                    }
                    yield return new WaitForSeconds(0.01f);
                }
                
                _IsMoving = false;
            }
            if (onIce && !_IsMoving)
            {
                StartCoroutine(slide(dir));
            }


        }
    }

    IEnumerator slide(Vector3 dir)
    {
        while (onIce)
        {
            _IsMoving = true;
            for (int i = 0; i < 1/_slideSpeed; i++)
            {   
                    transform.position += dir * _slideSpeed;
                    yield return new WaitForSeconds(0.01f);

            }

            _IsMoving = false;

        }
        
    }

    
}
