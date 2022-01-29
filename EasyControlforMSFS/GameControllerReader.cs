using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.Gaming.Input;
using System.Threading;
using System.Diagnostics;




// Instructions for use:
// 1. Use SDK.Contract nuget package
// 2. Change namespace in header to current project
// 3. Create object through: GameControllerReader mygamecontroller = new GameControllerReader()
// 4. 2 functions to call: ReadAvailableControllers and StartReadingController (with controller name as input)


namespace EasyControlforMSFS
{
    public class GameControllerReader
    {
        //RawGameController controller;

        static int max_nr_controllers = 10;
        static int smoothing_factor = 5;
        public double[,] axisArray = new double[max_nr_controllers,10]; // max 10 controllers with 10 axes each
        public double[,] axisArraySmooth = new double[max_nr_controllers, 10]; // max 10 controllers with 10 axes each
        public double[,,] axisInternalArraySmoothValues = new double[max_nr_controllers, 10, smoothing_factor]; // max 10 controllers with 10 axes each
        public bool[,] buttonArray = new bool[max_nr_controllers, 164]; // max 10 controllers with 30 buttons each
        public List<string> controllers_reading; //
        public GameControllerSwitchPosition[] switchArray; //not implemented further
        public RawGameController selectedcontroller;

        public GameControllerReader()
        // Any stuff to be done when instantiating the class
        {
            //Empty
            controllers_reading = new List<string>();
        }

        public string[] ReadAvailableGameControllers()
        // This function reads the game controllers (joysticks, throttles, etc) connected to the computer. Upon starting the first call, it can take a little while for the RawControllers.Any list to get populated
        {
            string[] returnstring;
            bool test = RawGameController.RawGameControllers.Any();
            if (RawGameController.RawGameControllers.Any())
            {
                returnstring = new string[RawGameController.RawGameControllers.Count];
                int counter = 0;
                foreach (var controller in RawGameController.RawGameControllers)
                {
                    var name = controller.DisplayName + "-" + controller.HardwareVendorId +"-" + controller.HardwareProductId;
                    returnstring[counter] = name;
                    Debug.WriteLine($"Controller {returnstring[counter]}");
                    counter += 1;
                }
            }
            else
            {
                returnstring = new string[1];
                returnstring[0] = "Controllers not loaded yet";
            }
            return returnstring;
        }

        public bool StartReadingController(string selectedcontrollernameInput)
        // This function is called from outside the class to start reading values from a specified controller
        {
            string selectedcontrollername = selectedcontrollernameInput;
            
            bool not_started = true;
            while (not_started)
            {
                if (RawGameController.RawGameControllers.Any())
                {
                    foreach (var controller in RawGameController.RawGameControllers)
                    {
                        string name = controller.DisplayName + "-" + controller.HardwareVendorId + "-" + controller.HardwareProductId;
                        if (name == selectedcontrollername)
                        {
                            Debug.WriteLine($"GameController: Found controller to start thread: {selectedcontrollername} {controller.DisplayName}");
                            selectedcontroller = controller;
                            //add controllers to controllers list
                            controllers_reading.Add(selectedcontrollername);
                            int controller_id = controllers_reading.Count-1;
                            // prepare arrays to read buttons and axis values
                            //int numaxis = selectedcontroller.AxisCount;
                            //int numbuttons = selectedcontroller.ButtonCount;
                            //axisArray = new double[numaxis]; // ALREADY INITIATED AT START
                            //buttonArray = new bool[numbuttons];// ALREADY INITIATED AT START

                            Thread thread = new Thread(() => StartReadingControllerThread(controller_id,selectedcontroller));
                            thread.IsBackground = true;
                            thread.Start();
                            not_started = false;
                            Debug.WriteLine($"GameController: Controller thread for {selectedcontroller.DisplayName} met id {controller_id} initiated");
                            return true;
                        }
                    }
                    if (not_started == true)
                    {
                        return false; //the selected joystickname is not connected or found
                    }
                }
            }
            return true; // this should never be returned as the while loop should return a value
        }

        private void StartReadingControllerThread(int id, RawGameController selectedcontroller)
        // This function is called from within the class to start the thread that continuously reads buttons and axis from the specified controller
        {
            Debug.WriteLine($"GameController: Controller {selectedcontroller.DisplayName} met id {id} thread starting");
            while (true)
            {
                var resulttuple = ReadAxisButtons(selectedcontroller);
                var tempAxisArray = resulttuple.Item1;
                for (int i = 0; i<tempAxisArray.Length; i++)
                {
                    axisArray[id,i] = tempAxisArray[i];
                    for (int j = 0 ; j <smoothing_factor-1; j++)
                    {
                        axisInternalArraySmoothValues[id, i, j] = axisInternalArraySmoothValues[id, i, j + 1];
                    }
                    axisInternalArraySmoothValues[id, i, smoothing_factor - 1] = axisArray[id, i];
                    //calculate avg - smoothed values
                    double sum = 0;
                    for (int j = 0; j < smoothing_factor; j ++)
                    {
                        sum += axisInternalArraySmoothValues[id, i, j];
                    }
                    axisArraySmooth[id, i] = sum / smoothing_factor; 
                }
                var tempButtonArray = resulttuple.Item2;
                for (int i = 0; i < tempButtonArray.Length; i++)
                {
                    buttonArray[id, i] = tempButtonArray[i];
                }
                Thread.Sleep(30);
            }
        }
        
        private Tuple<double[], bool[]> ReadAxisButtons(RawGameController selectedcontroller)
        // Reads the inputs of the selected controller
        {
            bool[] buttonArrayReading = new bool[selectedcontroller.ButtonCount];
            double[] axisArrayReading = new double[selectedcontroller.AxisCount];
            GameControllerSwitchPosition[] switchArrayReading = new GameControllerSwitchPosition[selectedcontroller.SwitchCount];

            if (RawGameController.RawGameControllers.Any())
            {
               var result = selectedcontroller.GetCurrentReading(buttonArrayReading, switchArrayReading, axisArrayReading);
            }
            return new Tuple<double[], bool[]>(axisArrayReading,buttonArrayReading);
        }

    }


}
