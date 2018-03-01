using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Archivos_de_Texto_y_Binarios
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            txtData.Text = getBpmData();
        }

        public string getBpmData()
        {
            openFileDialog1.ShowDialog();
            FileStream archivo = new FileStream(openFileDialog1.FileName, FileMode.Open);
            if (validBpm(archivo))
            {
                int width, heigh, pixels, bitsPerPixel;
                BinaryReader binaryFile = new BinaryReader(archivo);

                binaryFile.BaseStream.Seek(14, SeekOrigin.Begin); 
                pixels = binaryFile.ReadInt32();
                width = binaryFile.ReadInt32();
                heigh = binaryFile.ReadInt32();

                binaryFile.BaseStream.Seek(2, SeekOrigin.Current);

                bitsPerPixel = binaryFile.ReadInt16();

                binaryFile.Close();

                return "Tamaño: " + pixels.ToString() + "\r\n" + "Ancho: " + width.ToString() + "\r\n" + 
                    "Alto: " + heigh.ToString() + "\r\n" + 
                    "Bits por pixel: " + bitsPerPixel.ToString();

            }
            else
                return "Esto no es un BPM";
        }

        public bool validBpm(FileStream archivo)
        {
            BinaryReader binaryFile = new BinaryReader(archivo);
            int n;
            binaryFile.BaseStream.Seek(0, SeekOrigin.Begin);
            n = binaryFile.ReadInt16();
            if (n == 19778)
                return true;

            else
                return false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            CrearXml();
        }

        public void CrearXml()
        {
            saveFileDialog1.Filter = ".xml|*.xml";
            saveFileDialog1.ShowDialog();
            FileStream archivo = new FileStream(saveFileDialog1.FileName, FileMode.Create);

            XmlTextWriter writer = new XmlTextWriter(archivo, System.Text.Encoding.UTF8);

            writer.Formatting = Formatting.Indented;

             writer.WriteStartDocument();
            writer.WriteStartElement("Registro");
            writer.WriteElementString("nombre",txtNombre.Text);
            writer.WriteElementString("edad",txtEdad.Text);
            writer.WriteElementString("telefono",txtTelefono.Text);
            writer.WriteElementString("correo",txtCorreoElectronico.Text);
            writer.WriteElementString("direccion",txtDireccion.Text);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }
    }
}
