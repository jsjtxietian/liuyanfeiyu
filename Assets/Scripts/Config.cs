using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class Config : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Configs config = new Configs();
        //configPath(ref config);
        //configMask(ref config);
        //config.vacantCount = 100;
        //string json = JsonMapper.ToJson(config);
        //using (StreamWriter sw = new StreamWriter(Application.dataPath + "/StreamingAssets/" + "config.json"))
        //{
        //    sw.Write(json);
        //}
    }

    void configPath(ref Configs config)
    {
        SinglePath s1 = getPathFromItween("Path1");
        s1.time = 10;
        s1.delay = 0;

        SinglePath s2 = getPathFromItween("Path2");
        s2.time = 10;
        s2.delay = 0;

        SinglePath s3 = getPathFromItween("Path3");
        s3.time = 10;
        s3.delay = 0;

        SinglePath s4 = getPathFromItween("Path4");
        s4.time = 10;
        s4.delay = 0;

        
        config.Paths = new List<SinglePath>();

        config.Paths.Add(s1);
        config.Paths.Add(s2);
        config.Paths.Add(s3);
        config.Paths.Add(s4);

        config.wordDelay = 0.3;
    }

    void configMask(ref Configs config)
    {
        SingleMask s1 = getMaskFromScene("Mask1");
        SingleMask s2 = getMaskFromScene("Mask2");
        SingleMask s3 = getMaskFromScene("Mask3");

        config.Masks = new List<SingleMask>();

        config.Masks.Add(s1);
        config.Masks.Add(s2);
        config.Masks.Add(s3);
    }

    SinglePath getPathFromItween(string pathName)
    {
        Vector3[] temp = iTweenPath.GetPath(pathName);
        SinglePath s = new SinglePath();
        s.begin = new Coor(temp[0][0], temp[0][1]);
        s.end = new Coor(temp[1][0], temp[1][1]);

        return s;
    }

    SingleMask getMaskFromScene(string maskName)
    {
        RectTransform temp = transform.Find(maskName).GetComponent<RectTransform>();

        SingleMask s = new SingleMask();

        s.width = temp.rect.width;
        s.height = temp.rect.height;
        s.coor = new Coor(temp.anchoredPosition.x, temp.anchoredPosition.y);

        return s;
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

public struct SingleMask
{
    public double width { get; set; }
    public double height { get; set; }
    public Coor coor { get; set; }
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
    public double wordDelay { get; set; }
    public int vacantCount { get; set; }
    public List<SinglePath> Paths { get; set; }
    public List<SingleMask> Masks { get; set; }
}