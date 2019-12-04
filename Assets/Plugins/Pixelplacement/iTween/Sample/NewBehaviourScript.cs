using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class NewBehaviourScript : MonoBehaviour
{

    private Vector3[] path;
    private Vector3[] path1;
    private Vector3[] path2;
    private Vector3[] path3;
    private Vector3[] path4;
    private Vector3[] path5;
    private string filePath = @"C:\Users\jsjtx\Desktop\message.txt";

    public Text myText;
    public Font STXingkai;
    private Text temptext;
    long i = 1;    //循环计数变量 注意，由于长时间开机，要防止数据溢出
    long id = 1;    //每个字的编号
    long vacantCount = 0;    //空闲时间计数，空闲达到一定时间显示提示文字
    long tipid = 1;    //提示文字的编号
    string results;

    /* movePath 系列函数利用iTweenPath来控制字符的运动轨迹和旋转等动作属性，具体函数调用方式请参考iTweenPath官方说明*/
    void movePath(GameObject obj)
    {
        path1 = iTweenPath.GetPath("New Path 2");
        iTween.MoveTo(obj, iTween.Hash("path", path1, "time", 2, "easetype", iTween.EaseType.linear, "oncomplete", "movePath2", "oncompleteparams", obj));
        iTween.RotateTo(obj, iTween.Hash("rotation", new Vector3(0, 0, 90), "delay", 0, "time", 0.1));
    }

    void movePath2(GameObject obj)
    {
        path2 = iTweenPath.GetPath("New Path 3");
        iTween.MoveTo(obj, iTween.Hash("path", path2, "time", 4, "easetype", iTween.EaseType.linear, "oncomplete", "movePath3", "oncompleteparams", obj));
        iTween.RotateTo(obj, iTween.Hash("rotation", new Vector3(0, 0, 90), "delay", 0, "time", 1));

    }

    void movePath3(GameObject obj)
    {
        path3 = iTweenPath.GetPath("New Path 4");
        iTween.MoveTo(obj, iTween.Hash("path", path3, "time", 4, "easetype", iTween.EaseType.linear, "oncomplete", "movePath4", "oncompleteparams", obj));
        iTween.RotateTo(obj, iTween.Hash("rotation", new Vector3(0, 0, 90), "delay", 0, "time", 2, "easetype", iTween.EaseType.linear));
    }

    void movePath4(GameObject obj)
    {
        path4 = iTweenPath.GetPath("New Path 5");
        iTween.MoveTo(obj, iTween.Hash("path", path4, "time", 7, "easetype", iTween.EaseType.linear, "oncomplete", "movePath6", "oncompleteparams", obj));
        iTween.RotateTo(obj, iTween.Hash("rotation", new Vector3(0, 0, -90), "delay", 0, "time", 3));
    }

    void movePath5(GameObject obj)
    {
        Destroy(obj);
    }

    void movePath6(GameObject obj)
    {
        path5 = iTweenPath.GetPath("New Path 6");
        iTween.MoveTo(obj, iTween.Hash("path", path5, "time", 5, "easetype", iTween.EaseType.easeOutQuad, "oncomplete", "movePath5", "oncompleteparams", obj));
        iTween.RotateTo(obj, iTween.Hash("rotation", new Vector3(0, 0, 0), "delay", 0, "time", 3));
    }

    // Use this for initialization
    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        Cursor.visible = false;
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
                    temptext.GetComponent<Transform>().SetParent(GameObject.Find("Image").GetComponent<Transform>(), false);
                    temptext.GetComponent<Transform>().position = new Vector3(800.0f, 435.0f, 0);    //初始位置要根据实际定位
                    temptext.name = id.ToString();
                    path = iTweenPath.GetPath("New Path 1");
                    iTween.RotateTo(GameObject.Find(id.ToString()), iTween.Hash("rotation", new Vector3(0, 0, 85), "delay", 2.5 + delay, "time", 0.1));
                    iTween.MoveTo(GameObject.Find(id.ToString()), iTween.Hash("path", path, "time", 5, "delay", delay, "easetype", iTween.EaseType.easeOutQuad, "movetopath", true, "oncomplete", "movePath", "oncompleteparams", GameObject.Find(id.ToString())));
                    id++;
                    if (id <= 0) id = 1;    //防止数据溢出
                }
            }
            else
            {
                vacantCount++;    //没发现message.txt, 空闲
                if (vacantCount > 100)    //可以自己设置空闲多久后开始出现提示文字
                {
                    vacantCount = 0;
                    string str = "请在平板上输入文字";
                    float delay = -0.3f;
                    foreach (char c in str)
                    {
                        delay += 0.3f;
                        temptext = Instantiate(myText) as Text;
                        temptext.text = c.ToString();
                        temptext.GetComponent<Transform>().SetParent(GameObject.Find("Image").GetComponent<Transform>(), false);
                        temptext.GetComponent<Transform>().position = new Vector3(800.0f, 435.0f, 0);
                        temptext.name = "tip" + tipid.ToString();
                        path = iTweenPath.GetPath("New Path 1");
                        iTween.RotateTo(GameObject.Find("tip" + tipid.ToString()), iTween.Hash("rotation", new Vector3(0, 0, 85), "delay", 2.5 + delay, "time", 0.1));
                        iTween.MoveTo(GameObject.Find("tip" + tipid.ToString()), iTween.Hash("path", path, "time", 5, "delay", delay, "easetype", iTween.EaseType.easeOutQuad, "movetopath", true, "oncomplete", "movePath", "oncompleteparams", GameObject.Find("tip" + tipid.ToString())));
                        tipid++;
                        if (tipid <= 0)
                            tipid = 1;
                    }
                }
            }
        }
    }

}
