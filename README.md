# README

#### todo

方块可以变色，不纯黑

#### 打开后端

1. 打开Backend里面的config cmd.cmd
2. 参照水管的方法，关闭防火墙，获取ip地址
3. 然后ip+8000/index可访问地址，例如192.168.1.101:8000/index

#### 打开程序Liuyanfeiyu.exe

见config.json可进行参数配置：

* wordDelay控制每个字之间的时间间隔（秒）
* vacantCount控制现实“请输入文字”之间的时间间隔（帧）
* fontSize控制字号
* Paths为4个可见的文字路径(如图蓝字)的参数：
  * delay为开始前的等待时间（秒）
  * time为整个路径的时间长度（秒），可以用这个控制速度
  * begin为开始的坐标(越往右x越大，越往上y越大)
  * end为结束的坐标
* Masks为3个水管遮挡部件(如图黑字)的参数：
  * width为宽度
  * height为长度
  * coor里的x、y为坐标(越往右x越大，越往上y越大)

![test](.\Photo\test.jpg)
