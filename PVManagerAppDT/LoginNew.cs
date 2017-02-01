using PVManagerAppDT.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PVManagerAppDT
{
    public partial class LoginNew : Form
    {
        AppDTEntities db = new AppDTEntities();
        public LoginNew()
        {
            InitializeComponent();
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            Acceso();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void Acceso()
        {
            string Pass =GetMD5(txtContra.Text);

            var login = db.USUARIOS_PV.Where(u=>u.User_Login==txtUsuario.Text && u.User_Password== Pass).ToList();
           
            if (login.Count() != 0)
            {
                //MessageBox.Show("Entra");
                txtContra.Text = "";
                txtUsuario.Text = "";
                lblError.Visible = false;
                IndexNew objForm = new IndexNew();
                objForm.Show();

            }
            else
            {
                //MessageBox.Show("No entro");
                lblError.Visible = true;
            }

        }

        public static string GetMD5(string str)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = md5.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

    }
}
