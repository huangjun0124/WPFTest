using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MSMQ
{
    /// <summary>
    /// Interaction logic for Main2.xaml
    /// </summary>
    public partial class Main2 : Window
    {
        private void ShowMessagePopUp(string msg)
        {
            MessageBox.Show(msg);
        }

        public Main2()
        {
            InitializeComponent();
        }

        private string Path;
        /// <summary>
        /// 1.通过Create方法创建使用指定路径的新消息队列
        /// </summary>
        /// <param name="queuePath"></param>
        public void Createqueue(string queuePath )
        {
            try
            {
                if (!MessageQueue.Exists(queuePath))
                {
                    MessageQueue.Create(queuePath);
                    ShowMessagePopUp("消息队列创建成功");
                }
                else
                {
                    //MessageBox.Show(queuePath + "已经存在");
                    MessageQueue.Delete(queuePath);
                    MessageQueue.Create(queuePath);
                    ShowMessagePopUp(queuePath + "删除重建");
                }
                Path = queuePath;
            }
            catch (MessageQueueException e)
            {
                ShowMessagePopUp(e.Message);
            }
        }

        /// <summary>
        ///  2.连接消息队列并发送消息到队列
        /// 远程模式：MessageQueue rmQ = new MessageQueue("FormatName:Direct=OS:machinename//private$//queue");
        ///     rmQ.Send("sent to regular queue - Atul");对于外网的MSMQ只能发不能收
        /// </summary>
        public void SendMessage(string msg)
        {
            try
            {
                //连接到本地队列
                MessageQueue myQueue = new MessageQueue(Path);
                //MessageQueue myQueue = new MessageQueue("FormatName:Direct=TCP:192.168.12.79//Private$//myQueue1");
                //MessageQueue rmQ = new MessageQueue("FormatName:Direct=TCP:121.0.0.1//private$//queue");--远程格式
                Message myMessage = new Message();
                myMessage.Body = msg;
                myMessage.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                //发生消息到队列中
                myQueue.Send(myMessage);
                MessageBox.Show("消息发送成功");
            }
            catch (Exception e)
            {
                ShowMessagePopUp(e.Message);
            }
        }

        /// <summary>
        /// 3.连接消息队列并从队列中接收消息
        /// </summary>
        public void ReceiveMessage()
        {
            MessageQueue myQueue = new MessageQueue(Path);
            myQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            try
            {
                //从队列中接收消息
                Message myMessage = myQueue.Receive();// myQueue.Peek();--接收后不消息从队列中移除
                string context = myMessage.Body.ToString();
                ShowMessagePopUp("消息内容：" + context);
            }
            catch (MessageQueueException e)
            {
                ShowMessagePopUp(e.Message);
            }
            catch (InvalidCastException e)
            {
                ShowMessagePopUp(e.Message);
            }
        }



        /// <summary>
        /// 4.清空指定队列的消息
        /// </summary>
        public void ClearMessage()
        {
            MessageQueue myQueue = new MessageQueue(Path);
            myQueue.Purge();
            ShowMessagePopUp(string.Format("已清空对了{0}上的所有消息", Path));
        }


        /// <summary>
        /// 5.连接队列并获取队列的全部消息
        /// </summary>
        public void GetAllMessage()
        {
            MessageQueue myQueue = new MessageQueue(Path);
            Message[] allMessage = myQueue.GetAllMessages();
            XmlMessageFormatter formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            for (int i = 0; i < allMessage.Length; i++)
            {
                allMessage[i].Formatter = formatter;
                ShowMessagePopUp(string.Format("第{0}机密消息为:{1}", i + 1, allMessage[i].Body.ToString()));
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            SendMessage(txtMsg.Text);
        }

        private void btnCreateQueue(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtQueuePath.Text))
            {
                Createqueue(txtQueuePath.Text);
            }
        }

        private void btnGet_Click(object sender, RoutedEventArgs e)
        {
            ReceiveMessage();
        }

        private void btnGetAll_Click(object sender, RoutedEventArgs e)
        {
            GetAllMessage();
        }

        private void btnAnotherWnd_Click(object sender, RoutedEventArgs e)
        {
            Main2 another = new Main2();
            another.Show();
        }

        private void btnBeginReceive_Click(object sender, RoutedEventArgs e)
        {
            string path = txtQueuePath.Text;
            MessageQueue mq;
            if (MessageQueue.Exists(path))
            {
                mq = new MessageQueue(path);
            }
            else
            {
                mq = MessageQueue.Create(path);
            }
            mq.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            mq.ReceiveCompleted += mq_ReceiveCompleted;
            mq.BeginReceive();
        }

        private void mq_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            MessageQueue mq = (MessageQueue)sender;
            System.Messaging.Message m = mq.EndReceive(e.AsyncResult);
            //处理消息
            string str = m.Body.ToString();
            ShowMessagePopUp(str);
            //继续下一条消息
            mq.BeginReceive();
        }
    }
}
