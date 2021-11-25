using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PoS
{
    public partial class PuntoDeVenta : Form
    {
        private double total = 0.0;
        private int reng = 0;
        private int tiempo;
        

        public PuntoDeVenta()
        {
            InitializeComponent();   
            
        }

        private void PuntoDeVenta_Load(object sender, EventArgs e)
        {
            timer1.Start();
            label1.Location = new Point(this.Width / 2 - label1.Width / 2, 0);
            label3.Text = DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString();
            label3.Width = 100;
            label3.Height = 20;
            label3.Location = new Point(this.Width - label3.Width , this.Height - label3.Height);
            dataGridView1.Location = new Point(40, label1.Height + 50);
            dataGridView1.Width = this.Width/2 +50;
            dataGridView1.Height = (this.Height / 4) * 3;
            //MessageBox.Show(this.Width + "  " + this.Height);
            dataGridView1.Columns[0].Width = 100;
            dataGridView1.Columns[1].Width = 150;
            dataGridView1.Columns[2].Width = 150;
            dataGridView1.Columns[3].Width = 150;
            dataGridView1.Columns[4].Width = dataGridView1.Width * 20 / 100 + 20;

            dataGridView1.RowTemplate.Height = 60;
            textBox1.Location = new Point(dataGridView1.Width/2 - textBox1.Width/2, this.Height - textBox1.Height - 15);
            label4.Location = new Point(dataGridView1.Width + label4.Width/2, dataGridView1.Height/4*4-150);
            dataGridView1.Columns[2].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[3].CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            label5.Location = new Point(dataGridView1.Width/2 - label5.Width/2, this.Height - textBox1.Height - label5.Height - 20);
            label7.Location = new Point(dataGridView1.Width + label4.Width/2, (dataGridView1.Height / 4)*4-100);
            label8.Location = new Point(dataGridView1.Width + label4.Width/2, (dataGridView1.Height / 4)*4);
            label2.Location = new Point(dataGridView1.Width + label4.Width / 2, (dataGridView1.Height / 4) * 4-50);
            button1.Location = new Point(dataGridView1.Width - button1.Width + 500, dataGridView1.Height + 100);
            button2.Location = new Point(dataGridView1.Width - button1.Width + 200, dataGridView1.Height + 75);
            pictureBox1.Location = new Point((dataGridView1.Width)+100, 50);
            NomE.Text = "Lo atiende "+Form1.nombre;
            NomE.Location = new Point(0, this.Height-NomE.Height);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Text = DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString();

            tiempo++;
            label9.Text = tiempo.ToString();
            if (label9.Text == "3")
            {
                pictureBox1.ImageLocation = "C:\\Users\\OMEN\\source\\repos\\Pos2\\PoS\\bin\\Debug\\img\\Img1.jpg";
            }
            if (label9.Text == "6")
            {
                pictureBox1.ImageLocation = "C:\\Users\\OMEN\\source\\repos\\Pos2\\PoS\\bin\\Debug\\img\\Img2.jpg";
            }
            if (label9.Text == "9")
            {
                timer1.Enabled = false;
                pictureBox1.ImageLocation = "C:\\Users\\OMEN\\source\\repos\\Pos2\\PoS\\bin\\Debug\\img\\Img3.jpg";
                tiempo = 0;
                label9.Text = "0";
                timer1.Start();
            }

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27 && dataGridView1.Rows.Count > 0)
            {

                if (Convert.ToInt32(dataGridView1.CurrentCell.Value) > 1)
                {
                    dataGridView1.CurrentCell.Value = Convert.ToInt32(dataGridView1.CurrentCell.Value) - 1;
                    totalProd();
                }
                else
                {
                    dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);
                }
                CalcularTotal();
            }
            if (e.KeyChar == 13)
            {
                String query = "SELECT * FROM productos WHERE producto_codigo =" + textBox1.Text;
                
                try
                {
                    MySqlConnection mySqlConnection = new MySqlConnection("server=127.0.0.1; user=root; password=root; database=verificador_productos; SSL mode=none");
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlConnection);
                    MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                    if (mySqlDataReader.HasRows)
                    {
                        
                        mySqlDataReader.Read();
                        int codigo = mySqlDataReader.GetInt32(0);
                        reng = 0;
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            reng++;
                            if (mySqlDataReader.GetString(0) == Convert.ToString(row.Cells[1].Value))
                            {
                                row.Cells[0].Value = Convert.ToInt32(row.Cells[0].Value) + 1;
                                break;
                            }
                            else
                            {
                                reng = 0;
                            }
                            
                        }
                        if (reng==0)
                        {
                            dataGridView1.Rows.Add(1, mySqlDataReader["producto_codigo"].ToString(), mySqlDataReader.GetDouble(3), codigo);

                        }
                        totalProd();
                        //MessageBox.Show(Convert.ToString(reng));
                        CalcularTotal();
                        textBox1.Clear();
                        textBox1.Focus();
                        label7.Text = "Pagó Con: 0.00";
                        label8.Text = "Cambio: 0.00";
                    }
                    else
                    {

                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            if (e.KeyChar == 'P' || e.KeyChar == 'p')
            {
                e.Handled = true;
                //MessageBox.Show($"¿Va a pagar? {textBox1.Text} {total} {Environment.NewLine} " +
                //    $"{Convert.ToDouble(textBox1.Text) - total}");
                label7.Text = $"Pagó Con: " + Convert.ToDouble(textBox1.Text);
                if (Convert.ToDouble(label7.Text)< total)
                {
                    label8.Text = "Aun faltan"+ (total- Convert.ToDouble(label7.Text));
                }
                else
                {
                    label8.Text = $"Cambio: {Math.Round(Convert.ToDouble(textBox1.Text) - total, 2)}";
                }
                dataGridView1.Rows.Clear();
                textBox1.Clear();
                textBox1.Focus();

            }
        }

        private void totalProd()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[3].Value = Convert.ToDouble(row.Cells[0].Value) * Convert.ToDouble(row.Cells[2].Value);
            }
            
        }
        private void CalcularTotal()
        {
            total = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                total += Double.Parse(dataGridView1[3,i].Value.ToString());
            }
            label4.Text = "Total: " + String.Format("{0:0.00}",total);
        }


		private void textBox1_KeyDown(object sender, KeyEventArgs e)
		{


    
        }

        private void button1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
                MessageBox.Show("Imprimiendo Recibo");
                

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 login = new Form1();
            login.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        { //En este metodo se realiza el pago de la venta
            Double pago = Double.Parse(textBox1.Text);
            if(pago >= total)
            {
                try
                {
                    //aqui se hace el insert para la creacion de la venta
                    String venta = "INSERT INTO `verificador_productos`.`ventas` " +
                        "(`fechaventa`, `horaventa`, `operadorVenta`) " +
                        "VALUES (CURRENT_DATE(), CURRENT_TIME(), " + Form1.id_usuario + ")";
                    MySqlConnection mySqlConnection = new MySqlConnection("server=127.0.0.1; user=root; password=root; database=verificador_productos; SSL mode=none");
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand(venta, mySqlConnection);
                    mySqlCommand.ExecuteNonQuery();

                    string query = "SELECT LAST_INSERT_ID()";
                    mySqlCommand = new MySqlCommand(query, mySqlConnection);
                    MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                    int idVenta = 0;
                    if (mySqlDataReader.HasRows)
                    {
                        mySqlDataReader.Read();
                        idVenta = mySqlDataReader.GetInt32(0);
                    }

                    mySqlDataReader.Close();

                    for (int i =0; i < dataGridView1.RowCount; i++){
                        // aqui se hacen los inserts a la tabla 'venta detalles' uno por uno
                        String insert = "INSERT INTO `verificador_productos`.`ventas_detalle`" +
                            "(`id_venta`,`id_producto`,`cantidad`,`precio_producto`)" +
                            " VALUES (" +idVenta.ToString()+","+dataGridView1[1,i].ToString() +","+ dataGridView1[0, i].ToString() + ", " + dataGridView1[2, i].ToString() + ")";
                        mySqlCommand = new MySqlCommand(insert, mySqlConnection);
                        mySqlCommand.ExecuteNonQuery();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                double cambio = pago - total;
                MessageBox.Show("Su cambio es: $" + cambio.ToString());
            } else
            {
                MessageBox.Show("Pago insuficiente");
            }
        }
    }
}
