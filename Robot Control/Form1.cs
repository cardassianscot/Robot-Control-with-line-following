using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
// System.IO.Ports needed for SerialPort.DataRecieved event
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Robot_Control
{
    public partial class Form1 : Form
    {
        int[] values = new int[15];

        public Form1()
        {
            InitializeComponent();
            // set up cbPorts comboBox with available serial ports
            checkSerialPorts();
            // disable all buttons (until serial port connected)
            disableButtons();
            // allow detection of key presses
            KeyPreview = true;
            r = new radar(this,272,220);
            cbScanMode.SelectedIndex = 0;
            serialPort1.DtrEnable = true;
            for (int i = 0; i < 7; i++)
                values[i] = 0;
            for (int i = 7; i < 15; i++)
                values[i] = 150;
        }

        // when takeReading is true a sensor reading can be taken
        // stops repeat readings for a held space bar

        private radar r;
        private bool takeReading = true;
        private bool followLine = false;
        private bool lidarScan = false;
        private string buffer = "";
        private bool calibrated = false;
        private char prev = '\n';

        // A list of which keys are currently pressed in order of pressing
        private List<char> pressed = new List<char>();

        // Called on key press (KeyDown)
        // Ignores key presses when cbPortsInput, cbPortsOutput and textBox1 is focused.
        // Uses WASD keys for robot movement, space bar for sensor readings
        // stores keys in pressed list in order they are pressed, only stores each key once 
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // check if cbPortsInput, cbPortsOutput or textBox1 is focused, if so ignore keypresses
            if (!(cbPortsInput.Focused || cbPortsOutput.Focused || textBox1.Focused))
            {
                // convert key press to char c
                // WASD to move, don't send repeat keypresses by checking current direction
                // Updates dirLabel with direction
                // adds key to pressed list if not already in the list
                char c = Convert.ToChar(e.KeyCode);
                switch (c)
                {
                    case 'W':
                        if (dirLabel.Text!="Forward")
                        {
                            serialWrite("f");
                        }
                        dirLabel.Text = "Forward";
                        if (!pressed.Contains('W'))
                        {
                            pressed.Add('W');
                        }
                        break;
                    case 'A':
                        if (dirLabel.Text != "Left")
                        {
                            serialWrite("l");
                        }
                        dirLabel.Text = "Left";
                        if (!pressed.Contains('A'))
                        {
                            pressed.Add('A');
                        }
                        break;
                    case 'S':
                        if (dirLabel.Text != "Back")
                        {
                            serialWrite("b");
                        }
                        dirLabel.Text = "Back";
                        if (!pressed.Contains('S'))
                        {
                            pressed.Add('S');
                        }
                        break;
                    case 'D':
                        if (dirLabel.Text != "Right")
                        {
                            serialWrite("r");
                        }
                        dirLabel.Text = "Right";
                        if (!pressed.Contains('D'))
                        {
                            pressed.Add('D');
                        }
                        break;
                    case ' ':
                        if (takeReading)
                        {
                            serialWrite("#");
                            // stop repeat readings from one space bar press
                            takeReading = false;
                        }
                        break;
                }
            }
        }

        // Called on key release
        // Ignores key releases if cbPortsInput, cbPortsOutput or textBox1 is focused
        // If another key is pressed moves in direction of first key to be pressed otherwise
        // send stop signal if key released was WASD
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            // check if cbPortsInput, cbPortsOutput or textBox1 is focused, if so ignore key released
            if (!(cbPortsInput.Focused || cbPortsOutput.Focused || textBox1.Focused))
            {
                // convert key press to char c
                // sends a stop character "s" when keys released is WASD
                // updates dirLabel with "Stop"
                char c = Convert.ToChar(e.KeyCode);
                // remove key from pressed list 
                pressed.Remove(c);
                // if there is another key in pressed list, set movement according to first pressed
                if (pressed.Count>0)
                {
                    switch (pressed[0])
                    {
                        case 'W':
                            if (dirLabel.Text != "Forward")
                            {
                                serialWrite("f");
                            }
                            dirLabel.Text = "Forward";
                            break;
                        case 'A':
                            if (dirLabel.Text != "Left")
                            {
                                serialWrite("l");
                            }
                            dirLabel.Text = "Left";
                            break;
                        case 'S':
                            if (dirLabel.Text != "Back")
                            {
                                serialWrite("b");
                            }
                            dirLabel.Text = "Back";
                            break;
                        case 'D':
                            if (dirLabel.Text != "Right")
                            {
                                serialWrite("r");
                            }
                            dirLabel.Text = "Right";
                            break;
                    }
                }
                else
                {
                    serialWrite("s");
                    dirLabel.Text = "Stop";
                }
                // when space bar is released allow another press to take a reading
                if (c==' ')
                {
                    takeReading = true;
                }
            }
        }

        // called on connect/disconnect button click
        // opens or closes both serial ports
        private void btnConnectClick(object sender, EventArgs e)
        {
            // if connect, open serial ports
            if (btnConnect.Text=="Connect")
            {
                serialOpen();
            }
            // if disconnect, close serial ports
            else if (btnConnect.Text == "Disconnect")
            {
                serialClose();
            }
        }

        // Open the serial ports according to cbPortsInput and cbPortsOutput
        // if cbPortsOutput is "shared" then only open cbPortsInput
        // if either serial port fails to open display error message using MessageBox
        private void serialOpen()
        {
            try
            {
                // set serialPort1 port to cbPortsInput and open
                serialPort1.PortName = cbPortsInput.Text;
                serialPort1.Open();
                // stores the current serialPort name, not yet saved
                // inside try so only if open succeeds
                Properties.Settings.Default.InputPortName = cbPortsInput.Text;
                // if cbPortsOutput is not "shared" open it
                if (cbPortsOutput.SelectedIndex != 0)
                {
                    serialPort2.PortName = cbPortsOutput.Text;
                    serialPort2.Open();
                }
                // stores the current serialPort name, not yet saved
                // inside try so only if open succeeds
                Properties.Settings.Default.OutputPortName = cbPortsOutput.Text;
                // set connect/disconnect button to disconnect
                btnConnect.Text = "Disconnect";
                // enable all buttons
                enableButtons();
                // save settings, inside try so only if both opens succeeds 
                Properties.Settings.Default.Save();
            }
            catch
            {
                // if open serial port fails report error to user with MessageBox
                MessageBox.Show("Serial Port not connected\nBluetooth out of range");
            }
            
        }

        // close both serial ports, checking both are open
        // disables buttons
        private void serialClose()
        {
            // if serialPort1 is open, close it
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
            // if serialPort2 is open, close it
            if (serialPort2.IsOpen)
            {
                serialPort2.Close();
            }
            // change connect/disconnect button to connect
            btnConnect.Text = "Connect";
            // disable all buttons
            disableButtons();
        }

        // write the string s to SerialPort1
        // on error writing to SerialPort1 officially close both serial ports 
        // and use a MessageBox to report dropped Bluetooth connection to user
        private void serialWrite(string s)
        {
            try
            {
                // write s to serialPort1
                serialPort1.Write(s);
            }
            // if serialPort1 fails
            catch
            {
                // close both serial ports
                serialClose();
                // display error message to user using MessageBox
                MessageBox.Show("Serial Port disconnected\nBluetooth out of range");
            }
        }

        // called on Send button click
        // sends serial commands from textBox1
        private void btnSend_Click(object sender, EventArgs e)
        {
            serialPort1.Write(textBox1.Text);
        }

        // called on fwd button press
        // sends serial command "f" to make robot go forward and update dirLabel
        private void fwdMouseDown(object sender, MouseEventArgs e)
        {
            dirLabel.Text = "Forward";
            serialWrite("f");
        }

        // called on fwd button release
        // sends serial command "s" to make robot stop and update dirLabel
        private void fwdMouseUp(object sender, MouseEventArgs e)
        {
            dirLabel.Text = "Stop";
            serialWrite("s");
        }

        // called on back button press
        // sends serial command "b" to make robot go backward and update dirLabel
        private void backMouseDown(object sender, MouseEventArgs e)
        {
            dirLabel.Text = "Back";
            serialWrite("b");
        }

        // called on back button release
        // sends serial command "s" to make robot stop and update dirLabel
        private void backMouseUp(object sender, MouseEventArgs e)
        {
            dirLabel.Text = "Stop";
            serialWrite("s");
        }

        // called on left button press
        // sends serial command "l" to make robot rotate left and update dirLabel
        private void leftMouseDown(object sender, MouseEventArgs e)
        {
            dirLabel.Text = "Left";
            serialWrite("l");
        }

        // called on left button release
        // sends serial command "s" to make robot stop and update dirLabel
        private void leftMouseUp(object sender, MouseEventArgs e)
        {
            dirLabel.Text = "Stop";
            serialWrite("s");
        }

        // called on right button press
        // sends serial command "l" to make robot rotate right and update dirLabel
        private void rightMouseDown(object sender, MouseEventArgs e)
        {
            dirLabel.Text = "Right";
            serialWrite("r");
        }

        // called on right button release
        // sends serial command "s" to make robot stop and update dirLabel
        private void rightMouseUp(object sender, MouseEventArgs e)
        {
            dirLabel.Text = "Stop";
            serialWrite("s");
        }

        // called on sensor button click
        // sends serial command "#" to request sensor data
        private void btnSensor_Click(object sender, EventArgs e)
        {
            serialWrite("#");
        }

        // disable all buttons and key press detection
        private void disableButtons()
        {
            // disable buttons
            btnSend.Enabled = false;
            btnFwd.Enabled = false;
            btnBack.Enabled = false;
            btnLeft.Enabled = false;
            btnRight.Enabled = false;
            btnSensor.Enabled = false;
            btnFollowLine.Enabled = false;
            btnLidar.Enabled = false;
            btnCalibrate.Enabled = false;
            calibrated = false;
            // disable detection of KeyDown (press) and KeyUp (release)
            KeyDown -= new KeyEventHandler(Form1_KeyDown);
            KeyUp -= new KeyEventHandler(Form1_KeyUp);
        }

        // enable all buttons and key press detection
        private void enableButtons()
        {
            // enable buttons
            btnSend.Enabled = true;
            btnFwd.Enabled = true;
            btnBack.Enabled = true;
            btnLeft.Enabled = true;
            btnRight.Enabled = true;
            btnSensor.Enabled = true;
            btnLidar.Enabled = true;
            btnCalibrate.Enabled = true;
            if (calibrated)
            {
                btnFollowLine.Enabled = true;
            }
            // disable detection of KeyDown (press) and KeyUp (release)
            KeyDown += new KeyEventHandler(Form1_KeyDown);
            KeyUp += new KeyEventHandler(Form1_KeyUp);
        }

        // populates the cbPortsInput and cbPortsOutput comboBoxes with available serial ports
        // checks saved settings for last used ports
        // if not available defaults to the first serial port
        private void checkSerialPorts()
        {
            // create portNames array with available serial ports
            string[] portNames = SerialPort.GetPortNames();
            // add "Shared" as first item of cbPortsOutput
            // used when input and output are handled by one port
            cbPortsOutput.Items.Add("Shared");
            // copy names from serial port to comboBoxes
            foreach (string name in portNames)
            {
                cbPortsInput.Items.Add(name);
                cbPortsOutput.Items.Add(name);
            }

            // if there are serial ports available select a default
            if (cbPortsInput.Items.Count > 0)
            {
                // if there is a stored input port name from last time try to use it
                if (Properties.Settings.Default.InputPortName != "")
                {
                    // i = the index of the saved InputPortName in the comboBox or -1 if not found
                    int i = Array.IndexOf(portNames, Properties.Settings.Default.InputPortName);
                    // select that option or the first option if not available
                    cbPortsInput.SelectedIndex = Math.Max(i, 0); 
                }
                // if no stored input port name
                else
                {
                    // select the first option
                    cbPortsInput.SelectedIndex = 0;
                }
                // if there is a stored output port name from last time try to use it
                if (Properties.Settings.Default.OutputPortName != "")
                {
                    // i = the index of the saved OutputPortName in the comboBox or -1 if not found
                    int i = Array.IndexOf(portNames, Properties.Settings.Default.OutputPortName) + 1;
                    // select that option or the first option if not available
                    cbPortsOutput.SelectedIndex = i;
                }
                // if no stored output port name
                else
                {
                    // select the first option if no saved settings
                    cbPortsOutput.SelectedIndex = 0;
                }
            }
        }

        // Called when data is recieved on either of the 2 serial ports
        // writes data to serialDisplay RichTextBox and scrolls to bottom
        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // get correct serial port 
            SerialPort sp = (SerialPort)sender;
            // read data from serial port
            string inData = sp.ReadExisting();
            // write data to richTextBox - serialDisplay
            // Invoke needed due to serial events occuring in a different thread
            BeginInvoke(new EventHandler(delegate
            {
                processSerialData(inData);
            }));
        }

        // If the line starts with # get a full line (using buffer) 
        // and pass it to the radar obejct r to deal with
        // otherwise send it to serialDisplay as it arrives
        // 
        // all input should be on seperate lines 
        // seperated with '\n'
        private void processSerialData(string inData)
        {
            char[] letters = inData.ToCharArray();
            foreach (char c in letters)
            {
                if (buffer == "" && c!='#')
                {
                    // don't display double blank lines
                    if (c!='\n' || prev != '\n')
                    {
                        prev = c;
                        // display data
                        serialDisplay.AppendText(c.ToString());
                        // autoscroll to bottom of text
                        serialDisplay.ScrollToCaret();
                    }
                }
                else if (c=='#')
                {
                    buffer = c.ToString();
                }
                else if (c==';')
                {
                    r.processData(buffer.Trim('#'));
                    buffer = "";
                }
                else
                {
                    buffer += c.ToString();
                }
            }
        }

        private void followLine_Click(object sender, EventArgs e)
        {
            if (followLine)
            {
                serialWrite("Q");
                btnFollowLine.BackColor = SystemColors.Control;
                followLine = false; ;
            } 
            else
            {
                serialWrite("q");
                btnFollowLine.BackColor = Color.Green;
                followLine = true;
            }
            
        }

        private void lidar_Click(object sender, EventArgs e)
        {
            if (lidarScan)
            {
                serialWrite("W");
                btnLidar.BackColor = SystemColors.Control;
                lidarScan = false; ;
            }
            else
            {
                serialWrite("w");
                btnLidar.BackColor = Color.Green;
                lidarScan = true;
            }
        }

        private void btnCalibrate_Click(object sender, EventArgs e)
        {
            FormCalibration f = new FormCalibration(values);
            f.ShowDialog();
            if (f.manual)
            {
                serialWrite("C");
                calibrated = true;
                btnFollowLine.Enabled = true;

            }
            else if (f.auto)
            {
                serialWrite("c");
                calibrated = true;
                btnFollowLine.Enabled = true;
            }
            else if (f.update)
            {
                serialWrite("u");
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = f.values[i];
                    serialWrite(""+values[i]);
                    serialWrite(i == values.Length - 1 ? ";" : ",");
                }   
            }
        }

        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            serialWrite("v" + vScrollBar1.Value + ";");
        }

        public class radar
        // Point class: width - width of box, height - height of box, 
        // radius - max radius of points to plot inside box (used for scaling r into box)
        // when the following are set they update their counterparts (read/write)
        // thetaDeg - angle in degrees
        // thetaRad - angle in radians
        // r - radius
        // copyR - copies the radius from another Point - doesn't scale with radius
        // Read only co-ordinates x,y
        {
            private Form _form = null;
            private Pen myPen = new Pen(Color.ForestGreen, 2);
            // Brush used for background of lidar scan semi-circle
            private SolidBrush WhiteBrush = new SolidBrush(Color.White);
            // Used for drawing all the points
            private Graphics g;
            // Bitmap where image is updated in the background before sending to screen
            // used to refresh the window 
            private Bitmap m_objDrawingSurface;
            // the number of past points to keep 
            const int history = 3;
            // use for the end of the scan line for Lidar
            Point end;

            // set of points, with old data up to history
            Point[,] points = new Point[181, history];
            // set of colours, pens and brushes with colours for old points
            Color[] myColours = new Color[history];
            Pen[] myPens = new Pen[history];
            SolidBrush[] myBrush = new SolidBrush[history];
            // variable for rectangle used to draw background
            Rectangle rectBounds;
            // previous angle
            int previous = 0;

            int scanMode;
            int locationX, locationY;

            public radar(Form f, int x = 12, int y = 2)
            {
                _form = f;
                locationX = x;
                locationY = y;
                Point.width = 200;
                Point.height = 100;
                // set the maximum distance measured by the LIDAR device 
                Point.radius = 1000;
                // set the distance from centre of servo to sensor
                Point.baseR = 16;
                // set the colours for the history points
                myColours[0] = Color.Black;
                myColours[1] = Color.SlateGray;
                myColours[2] = Color.LightSlateGray;
                // initiallize Points, Pen and Brush arrays
                for (int j = 0; j < history; j++)
                {
                    for (int i = 0; i < 181; i++)
                    {
                        points[i, j] = new Point();
                        points[i, j].thetaDeg = i;
                    }
                    myPens[j] = new Pen(myColours[j], 1);
                    myBrush[j] = new SolidBrush(myColours[j]);
                }
                // setup the length of the end point of the LIDAR line
                end = new Point();
                end.r = 1000;
                // create the bitmap to draw LIDAR image to 
                m_objDrawingSurface = new Bitmap(200, 100, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                // create the rectBounds rectangle to give size to semi-circle for LIDAR display
                rectBounds = new Rectangle(0, 0,
                      m_objDrawingSurface.Width, m_objDrawingSurface.Height * 2 - 1);
                rectBounds.Inflate(-1, -1);
                // set the default LIDAR mode 
                scanMode = 0;
            }

            public void setScanMode(int s)
            {
                scanMode = s;
            }

            public void redraw(Graphics h)
            {
                h.DrawImage(m_objDrawingSurface, locationX, locationY,
                    m_objDrawingSurface.Width,
                    m_objDrawingSurface.Height);
            }

            // called to draw image first time
            public void InitializeSurface()
            {
                // Create a Graphics object that references the bitmap and clear it.
                g = Graphics.FromImage(m_objDrawingSurface);
                g.Clear(SystemColors.Control);

                // Draw a white semi-circle outlined in black
                g.FillPie(WhiteBrush, rectBounds, 180, 180);
                g.DrawPie(Pens.Black, rectBounds, 180, 180);
            }

            public void processData(string s)
            {
                char[] delimiterChars = { ',', '\n' };
                int thetaDeg, r;
                string[] n = s.Split(delimiterChars);
                int.TryParse(n[0], out thetaDeg);
                int.TryParse(n[1], out r);

                // shifts points[thetaDeg,i] to points[thetaDeg,i+1]
                for (int i = history - 1; i > 0; i--)
                {
                    points[thetaDeg, i].copyR = points[thetaDeg, i - 1].r;
                }
                points[thetaDeg, 0].r = r;

                // Shows scan depending on cbScanMode setting 
                if (scanMode == 0)
                {
                    // Write 3 dots of fading color
                    write3Image(thetaDeg);
                }
                else if (scanMode == 1)
                {
                    // Write 1 dot where color depends of closeness to other dots
                    writeClosenessImage(thetaDeg);
                }
                else if (scanMode == 2)
                {
                    // Write image where colour depends on age of point
                    writeTimeImage(thetaDeg);
                }
                // save previous angle
                previous = thetaDeg;
            }

            // Write the image with the last 3 points for each location with older in lighter greys
            private void write3Image(int a)
            {
                writeBackground(a);
                g = Graphics.FromImage(m_objDrawingSurface);
                for (int j = history - 1; j > -1; j--)
                {
                    for (int i = 0; i < 180; i++)
                    {
                        // plot point unless point is at origin
                        if (points[i, j].r != Point.rZero)
                        {
                            g.DrawRectangle(myPens[j], points[i, j].x - 1, points[i, j].y - 1, 3, 3);
                            g.FillRectangle(myBrush[j], points[i, j].x - 1, points[i, j].y - 1, 3, 3);
                        }
                    }
                }
                // redraw picture
                _form.Invalidate();
            }

            // Write the image with the greyness value set by how close it is to the nearest points
            private void writeClosenessImage(int a)
            {
                writeBackground(a);
                g = Graphics.FromImage(m_objDrawingSurface);
                int distance;
                // range is the +- value of degrees to measure the distance to 
                int range = 5;

                for (int i = 0; i < 180; i++)
                {
                    // work out the minimum distance to +- range
                    distance = Point.height;
                    for (int j = 1; j < range + 1; j++)
                    {
                        distance = Math.Min(distanceBetween(i, i - j), distance);
                        distance = Math.Min(distanceBetween(i, i + j), distance);
                    }
                    // convert the distance to an rgbValue and use for a brush and pen
                    int rgbValue = map(distance, 0, Point.height / 4, 0, 200);
                    Color c = Color.FromArgb(rgbValue, rgbValue, rgbValue);
                    Brush myBrush = new SolidBrush(c);
                    Pen myPen = new Pen(c, 1);
                    // Plot points unless point is at origin
                    if (points[i, 0].r != Point.rZero)
                    {
                        g.DrawRectangle(myPen, points[i, 0].x - 1, points[i, 0].y - 1, 3, 3);
                        g.FillRectangle(myBrush, points[i, 0].x - 1, points[i, 0].y - 1, 3, 3);
                    }
                }
                // redraw picture
                _form.Invalidate();
            }

            // write image based on the age of the point
            private void writeTimeImage(int a)
            {
                writeBackground(a);
                g = Graphics.FromImage(m_objDrawingSurface);

                for (int i = 0; i < 180; i++)
                {
                    // set color depending on age of point
                    int rgbValue = map(age(a, i), 0, 360, 0, 255);
                    Color c = Color.FromArgb(rgbValue, rgbValue, rgbValue);
                    Brush myBrush = new SolidBrush(c);
                    Pen myPen = new Pen(c, 1);
                    // Plot points unless point is at origin
                    if (points[i, 0].r != Point.rZero)
                    {
                        g.DrawRectangle(myPen, points[i, 0].x - 1, points[i, 0].y - 1, 3, 3);
                        g.FillRectangle(myBrush, points[i, 0].x - 1, points[i, 0].y - 1, 3, 3);
                    }
                }
                // redraw picture
                _form.Invalidate();
            }

            // scale n from smin-smax to fmin-fmax
            // confine n to smin-smax
            private int map(int n, int smin, int smax, int fmin, int fmax)
            {
                n = Math.Min(smax, Math.Max(smin, n));
                return ((n - smin) * (fmax - fmin) / (smax - smin) + fmin);
            }

            // distance points[i] and points[j] with a max of Point.height
            // returns max value (Point.height) if either has a r=0
            private int distanceBetween(int i, int j)
            {
                int distance, x, y;
                if (j > -1 && j < 181)
                {
                    x = Math.Min(points[i, 0].x - points[j, 0].x, Point.height);
                    y = points[i, 0].y - points[j, 0].y;
                    distance = points[i, 0].r == Point.rZero || points[j, 0].r == Point.rZero ?
                        Point.height : Convert.ToInt32(Math.Sqrt(x * x + y * y));
                }
                else
                {
                    distance = Point.height;
                }
                return distance;
            }

            // Calculate the age of the point a, based on the current angle
            // returns age between 0 and 360
            private int age(int current, int a)
            {
                // if it is moving from 0 to 180
                if (current > previous)
                {
                    // if a>current angle has not yet been reached on this swing
                    if (a > current)
                    {
                        // time = time from angle to 0 + time from 0 to current
                        return (a + current);
                    }
                    // angle has been passed on this swing
                    else
                    {
                        // time = time from 0 to current - time from 0 to angle
                        return (current - a);
                    }
                }
                // now moving from 180 to 0
                else
                {
                    // angle has been reached on this swing
                    if (a > current)
                    {
                        // time = angle - current
                        return (a - current);
                    }
                    // angle has not yet been reached on this swing
                    else
                    {
                        // time = time from 180 to current * 2 (both ways) + current - angle
                        return ((180 - current) * 2 + current - a);
                    }
                }
            }

            // write the semicircle in white with black outline
            // also write a green line at angle a
            private void writeBackground(int a)
            {
                g = Graphics.FromImage(m_objDrawingSurface);
                g.Clear(SystemColors.Control);
                g.FillPie(WhiteBrush, rectBounds, 180, 180);
                g.DrawPie(Pens.Black, rectBounds, 180, 180);
                end.thetaDeg = a;
                g.DrawLine(myPen, Point.width / 2, Point.height - 1, end.x, end.y);
            }

            private class Point
            {
                static public int width { get; set; }
                static public int height { get; set; }
                static private double _radius; // max distance can read + distance from sensor to centre of servo
                static private double _radiusOrig; // max distance can read
                static private double _baseR; // distance from sensor to centre of servo
                static private double _rZero; // scaled distance from sensor to centre of servo
                private int _thetaDeg;
                private double _thetaRad;
                private double _r;
                private int _x;
                private int _y;

                // scaled distance from sensor to centre of servo
                static public double rZero
                {
                    get
                    {
                        return _rZero;
                    }
                }

                // distance from sensor to centre of servo
                // updates _radius and _rZero
                static public double baseR
                {
                    get
                    {
                        return _baseR;
                    }
                    set
                    {
                        _baseR = value;
                        _radius = _radiusOrig + _baseR;
                        _rZero = _baseR * height / _radius;
                    }
                }

                // max distance can read + distance from sensor to centre of servo
                // updates _radiusOrig, _radius and _rZero
                static public double radius
                {
                    get
                    {
                        return _radius;
                    }
                    set
                    {
                        _radiusOrig = value;
                        _radius = _radiusOrig + _baseR;
                        _rZero = _baseR * height / _radius;
                    }
                }


                // initialises point with angle=0 and radius=_rZero
                public Point()
                {
                    _thetaDeg = 0;
                    _thetaRad = 0;
                    _r = _rZero;
                    _x = Convert.ToInt32(_r * Math.Cos(_thetaRad) + width / 2);
                    _y = Convert.ToInt32(height - _r * Math.Sin(_thetaRad));
                }

                public int thetaDeg
                {
                    get
                    {
                        return _thetaDeg;
                    }
                    set
                    {
                        _thetaDeg = value;
                        _thetaRad = Math.PI * value / 180;
                        _x = Convert.ToInt32(_r * Math.Cos(_thetaRad) + width / 2);
                        _y = Convert.ToInt32(height - _r * Math.Sin(_thetaRad));
                    }
                }

                public double thetaRad
                {
                    get
                    {
                        return _thetaRad;
                    }
                    set
                    {
                        _thetaRad = value;
                        _thetaDeg = Convert.ToInt32(180 * value / Math.PI);
                        _x = Convert.ToInt32(_r * Math.Cos(_thetaRad) + width / 2);
                        _y = Convert.ToInt32(height - _r * Math.Sin(_thetaRad));
                    }
                }

                // copyR copy r value without scaling
                public double copyR
                {
                    set
                    {
                        _r = value;
                        _x = Convert.ToInt32(_r * Math.Cos(_thetaRad) + width / 2);
                        _y = Convert.ToInt32(height - _r * Math.Sin(_thetaRad));
                    }
                }

                public double r
                {
                    get
                    {
                        return _r;
                    }
                    set
                    {
                        _r = value + _baseR;
                        if (_r > _radius)
                        {
                            _r += 100;
                        }
                        _r *= height / _radius;
                        _x = Convert.ToInt32(_r * Math.Cos(_thetaRad) + width / 2);
                        _y = Convert.ToInt32(height - _r * Math.Sin(_thetaRad));
                    }
                }

                public int x
                {
                    get
                    {
                        return _x;
                    }
                }

                public int y
                {
                    get
                    {
                        return _y;
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            r.InitializeSurface();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            r.redraw(e.Graphics);
        }

        private void cbScanMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            r.setScanMode(cbScanMode.SelectedIndex);
        }
    }
}
