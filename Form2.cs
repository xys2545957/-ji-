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


namespace 玄鸟刀剑2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox2.Text.Equals(textBox3.Text))
            {
                MD5 mD5 = new MD5CryptoServiceProvider();
                byte[] fromData = System.Text.Encoding.UTF8.GetBytes(textBox2.Text);
                byte[] targetData = mD5.ComputeHash(fromData);
                string byte25tring = null;
                for (int i = 0; i < targetData.Length; i++)
                {
                    byte25tring += targetData[i].ToString("x2");
                }


                var request = (HttpWebRequest)WebRequest.Create("http://110.80.137.159:81/reg.php?u=" + textBox1.Text.Trim() + "&p=" + byte25tring);

              //var request = (HttpWebRequest)WebRequest.Create("http://110.80.137.159:81/login.php?u=" + textBox1.Text.Trim() + "&p=" + byte25tring);



                var response = (HttpWebResponse)request.GetResponse();
                 var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
               // MessageBox.Show(byte25tring);
               // MessageBox.Show(responseString);
                if (responseString.Length==1)
                {
                   
                    MessageBox.Show("注册失败,重复注册");
                    return;
                }

                MessageBox.Show("注册成功,可以登陆游戏了");
                return;
                

            }

            MessageBox.Show("请输两次一样的密码");          





        }
    }
}
