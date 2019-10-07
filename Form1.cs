using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.InteropServices;
//using IWshRuntimeLibrary;



namespace 玄鸟刀剑2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f =new Form2();


            f.ShowDialog();

        }

        private void button2_Click(object sender, EventArgs e)
        {



           


            MD5 mD5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.UTF8.GetBytes(textBox2.Text);
            byte[] targetData = mD5.ComputeHash(fromData);
            string byte25tring = null;
            for (int i = 0; i < targetData.Length; i++)
            {
                byte25tring += targetData[i].ToString("x2");
            }


           // var request = (HttpWebRequest)WebRequest.Create("http://110.80.137.159:81/reg.php?u=" + textBox1.Text.Trim() + "&p=" + byte25tring);

            var request = (HttpWebRequest)WebRequest.Create("http://110.80.137.159:81/login.php?u=" + textBox1.Text.Trim() + "&p=" + byte25tring);



            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            // MessageBox.Show(byte25tring);
            //  MessageBox.Show(responseString);
           

            if (responseString.Length <=3)
            {

                MessageBox.Show("登陆失败,账号或密码错误");
                return;
            }
            // MessageBox.Show("可以登陆游戏了");


            string[] data = System.Text.RegularExpressions.Regex.Split(responseString, @"[|]");

            // MessageBox.Show(responseString);
            //  MessageBox.Show(data[0]);
           
                if (File.Exists("update_cfg.xml"))
                {
                    File.Delete("update_cfg.xml");
                }
               
                string srcFileName = @"xys.dll";
                string destFileName = @"update_cfg.xml";
                if (File.Exists(srcFileName))
                {
                    File.Move(srcFileName, destFileName);
                }
          



            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = "patcher.exe";//需要启动的程序名      
                                                 //a启动参数 

           // p.StartInfo.Arguments = " -u " + responseString + " -p 1111 -z 1"; //启动参数  
            p.StartInfo.Arguments = " -u " + data[0] + " -p 1111 -z 1"; //启动参数  

            p.Start();//启动    

            p.WaitForExit();

            File.Move(destFileName, srcFileName);



            System.Environment.Exit(0);


            return;





        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Mp3Player mp3 = new Mp3Player();
            mp3.FileName = "ini/音乐.mp3";
            mp3.play();

            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            CreateShortcut(desktop + @"\玄鸟刀剑2.lnk", "");


            //var request = (HttpWebRequest)WebRequest.Create("http://254595754.qzone.qq.com");


            //var response = (HttpWebResponse)request.GetResponse();
            //var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            //linkLabel1.Text = responseString.ToString();





        }

        ///
        ///
        private static void CreateShortcut(string lnkFilePath, string args = "")
        {
            /// <summary>
            /// 为当前正在运行的程序创建一个快捷方式。
            /// </summary>
            /// <param name="lnkFilePath">快捷方式的完全限定路径。</param>
            /// <param name="args">快捷方式启动程序时需要使用的参数。</param>
            var shellType = Type.GetTypeFromProgID("WScript.Shell");
            dynamic shell = Activator.CreateInstance(shellType);
            var shortcut = shell.CreateShortcut(lnkFilePath);
            shortcut.TargetPath = System.Reflection.Assembly.GetEntryAssembly().Location;
            shortcut.Arguments = args;
            shortcut.WorkingDirectory = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            shortcut.Save();
        }


        //播放MP3



        /// <SUMMARY>   
        /// clsMci 的摘要说明。   
        /// </SUMMARY>   
        public class Mp3Player
        {
            public Mp3Player()
            {
                //   
                // TODO: 在此处添加构造函数逻辑   
                //   
            }

            //定义API函数使用的字符串变量    
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            private string Name = "";
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            private string durLength = "";
            [MarshalAs(UnmanagedType.LPTStr, SizeConst = 128)]
            private string TemStr = "";
            int ilong;
            //定义播放状态枚举变量   
            public enum State
            {
                mPlaying = 1,
                mPuase = 2,
                mStop = 3
            };
            //结构变量   
            public struct structMCI
            {
                public bool bMut;
                public int iDur;
                public int iPos;
                public int iVol;
                public int iBal;
                public string iName;
                public State state;
            };

            public structMCI mc = new structMCI();

            //取得播放文件属性   
            public string FileName
            {
                get
                {
                    return mc.iName;
                }
                set
                {
                    //ASCIIEncoding asc = new ASCIIEncoding();    
                    try
                    {
                        TemStr = "";
                        TemStr = TemStr.PadLeft(127, Convert.ToChar(" "));
                        Name = Name.PadLeft(260, Convert.ToChar(" "));
                        mc.iName = value;
                        ilong = APIClass.GetShortPathName(mc.iName, Name, Name.Length);
                        Name = GetCurrPath(Name);
                        //Name = "open " + Convert.ToChar(34) + Name + Convert.ToChar(34) + " alias media";   
                        Name = "open " + Convert.ToChar(34) + Name + Convert.ToChar(34) + " alias media";
                        ilong = APIClass.mciSendString("close all", TemStr, TemStr.Length, 0);
                        ilong = APIClass.mciSendString(Name, TemStr, TemStr.Length, 0);
                        ilong = APIClass.mciSendString("set media time format milliseconds", TemStr, TemStr.Length, 0);
                        mc.state = State.mStop;
                    }
                    catch (Exception exp)
                    {
                       // Log.Error(exp);
                        //MessageBox.Show("出错错误!");
                    }
                }
            }
            //播放   
            public void play()
            {
                TemStr = "";
                TemStr = TemStr.PadLeft(127, Convert.ToChar(" "));
                APIClass.mciSendString("play media", TemStr, TemStr.Length, 0);
                mc.state = State.mPlaying;
            }
            //停止   
            public void StopT()
            {
                TemStr = "";
                TemStr = TemStr.PadLeft(128, Convert.ToChar(" "));
                ilong = APIClass.mciSendString("close media", TemStr, 128, 0);
                ilong = APIClass.mciSendString("close all", TemStr, 128, 0);
                mc.state = State.mStop;
            }

            public void Puase()
            {
                TemStr = "";
                TemStr = TemStr.PadLeft(128, Convert.ToChar(" "));
                ilong = APIClass.mciSendString("pause media", TemStr, TemStr.Length, 0);
                mc.state = State.mPuase;
            }
            private string GetCurrPath(string name)
            {
                if (name.Length < 1)
                    return "";
                name = name.Trim();
                name = name.Substring(0, name.Length - 1);
                return name;
            }
            //总时间   
            public int Duration
            {
                get
                {
                    durLength = "";
                    durLength = durLength.PadLeft(128, Convert.ToChar(" "));
                    APIClass.mciSendString("status media length", durLength, durLength.Length, 0);
                    durLength = durLength.Trim();
                    if (durLength == "")
                        return 0;
                    return (int)(Convert.ToDouble(durLength) / 1000f);
                }
            }

            //当前时间   
            public int CurrentPosition
            {
                get
                {
                    durLength = "";
                    durLength = durLength.PadLeft(128, Convert.ToChar(" "));
                    APIClass.mciSendString("status media position", durLength, durLength.Length, 0);
                    mc.iPos = (int)(Convert.ToDouble(durLength) / 1000f);
                    return mc.iPos;
                }
            }
        }

        public class APIClass
        {
            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern int GetShortPathName(
             string lpszLongPath,
             string shortFile,
             int cchBuffer
            );

            [DllImport("winmm.dll", EntryPoint = "mciSendString", CharSet = CharSet.Auto)]
            public static extern int mciSendString(
             string lpstrCommand,
             string lpstrReturnString,
             int uReturnLength,
             int hwndCallback
            );
        }
    


}



}
