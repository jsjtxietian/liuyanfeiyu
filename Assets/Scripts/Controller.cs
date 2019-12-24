using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using LitJson;
using System.Collections;

public class Controller : MonoBehaviour
{
    private Vector3[] path ;
    private Configs config;

    private string configPath ;
    private string inputPath ;
    private int vacant;

    public Transform TextParent;
    public GameObject Mask;
    public Text myText;
    private Text temptext;
    long i = 1;    //循环计数变量 注意，由于长时间开机，要防止数据溢出
    long id = 1;    //每个字的编号
    long vacantCount = 0;    //空闲时间计数，空闲达到一定时间显示提示文字
    long tipid = 1;    //提示文字的编号

    
    // Use this for initialization
    void Start()
    {
        Screen.SetResolution(1920, 1080, true);

        configPath = Application.dataPath + "/StreamingAssets/config.json";
        inputPath = Application.dataPath + "/StreamingAssets/input.txt";

        StreamReader sr = new StreamReader(configPath);
        string rawConfig = sr.ReadToEnd();
        config = JsonMapper.ToObject<Configs>(rawConfig);
        sr.Close();

        vacant = config.vacantCount;

        path = new Vector3[2];
        AddMask();

        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        i++;
        if (i <= 0) i = 1;    //防止溢出
        if (i % 10 == 0)
        {
            if (File.Exists(inputPath))    //此处路径需要根据实际做对应修改
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
                string str = File.ReadAllText(inputPath, Encoding.UTF8);
                File.Delete(inputPath);

                float delay = -(float)config.wordDelay;
                foreach (char c in str)
                {
                    delay += (float)config.wordDelay;
                    movePathwithConfig(c, delay);

                    id++;
                    if (id <= 0)
                        id = 1;
                }
            }
            else
            {
                vacantCount++;    //没发现message.txt, 空闲
                if (vacantCount > vacant)    //可以自己设置空闲多久后开始出现提示文字
                {
                    vacantCount = 0;
                    string str = "请在平板上输入文字";
                    float delay = - (float)config.wordDelay;
                    foreach (char c in str)
                    {
                        delay += (float)config.wordDelay;
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
        temptext.fontSize = config.fontSize;


        path[0] = new Vector3((float)(config.Paths[0].begin.x), (float)(config.Paths[0].begin.y), 0);
        path[1] = new Vector3((float)config.Paths[0].end.x, (float)config.Paths[0].end.y, 0);

        temptext.transform.position = path[0];
        Vector3 directionV = path[1] - path[0];
        float angleF = Mathf.Atan2(directionV.x, directionV.y) * Mathf.Rad2Deg;

        temptext.color = new Color(1, 1, 1, 0);
        temptext.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angleF));
        //iTween.RotateTo(temptext.gameObject,
        //    iTween.Hash("rotation", new Vector3(0, 0, -angleF), "time", 1));

        iTween.MoveTo(temptext.gameObject,
            iTween.Hash("path", path.Clone(), "time", config.Paths[0].time, "delay", delay, "easetype", iTween.EaseType.linear,
                "oncomplete", "movePath2", "oncompleteparams", temptext.gameObject, "oncompletetarget", gameObject,
                "onstart", "RepairAlpha", "onstartparams", temptext.gameObject, "onstarttarget", gameObject));

    }


    void movePath2(GameObject obj)
    {
        //path = iTweenPath.GetPath("Path2");

        path[0] = new Vector3((float)config.Paths[1].begin.x, (float)config.Paths[1].begin.y, 0);
        path[1] = new Vector3((float)config.Paths[1].end.x, (float)config.Paths[1].end.y, 0);

        obj.transform.position = path[0];
        Vector3 directionV = path[1] - path[0];
        float angleF = Mathf.Atan2(directionV.x, directionV.y) * Mathf.Rad2Deg;

        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angleF));
        obj.GetComponent<Text>().color = new Color(1, 1, 1, 0);
        //iTween.RotateTo(obj,
        //    iTween.Hash("rotation", new Vector3(0, 0, -angleF), "time", 1));

        iTween.MoveTo(obj,
            iTween.Hash( "path", path.Clone(), "time", config.Paths[1].time, "delay", config.Paths[1].delay, "easetype", iTween.EaseType.linear,
                 "oncomplete", "movePath3", "oncompleteparams", obj, "oncompletetarget", gameObject,
                 "onstart", "RepairAlpha", "onstartparams",obj, "onstarttarget", gameObject));
    }

    void movePath3(GameObject obj)
    {
        //path = iTweenPath.GetPath("Path3");
        path[0] = new Vector3((float)config.Paths[2].begin.x, (float)config.Paths[2].begin.y, 0);
        path[1] = new Vector3((float)config.Paths[2].end.x, (float)config.Paths[2].end.y, 0);

        obj.transform.position = path[0];
        Vector3 directionV = path[1] - path[0];
        float angleF = Mathf.Atan2(directionV.x, directionV.y) * Mathf.Rad2Deg;

        obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angleF));
        obj.GetComponent<Text>().color = new Color(1, 1, 1, 0);

        //iTween.RotateTo(obj,
        //    iTween.Hash("rotation", new Vector3(0, 0, -angleF), "time", 1));

        iTween.MoveTo(obj,
            iTween.Hash("path", path.Clone(), "time", config.Paths[2].time, "delay", config.Paths[2].delay, "easetype", iTween.EaseType.linear,
                 "oncomplete", "movePath4", "oncompleteparams", obj, "oncompletetarget", gameObject,
        "onstart", "RepairAlpha", "onstartparams",obj, "onstarttarget", gameObject));
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

    void RepairAlpha(GameObject obj)
    {
        obj.GetComponent<Text>().color = new Color(1, 1, 1, 1);
    }

    void destoryWord(GameObject self)
    {
        Destroy(self);
    }

    void AddMask()
    {
        for(int i  = 0; i< config.Masks.Count; i++)
        {
            SingleMask m = config.Masks[i];
            GameObject mask = Instantiate(Mask);
            mask.transform.SetParent(GameObject.Find("Canvas").transform,false);
            mask.GetComponent<RectTransform>().anchoredPosition = new Vector2((float)m.coor.x, (float)m.coor.y);
            mask.GetComponent<RectTransform>().sizeDelta = new Vector2((float)m.width, (float)m.height);

            mask.GetComponent<Image>().color = new Color(1, 0, 0);
            StartCoroutine(HideMask(mask));
        }
    }

    IEnumerator HideMask(GameObject mask)
    {
        yield return new WaitForSeconds(10);
        mask.GetComponent<Image>().color = new Color(0, 0, 0);
    }
}

