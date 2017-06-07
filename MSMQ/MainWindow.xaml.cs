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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MSMQ
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSendPayment_Click(object sender, RoutedEventArgs e)
        {
            Payment myPayment;
            myPayment.Payor = txtPayTo.Text;
            myPayment.Payee = txtAccount.Text;
            myPayment.Amount = Convert.ToInt32(txtAccount.Text);
            myPayment.DueDate = txtDueDate.Text;

            System.Messaging.Message msg = new System.Messaging.Message();
            msg.Body = myPayment;
            string queuePath = ".\\Private$\\billpay";
            if (!MessageQueue.Exists(queuePath))
            {
                MessageQueue.Create(queuePath);
            }
            MessageQueue msgQ = new MessageQueue(queuePath);
            msgQ.Send(msg);
        }

        private void btnProcessPayment_Click(object sender, RoutedEventArgs e)
        {
            MessageQueue msgQ = new MessageQueue(".\\Private$\\billpay");

            Payment myPayment = new Payment();
            Object o = new Object();
            System.Type[] arrTypes = new System.Type[2];
            arrTypes[0] = myPayment.GetType();
            arrTypes[1] = o.GetType();
            msgQ.Formatter = new XmlMessageFormatter(arrTypes);
            myPayment = ((Payment)msgQ.Receive().Body);

            StringBuilder sb = new StringBuilder();
            sb.Append("Payment paid to: " + myPayment.Payor);
            sb.Append("\n");
            sb.Append("Paid by: " + myPayment.Payee);
            sb.Append("\n");
            sb.Append("Amount: $" + myPayment.Amount.ToString());
            sb.Append("\n");
            sb.Append("Due Date: " + myPayment.DueDate);

            MessageBox.Show(sb.ToString(), "Message Received!");
        }
    }

    public struct Payment
    {
        public string Payor, Payee;
        public int Amount;
        public string DueDate;
    }
}

/*
This application works with a private queue that you must first create in the Computer Management console. To do this, follow these steps:

    On the desktop, right-click My Computer, and then click Manage.
    Expand the Services and Applications node to find Message Queuing.

    Note If you do not find Message Queuing, it is not installed.

Expand Message Queuing, right-click Private Queues, point to New, and then click Private Queue.
In the Queue name box, type billpay, and then click OK.

Note Do not select the Transactional check box. Leave the Computer Management console open because you return to it later to view messages. 





*/