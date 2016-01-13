using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

namespace ST
{
    public partial class Form1 : Form
    {
        SerialPort port;
        
        string returnMessage = "";
        int mod = 0;
        int amp = 0;  // amplitude
        int freq = 0;  //frequency
        int run = 0;
        int x = 0;
        int y = 0;
        int z = 0;


        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)  //run
        {
            // try to open the port
            try
            {
             
                port = new SerialPort("COM" + textBox3.Text, 9600);
                port.Open();

                run = 1;
                mod = 0;



                try
                {
                    freq = Int32.Parse(textBox1.Text);
                }
                catch (FormatException ex)
                {
                }



                try
                {
                    amp = Int32.Parse(textBox2.Text);
                }
                catch (FormatException ex)
                {
                }

                send();

 

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


  
            port.Close();
            
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
           

        }

 

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void DoUpdate(object sender, EventArgs e)
        {

        }



        private void serialPort1_DataReceived
            (object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
    
            this.Invoke(new EventHandler(DoUpdate));
        }



        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)  //stop button
        {
            // try to open the port
            try
            {

                port = new SerialPort("COM" + textBox3.Text, 9600);
                port.Open();


                run = 0;
                mod = 0;


                try
                {
                    freq = Int32.Parse(textBox1.Text);
                }
                catch (FormatException ex)
                {
                }



                try
                {
                    amp = Int32.Parse(textBox2.Text);
                }
                catch (FormatException ex)
                {
                }

                send();



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



            port.Close();
        }



        private void send() { //send signal to 
            byte[] buffer = new byte[4];
            buffer[0] = Convert.ToByte(run);  //run or not
            buffer[1] = Convert.ToByte(mod);  //module one : frequency module
            buffer[2] = Convert.ToByte(freq);  //send freqency
            buffer[3] = Convert.ToByte(amp);   //send amplitude





            int intReturnASCII = 0;
            char charReturnValue = (Char)intReturnASCII;


            port.Write(buffer, 0, 4);


            Thread.Sleep(1000);




            port.Close();

       
            Application.DoEvents(); //flush the data in the buffer
        
        
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                port = new SerialPort("COM" + textBox3.Text, 9600);
                port.Open();


                run = 1;  //make it run
                mod = 1;  //module 2




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            // number of bytes to read
            int count = port.BytesToRead;
           

            int [] inBuf = new int[2]; //initialize a 2 byte array.

            int temp = 0;

            while (run==1) 
            {
                

       
                {
                    inBuf[0] =  port.ReadByte(); //get 2 bytes first from micor controller
                    inBuf[1] =  port.ReadByte();

                    if ((inBuf[0] + (inBuf[1] * 256)) == 0xDEAD)
                    { //check if equal the start tag 0xDEAD

   
                        for (int i = 0; i < 3;i++ )  // get x y c acceleroment, each size of them is 2 byte
                        {

                             inBuf[0] = port.ReadByte();
                            inBuf[1] = port.ReadByte();
                            temp = 0;

                            if (i == 0)  //x
                            {
                                label8.Text = Convert.ToString(inBuf[0] + (inBuf[1] * 256));  // combine 2 byte to 1
                                temp = temp + (inBuf[0] + (inBuf[1] * 256));
                            }
                            else if (i == 1) //y
                            {
                                label9.Text = Convert.ToString(inBuf[0] + (inBuf[1] * 256));
                                temp = temp + (inBuf[0] + (inBuf[1] * 256));
                            }

                            else if (i == 2) //z
                            {
                                label10.Text = Convert.ToString(inBuf[0] + (inBuf[1] * 256));
                                temp = temp + (inBuf[0] + (inBuf[1] * 256));
                            }



                            perfChart1.AddValue(temp/30);  //reduce the size of the acceleroment and put it on the graph. Using the perf Chart.

                           

                            Application.DoEvents();
                        }
                        






                    }
                    else continue;

                }
                

            }
            
            
            

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            run = 0;
            try
            {
                port.Close();
            }
            catch (Exception ex) { }
         }

        private void perfChart1_Load(object sender, EventArgs e) //perfChar is a online resourse used to creat a realtime chart, and I use it draw the realtime graph for accelerometer sensor signal.
        {

        }



    }
}
