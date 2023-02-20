# Unity-URP-LinearSpaceUI

本工程是一个示例演示工程，主要参考了 https://github.com/TakeshiCho/UI_RenderPipelineInLinearSpace 的改动，更新到了URP12.1.8版本。

## 演示结果

工程中提供了两张我在PS中制作的测试图片，一张是在PS中输出对应RGB通道的颜色块，一张是RGB为

（255,0,0），Alpha通道为0.5的图片。在Game窗口中查看结果：

**正确显示对应的RGB（可自行测试窗口中的RGB值）：**

![image-20230220121641051](https://user-images.githubusercontent.com/89976115/220012022-bb617985-fdca-4b69-86dd-649c6c508411.png)


**直接使用URP原本的设置没有正确混合（如左下角和黑色混合后得到的颜色为（188,0,0））:**

![image-20230220121459891](https://user-images.githubusercontent.com/89976115/220012083-5b0e878d-dc09-4d23-8d88-ded2044af613.png)


这是因为

```
gamma空间混合公式： color = (A.rgb * A.a) + (B.rgb * (1 - A.a))
Linear空间混合公式： color = ((A.rgb ^ 2.2 * A.a) + (B.rgb ^ 2.2 * (1 - A.a))) ^（1 / 2.2)
```

所以得到错误的混合颜色：

r = 188/256  = 0.734 约等于 0.5^(1/2.2) 

**正确混合（如左下角和黑色混合后得到的颜色为（128,0,0））：**

![image-20230220121533659](https://user-images.githubusercontent.com/89976115/220012112-ebb7778e-0abf-49a4-8793-f308327ecab9.png)


根据混合公式 color = (A.rgb * A.a) + (B.rgb * (1 - A.a))

r = 1 * 0.5 + 0 * 0.5 =  0.5  -> ( 0.5 *256 =128) ,得到了正确的混合结果

## 如何使用在新工程中

1.创建一个新工程，将Packages 目录下 com.unity.render-pipelines.universal@12.1.8 文件夹移动到新工程Packages 目录下。

2.在新工程中创建相应的URP Render Asset，并在Graphics中应用它

![image-20230220115445464](https://user-images.githubusercontent.com/89976115/220012153-becc711c-942e-44d1-a61b-3b2374564e93.png)

![image-20230220115904924](https://user-images.githubusercontent.com/89976115/220012188-8c884e89-0527-472a-87ae-b9301754c8c8.png)

![image-20230220115917393](https://user-images.githubusercontent.com/89976115/220012208-e533c5f1-e617-4074-a453-90135a05ffcb.png)

3.创建合适的UI相机，并添加到主相机堆栈中。

![image-20230220121301979](https://user-images.githubusercontent.com/89976115/220012227-56f756c9-2f1d-48f4-affa-84fb4f57bf09.png)


## Todo

如果你想应用此方案到实际项目中，建议仍需要做的工作：

1.同步结果至Scene窗口

2.适配多个相机

3.UI文字

...
