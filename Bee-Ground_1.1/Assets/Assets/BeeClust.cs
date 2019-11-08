using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeClust : MonoBehaviour {

    public float maxSensingRange;
    public float forwardSpeed;
    public float turnSpeed;
    public float turnAngle1;
    public float turnAngle2;
    public float turnAngle3;
    public float sensor2angle;
    public float sensor3angle;
    public float wMax = 60.0f;
    public float theta = 1000.0f;
    public int s;
    public float w,dis;
    public bool waiting = false;
    public bool frontSensingOnly = false;

    private RaycastHit frontSens;
    private RaycastHit leftSens;
    private RaycastHit rightSens;
    private Vector3 irLeft1;
    private Vector3 irLeft2;
    private Vector3 irRight1;
    private Vector3 irRight2;
    private float turnAngle;
    private bool takeOver = false;
    private bool turning = false;
    private Quaternion startBearing;
    private Vector3 initPos;
    private Vector3 nextPosition;
    Parameters master;

    // Use this for initialization
    void Start () {
        master = GameObject.Find("Master").GetComponent<Parameters>();

        
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Time.timeScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Time.timeScale = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Time.timeScale = 10;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Time.timeScale = 30;
        }

        if (master.run)
        {
            RunSensors();
            if (!takeOver)
            {
                if (turning)
                {
                    Turn();
                }
                else
                {
                    MoveForward();
                    if (Physics.Raycast(transform.position, transform.forward, out frontSens, maxSensingRange))
                    {
                        if (!turning)
                        {
                            startBearing = transform.rotation;
                            bool boolValue = (Random.Range(0, 2) == 0);
                            if (boolValue)
                            {
                                turnAngle = turnAngle1;
                            }
                            else
                            {
                                turnAngle = -turnAngle1;
                            }
                            if (frontSens.transform.gameObject.CompareTag("MONA"))
                            {
                                WaitFunction();
                            }
                            else
                            {
                                turning = true;
                            }
                        }
                    }
                    if (Physics.Raycast(transform.position, irLeft1, out frontSens, maxSensingRange))
                    {
                        if (!turning)
                        {
                            startBearing = transform.rotation;
                            turnAngle = turnAngle2;
                            if (frontSens.transform.gameObject.CompareTag("MONA") && !frontSensingOnly)
                            {
                                WaitFunction();
                            }
                            else
                            {
                                turning = true;
                            }
                        }
                    }
                    if (Physics.Raycast(transform.position, irLeft2, out frontSens, maxSensingRange))
                    {
                        if (!turning)
                        {
                            startBearing = transform.rotation;
                            turnAngle = turnAngle3;
                            if (frontSens.transform.gameObject.CompareTag("MONA") && !frontSensingOnly)
                            {
                                WaitFunction();
                            }
                            else
                            {
                                turning = true;
                            }
                        }
                    }
                    if (Physics.Raycast(transform.position, irRight1, out frontSens, maxSensingRange))
                    {
                        if (!turning)
                        {
                            startBearing = transform.rotation;
                            turnAngle = -turnAngle2;
                            if (frontSens.transform.gameObject.CompareTag("MONA") && !frontSensingOnly)
                            {
                                WaitFunction();
                            }
                            else
                            {
                                turning = true;
                            }
                        }
                    }
                    if (Physics.Raycast(transform.position, irRight2, out frontSens, maxSensingRange))
                    {
                        if (!turning)
                        {
                            startBearing = transform.rotation;
                            turnAngle = -turnAngle3;
                            if (frontSens.transform.gameObject.CompareTag("MONA") && !frontSensingOnly)
                            {
                                WaitFunction();
                            }
                            else
                            {
                                turning = true;
                            }
                        }
                    }
                }
            }
        }       
    }


    void RunSensors()
    {
        irLeft1 = Quaternion.AngleAxis(-sensor2angle, Vector3.up) * transform.forward;
        irLeft2 = Quaternion.AngleAxis(-sensor3angle, Vector3.up) * transform.forward;
        irRight1 = Quaternion.AngleAxis(sensor2angle, Vector3.up) * transform.forward;
        irRight2 = Quaternion.AngleAxis(sensor3angle, Vector3.up) * transform.forward;

        Debug.DrawRay(transform.position, transform.forward * maxSensingRange);
        Debug.DrawRay(transform.position, irLeft1 * maxSensingRange);
        Debug.DrawRay(transform.position, irLeft2 * maxSensingRange);
        Debug.DrawRay(transform.position, irRight1 * maxSensingRange);
        Debug.DrawRay(transform.position, irRight2 * maxSensingRange);
    }

    void Turn()
    {
        if (turnAngle < 0)
        {
            gameObject.transform.RotateAround(transform.position, Vector3.up, -turnSpeed * Time.deltaTime);
        }
        else
        {
            gameObject.transform.RotateAround(transform.position, Vector3.up, turnSpeed * Time.deltaTime);
        }
        if (Mathf.Abs(Quaternion.Angle(startBearing, transform.rotation)) >= Mathf.Abs(turnAngle))
        {
            turning = false;
        }
    }

    void MoveForward()
    {

        nextPosition = transform.position + transform.forward * forwardSpeed * Time.deltaTime;
        if (nextPosition.x > (0 + master.beeWidth /2) && nextPosition.x < (master.width - master.beeWidth / 2) && nextPosition.z > (0 + master.beeLength / 2) && nextPosition.z < master.length - master.beeLength / 2)
        {
            transform.position += transform.forward * forwardSpeed * Time.deltaTime;

        }
        else
        {
            turning = true;

        }
    }

    void WaitFunction()
    {
        if(master.tempPath != "")
        {
            
            int xTemp = (int)Mathf.Round(transform.position.x * 10.0f);
            int yTemp = (int)Mathf.Round(transform.position.z * 10.0f);

            s = master.tempEntries[master.width * 10 - 1 - yTemp, xTemp];
            //s = master.tempEntries[yTemp, xTemp];

            // dis = Mathf.Pow((xTemp - 125), 2.0f) + Mathf.Pow((yTemp - 125), 2.0f);
            //if(dis > 3600)
            //{
            //    Debug.Log("OUT");
            //    s = 0;
            //}
            //else
            //{
            //    Debug.Log("IN");
            //    Debug.Log("Xtemp=" + xTemp);
            //    Debug.Log("Ytemp=" + yTemp);
            //    dis = Mathf.Pow(dis, 0.5f);
            //    s = (int)Mathf.Round((1 - (dis / 60)) * 250);
            //}
            ////s = 255;
            w = wMax * (Mathf.Pow(s, 2.0f) / (Mathf.Pow(s, 2.0f) + theta));
        }
        else
        {
            w = 0;
        }
        StartCoroutine(Delay(w));
    }


    IEnumerator Delay(float w)
    {
        takeOver = true;
        yield return new WaitForSeconds(w);
        waiting = false;
        takeOver = false;
        turning = true;
    }
}


