using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Parameters : MonoBehaviour
{
    public int width;
    public int length;
    public string obsPath;
    public string tempPath;
    //public string phePath;
    public int[,] tempEntries;
    public bool run = false;
    public int beeCount;
    public float beeWidth;
    public float beeLength;
    public float beeSpeed;
    public float beeTurn;
    public float senseRange;
    public int minX;
    public int maxX;
    public int minY;
    public int maxY;
    public int digitCount;
    public float simTime;
    public int simCount;
    public string fileDir;
    public Text timerText;
    string path;
    //public string fileName;
    int toggle = 0;
    int iteration = 0;
    float startTime;
    float interval = 1;
    int totalInstances;
    int instance = 0;
    GameObject[] monas;

    // Start is called before the first frame update
    void Start()
    {
        if(tempPath != "" && tempPath != null)
        {
            PopulateTemperatureArray();
        }
        path = fileDir + "/" + "BeeClust_" + System.DateTime.Now.ToString("dd-MM-yy_Thhmmss") + ".txt";
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Run\tTime\tName\tX pos\tY pos\n");
        }
        monas = GameObject.FindGameObjectsWithTag("MONA");
        run = true;
        totalInstances = (int)(simTime / interval);
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine();
        writer.Close();
        InvokeRepeating("LogPosition", 0.0f, 1.0f);
       

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Renderer floorRenderer = GameObject.Find("Floor").GetComponent<Renderer>();
            if (toggle > 0)
            {
                toggle = 0;
            }
            else
            {
                toggle++;
            }
            if(toggle == 0)
            {
                floorRenderer.material.mainTexture = null;
                floorRenderer.material.SetColor("_Color", Color.blue);
            }
            if (toggle == 1)
            {
                Texture tempTex = Resources.Load<Texture>("Textures/tempTexture");
                floorRenderer.material.mainTexture = tempTex;
                floorRenderer.material.SetColor("_Color", Color.white);
            }
        }
        if (run)
        {
           
            timerText.text = "Run: " + (iteration + 1) + "\t\t" + "Elapsed Time: " + (Time.time - startTime).ToString("F2")+ "\r\n" + Time.timeScale + "x Speed" + "\r\n";

        }
    }

    void PopulateTemperatureArray()
    {
        StreamReader reader = new StreamReader(tempPath);
        string fullData = reader.ReadToEnd();
        int i = 0, j = 0;
        tempEntries = new int[width * 10, length * 10];
        foreach (var row in fullData.Split('\n'))
        {
            j = 0;
            foreach (var col in row.Split(','))
            {
                if (int.TryParse(col, out int result))
                {
                    tempEntries[i, j] = int.Parse(col);
                    j++;
                }
            }
            i++;
        }

        reader.Close();
    }

    void LogPosition()
    {
        StreamWriter writer = new StreamWriter(path, true);
        float time = Time.time - startTime;
        foreach (GameObject mona in monas)
        {
            writer.WriteLine(iteration + "\t" + time.ToString("F2") + "\t" + mona.name + "\t" + mona.transform.localPosition.x.ToString("F2") + "\t" + mona.transform.localPosition.z.ToString("F2"));
        }
        writer.Close();
        instance++;
        if(instance > totalInstances)
        {
            CancelInvoke();
            if (iteration < simCount - 1)
            {
                Debug.Log("Reset");
                foreach (GameObject mona in monas)
                {
                    while (true)
                    {
                        int x = Random.Range(minX, maxX);
                        int y = Random.Range(minY, maxY);
                        Collider[] occupied = Physics.OverlapBox(new Vector3(x + 0.5f, 0.5f, y + 0.5f), new Vector3(0.45f, 0.45f, 0.45f), Quaternion.identity);
                        if (occupied.Length == 0)
                        {
                            mona.transform.position = new Vector3(x + 0.5f, 0.15f, y + 0.5f);
                            mona.transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                            break;
                        }
                    }
                }
                iteration++;
                instance = 0;
                startTime = Time.time;
                InvokeRepeating("LogPosition", 0.0f, 1.0f);
            }
            else
            {
                Debug.Log("End");
                run = false;
            }
        }
    }
}
