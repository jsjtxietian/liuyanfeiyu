using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using LitJson;

public class Controller : MonoBehaviour
{
    private Vector3[] path ;
    private Configs config;

    private string filePath = @"C:\Users\jsjtx\Desktop\message.txt";
    public Transform TextParent;
    public Text myText;
    private Text temptext;
    long i = 1;    //循环计数变量 注意，由于长时间开机，要防止数据溢出
    long id = 1;    //每个字的编号
    long vacantCount = 24;    //空闲时间计数，空闲达到一定时间显示提示文字
    long tipid = 1;    //提示文字的编号

    
    // Use this for initialization
    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        StreamReader sr = new StreamReader(Application.dataPath + "/StreamingAssets/" + "config.json");
        string rawConfig = sr.ReadToEnd();
        config = JsonMapper.ToObject<Configs>(rawConfig);
        sr.Close();

        path = new Vector3[2];
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        i++;
        if (i <= 0) i = 1;    //防止溢出
        if (i % 10 == 0)
        {
            if (File.Exists(filePath))    //此处路径需要根据实际做对应修改
            {
                vacantCount = 0;    //重置空闲时间计数
                for (int ii = 1; ii <= tipid; ii++)    //如果当前有提示文字，清空它们
                {
                    if (GameObject.Find("tip" + ii.ToString()))
                    {
                        Destroy(GameObject.Find("tip" + ii.ToString()));
                    }
                }
                tipid = 1;
                string str = File.ReadAllText(filePath, Encoding.UTF8);
                File.Delete(filePath);

                float delay = -0.3f;    //控制一段话之内每个字的流出延迟，防止一次全显示出来
                foreach (char c in str)
                {
                    delay += 0.3f;
                    temptext = Instantiate(myText) as Text;
                    temptext.text = c.ToString();
                    temptext.GetComponent<Transform>().SetParent(TextParent, false);
                    temptext.GetComponent<Transform>().position = new Vector3(800.0f, 435.0f, 0);    //初始位置要根据实际定位
                    temptext.name = id.ToString();
                    path = iTweenPath.GetPath("Path1");
                    iTween.RotateTo(GameObject.Find(id.ToString()), 
                        iTween.Hash("rotation", new Vector3(0, 0, 85), "delay", 2.5 + delay, "time", 0.1));
                    iTween.MoveTo(GameObject.Find(id.ToString()), 
                        iTween.Hash("path", path, "time", 5, "delay", delay, "easetype", iTween.EaseType.easeOutQuad, "movetopath", true));
                    id++;
                    if (id <= 0) id = 1;    //防止数据溢出
                }
            }
            else
            {
                vacantCount++;    //没发现message.txt, 空闲
                if (vacantCount > 24)    //可以自己设置空闲多久后开始出现提示文字
                {
                    vacantCount = 0;
                    string str = "请在平板上输入文字";
                    float delay = - (float)config.wordDelay;
                    foreach (char c in str)
                    {
                        delay += (float)config.wordDelay;
                        //movePath(c, delay);
                        movePathwithConfig(c, delay);

                        tipid++;
                        if (tipid <= 0)
                            tipid = 1;
                    }
                }

            }
        }
    }

    /* movePath 系列函数利用iTweenPath来控制字符的运动轨迹和旋转等动作属性，具体函数调用方式请参考iTweenPath官方说明*/
    void movePathwithConfig(char c, float delay)
    {
        temptext = Instantiate(myText) as Text;
        temptext.text = c.ToString();
        temptext.transform.SetParent(TextParent, false);
        temptext.name = "tip" + tipid.ToString();

        path[0] = new Vector3((float)(config.Paths[0].begin.x), (float)(config.Paths[0].begin.y), 0);
        path[1] = new Vector3((float)config.Paths[0].end.x, (float)config.Paths[0].end.y, 0);

        temptext.transform.position = path[0];
        Vector3 directionV = path[1] - path[0];
        float angleF = Mathf.Atan2(directionV.x, directionV.y) * Mathf.Rad2Deg;

        iTween.RotateTo(temptext.gameObject,
            iTween.Hash("rotation", new Vector3(0, 0, -angleF), "time", 1));

        iTween.MoveTo(temptext.gameObject,
            iTween.Hash("path", path.Clone(), "time", config.Paths[0].time, "delay", delay, "easetype", iTween.EaseType.linear,
                "oncomplete", "movePath2", "oncompleteparams", temptext.gameObject, "oncompletetarget", gameObject));
    }


    void movePath2(GameObject obj)
    {
        //path = iTweenPath.GetPath("Path2");

        path[0] = new Vector3((float)config.Paths[1].begin.x, (float)config.Paths[1].begin.y, 0);
        path[1] = new Vector3((float)config.Paths[1].end.x, (float)config.Paths[1].end.y, 0);

        obj.transform.position = path[0];
        Vector3 directionV = path[1] - path[0];
        float angleF = Mathf.Atan2(directionV.x, directionV.y) * Mathf.Rad2Deg;

        iTween.RotateTo(obj,
            iTween.Hash("rotation", new Vector3(0, 0, -angleF), "time", 1));

        iTween.MoveTo(obj,
            iTween.Hash( "path", path.Clone(), "time", config.Paths[1].time, "delay", config.Paths[1].delay, "easetype", iTween.EaseType.linear,
                 "oncomplete", "movePath3", "oncompleteparams", obj, "oncompletetarget", gameObject));
    }

    void movePath3(GameObject obj)
    {
        //path = iTweenPath.GetPath("Path3");
        path[0] = new Vector3((float)config.Paths[2].begin.x, (float)config.Paths[2].begin.y, 0);
        path[1] = new Vector3((float)config.Paths[2].end.x, (float)config.Paths[2].end.y, 0);

        obj.transform.position = path[0];
        Vector3 directionV = path[1] - path[0];
        float angleF = Mathf.Atan2(directionV.x, directionV.y) * Mathf.Rad2Deg;

        iTween.RotateTo(obj,
            iTween.Hash("rotation", new Vector3(0, 0, -angleF), "time", 1));

        iTween.MoveTo(obj,
            iTween.Hash("path", path.Clone(), "time", config.Paths[2].time, "delay", config.Paths[2].delay, "easetype", iTween.EaseType.linear,
                 "oncomplete", "movePath4", "oncompleteparams", obj, "oncompletetarget", gameObject));
    }

    void movePath4(GameObject obj)
    {
        //path = iTweenPath.GetPath("Path4");
        path[0] = new Vector3((float)config.Paths[3].begin.x, (float)config.Paths[3].begin.y, 0);
        path[1] = new Vector3((float)config.Paths[3].end.x, (float)config.Paths[3].end.y, 0);

        obj.transform.position = path[0];
        Vector3 directionV = path[1] - path[0];
        float angleF = Mathf.Atan2(directionV.x, directionV.y) * Mathf.Rad2Deg;

        iTween.RotateTo(obj,
            iTween.Hash("rotation", new Vector3(0, 0, -angleF), "time", 1));

        iTween.MoveTo(obj,
            iTween.Hash("path", path.Clone(), "time", config.Paths[3].time, "delay", config.Paths[3].delay, "easetype", iTween.EaseType.linear,
                 "oncomplete", "destoryWord", "oncompleteparams", obj, "oncompletetarget", gameObject));
    }

    void destoryWord(GameObject self)
    {
        Destroy(self);
    }

}

