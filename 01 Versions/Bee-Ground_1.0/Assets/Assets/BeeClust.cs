using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;


public class BeeClust : MonoBehaviour
{

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
    public float w, dis;
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

    public string path_Collision, path_State, path_Parameters;
    public string fileDir;
    public int collision;

    // Use this for initialization
    void Start()
    {
        master = GameObject.Find("Master").GetComponent<Parameters>();

        fileDir = master.fileDir;

        path_Collision = fileDir + "/" + System.DateTime.Now.ToString("dd-MM-yy_hhmmss") + "_Collision" + ".txt";
        if (!File.Exists(path_Collision))
        {
            File.WriteAllText(path_Collision, "Run\tTime\tName\tX pos\tY pos\tCollision\tOnCue\n");
        }

        path_State = fileDir + "/" + System.DateTime.Now.ToString("dd-MM-yy_hhmmss") + "_State" + ".txt";
        if (!File.Exists(path_State))
        {
            File.WriteAllText(path_State, "Run\tTime\tName\tX pos\tY pos\tState\tDelay\tOnCue\n");
        }

        path_Parameters = fileDir + "/" + System.DateTime.Now.ToString("dd-MM-yy_hhmmss") + "_Parameters" + ".txt";
        if (!File.Exists(path_Parameters))
        {
            File.WriteAllText(path_Parameters, "Simulation Log:\n");
            StreamWriter writer = new StreamWriter(path_Parameters, true);
            writer.WriteLine("Date and Time: " + System.DateTime.Now.ToString("dd-MM-yy hh:mm:ss") + "\n");

            writer.WriteLine("Simulation:\n" + "Time: " + master.simTime);
            writer.WriteLine("Repeat count: " + master.simCount + "\n");
  
            writer.WriteLine("Arena:\n" + "Length: " + master.length);
            writer.WriteLine("Width: " + master.width+"\n");

            writer.WriteLine("Agents:\n" + "Counts: " + master.beeCount);
            writer.WriteLine("Length: " + master.beeLength);
            writer.WriteLine("Width: " + master.beeWidth);
            writer.WriteLine("Speed: " + master.beeSpeed);
            writer.WriteLine("Turning Speed: " + master.beeTurn);
            writer.WriteLine("Sensor Range: " + master.senseRange + "\n");

            writer.WriteLine("Initialization Postion:\n" + "X: from " + master.minX + " to " + master.maxX);
            writer.WriteLine("Y: from " + master.minY + " to " + master.maxY);

            writer.Close();
        }

    }

    // Update is called once per frame
    void Update()
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
        if (nextPosition.x > (0 + master.beeWidth / 2) && nextPosition.x < (master.length - master.beeWidth / 2) && nextPosition.z > (0 + master.beeLength / 2) && nextPosition.z < (master.width - master.beeLength / 2))
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
        if (master.tempPath != "")
        {

            int xTemp = (int)Mathf.Round(transform.position.x * 10.0f);
            int yTemp = (int)Mathf.Round(transform.position.z * 10.0f);

            s = master.tempEntries[master.width * 10 - 1 - yTemp, xTemp];

            w = wMax * (Mathf.Pow(s, 2.0f) / (Mathf.Pow(s, 2.0f) + theta));
        }
        else
        {
            w = 0;
        }
        //Collision
        collision++;
        bool oncue = false;
        if (s != 0)
        {
            oncue = true;
        }
        else
        {
            oncue = false;
        }

        float time = Time.time - master.startTime;
        
        StreamWriter writer = new StreamWriter(path_Collision, true);

        writer.WriteLine(master.iteration + "\t" + time.ToString("F2") + "\t" + gameObject.name + "\t" + transform.localPosition.x.ToString("F2") + "\t" + transform.localPosition.z.ToString("F2") + "\t" + collision + "\t" + oncue);
        writer.Close();
        StreamWriter writer_state = new StreamWriter(path_State, true);

        writer_state.WriteLine(master.iteration + "\t" + time.ToString("F2") + "\t" + gameObject.name + "\t" + transform.localPosition.x.ToString("F2") + "\t" + transform.localPosition.z.ToString("F2") + "\t" + "0" + "\t" + w + "\t" + oncue);
        writer_state.Close();

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


