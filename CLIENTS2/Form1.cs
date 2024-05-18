using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CLIENTS2
{
    public partial class Form1 : Form
    {

        HubConnection connection;
        public Form1()
        {
            InitializeComponent();
            connection = new HubConnectionBuilder().WithUrl("http://localhost:5067/chatHub").Build();
            connection.StartAsync();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (connection != null)
            {
                connection.On<string, string>("ReceiveMessage",
                    (string user, string message) =>
                    {
                        DateTime getDate = DateTime.Now;
                        if (lstChatBox.InvokeRequired)
                        {
                            lstChatBox.Invoke((MethodInvoker)delegate ()
                            {
                                lstChatBox.Items.Add(message + "\t\t" + getDate.ToString("yyyy/MM/dd - HH:mm:ss"));
                            }
                            );
                        }
                    });
            }
        }

        private void btnJoinChat_Click(object sender, EventArgs e)
        {
            UserConnection userConnection = new UserConnection();
            userConnection.ConnectionID = connection.ConnectionId;
            userConnection.UserName = txtUserName.Text;
            connection.InvokeCoreAsync("JoinChat", new object[] { userConnection });
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            connection.InvokeCoreAsync("SendMessage", new object[] { txtReceiver.Text, txtUserName.Text, txtUserName.Text + ": " + txtMess.Text });
        }
    }

    public class UserConnection
    {
        public string UserName { get; set; }
        public string ConnectionID { get; set; }
    }
}
