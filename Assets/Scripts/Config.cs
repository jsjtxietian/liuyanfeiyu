using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class Config : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SinglePath s1 = new SinglePath();
        s1.begin = new Coor(700, 700);
        s1.end = new Coor(150, 700);
        s1.time = 10;
        s1.delay = 0;

        Configs config = new Configs();
        config.Paths = new List<SinglePath>();
        config.Paths.Add(s1);

        string json = JsonMapper.ToJson(config);

        using (StreamWriter sw = new StreamWriter(Application.dataPath + "/StreamingAssets/" + "config.json"))
        {
            sw.Write(json);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


public struct SinglePath
{
    public double delay { get; set; }
    public double time { get; set; }
    public Coor begin { get; set; }
    public Coor end { get; set; }
}

public struct Coor
{
    public double x;
    public double y;

    public Coor(double _x , double _y)
    {
        x = _x;
        y = _y;
    }
}

public struct Configs
{
    public List<SinglePath> Paths { get; set; }
}