﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using WoW_Realmlister_v3.Properties;

namespace WoW_Realmlister_v3
{
    public partial class Form2 : Form
    {
        public Form2(string name, string realmlist, string rpath, string wpath)
        {
            InitializeComponent();
            textBox1.Text = name;
            textBox2.Text = realmlist;
            textBox3.Text = rpath;
            textBox4.Text = wpath;

            this.name = name;
            this.realmlist = realmlist;
            this.rpath = rpath;
            this.wpath = wpath;

            button3.Visible = true;
        }

        public Form2()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog opdf = new OpenFileDialog();
            opdf.Filter = "Realmlist (.wtf)|realmlist.wtf";
            if (opdf.ShowDialog() == DialogResult.OK)
                textBox3.Text = opdf.FileName.ToString();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            OpenFileDialog opdf = new OpenFileDialog();
            opdf.Filter = "Wow (.exe)|Wow.exe";
            if (opdf.ShowDialog() == DialogResult.OK)
                textBox4.Text = opdf.FileName.ToString();
        }

        private void mephobiaButton1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim().Length != 0 && textBox2.Text.Trim().Length != 0)
            {
                try
                {
                    XDocument doc = XDocument.Load(Settings.Default["xml_filename"].ToString());
                    XElement Realm = new XElement("Realm", new XElement("Name", textBox1.Text),
                        new XElement("Realmlist", textBox2.Text),
                        new XElement("rPath", textBox3.Text),
                        new XElement("wPath", textBox4.Text));
                    doc.Root.Add(Realm);
                    doc.Save(Settings.Default["xml_filename"].ToString());
                }
                catch
                {
                    // Exceptions: wrong xml structure, empty file
                    //- So we create a new xml configuration file
                    Form1 form1 = new Form1();
                    form1.CreateDefaultXMLConfFile();

                    //- We add new data which failed in this exception
                    XDocument doc = XDocument.Load(Settings.Default["xml_filename"].ToString());
                    XElement Realm = new XElement("Realm", new XElement("Name", textBox1.Text),
                        new XElement("Realmlist", textBox2.Text),
                        new XElement("rPath", textBox3.Text),
                        new XElement("wPath", textBox4.Text));
                    doc.Root.Add(Realm);

                    doc.Save(Settings.Default["xml_filename"].ToString());
                    MessageBox.Show("An error was ocurred while adding new data in configuration file, we recommend you to restart your application.");
                }
            }

            this.Dispose();
            this.Close();
        }

        private void mephobiaButton2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim().Length != 0 && textBox2.Text.Trim().Length != 0)
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(Settings.Default["xml_filename"].ToString());

                    string namepath = "ServerInfo/Realm[Name='" + this.name + "']";
                    string realmlistaddress = "ServerInfo/Realm[Realmlist='" + this.realmlist + "']";
                    string realmlistpath = "ServerInfo/Realm[rPath='" + this.rpath + "']";
                    string woexepath = "ServerInfo/Realm[wPath='" + this.wpath + "']";

                    XmlNode _name = doc.SelectSingleNode(namepath);
                    XmlNode _realmlistaddress = _name.FirstChild.NextSibling;
                    XmlNode _realmlistpath = _realmlistaddress.NextSibling;
                    XmlNode _wowexepath = doc.SelectSingleNode(woexepath);

                    if (textBox1.Text != _name.FirstChild.InnerText)
                        _name.FirstChild.InnerText = textBox1.Text;

                    if (textBox2.Text != _realmlistaddress.InnerText)
                        _realmlistaddress.InnerText = textBox2.Text;

                    if (textBox3.Text != _realmlistpath.InnerText)
                        _realmlistpath.InnerText = textBox3.Text;

                    if (textBox4.Text != _wowexepath.LastChild.InnerText)
                        _wowexepath.LastChild.InnerText = textBox4.Text;

                    doc.Save(Settings.Default["xml_filename"].ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            this.Dispose();
            this.Close();
        }

        private string name;
        private string realmlist;
        private string rpath;
        private string wpath;
    }
}
