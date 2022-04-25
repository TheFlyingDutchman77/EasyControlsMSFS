using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Configuration;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;


namespace EasyControlforMSFS
{
    /// <summary>
    /// Interaction logic for AddDefinitionWindow.xaml
    /// </summary>
    public partial class AddDefinitionWindow : Window
    {
        /// <summary>
        /// This List contains elements of the class AircraftControls. In this class, we store - per controller - the aircraft, axis and button events that are defined. 
        /// </summary>
        public GameControllerReader mygamecontroller;
        public string[] available_controllers = new string[10];
        public bool[] buttonArray = new bool[160];

        public AircraftControls aircraftControls;
        public SimConnectImplementer mysimconnect;
        public string str_selected_aircraft;

        
        /// <summary>
        /// This loads the window to add definitions
        /// </summary>
        public AddDefinitionWindow(AircraftControls aircraftControlsInput, GameControllerReader myGameControllerInput, SimConnectImplementer mysimconnectInput, string str_selected_aircraftInput)        
        {
            InitializeComponent();
            mygamecontroller = myGameControllerInput;
            aircraftControls = aircraftControlsInput;
            mysimconnect = mysimconnectInput;
            str_selected_aircraft = str_selected_aircraftInput;


            Thread readControllers = new Thread(ReadControllers);
            readControllers.IsBackground = true;
            readControllers.Start();

            Thread readControllersState = new Thread(ReadControllersState);
            readControllersState.IsBackground = true;
            readControllersState.Start();

            //Now we add the elements to the combobox
            aircraftControls.aircraft.Sort();
            for (int j = 0; j < aircraftControls.aircraft.Count; j++)
            {
                ACnamesComboBox.Items.Add(aircraftControls.aircraft[j]);
            }
            ACnamesComboBox.SelectedItem = str_selected_aircraft;
            aircraftControls.all_events.Sort();
            for (int j = 0; j < aircraftControls.all_events.Count; j++)
            {
                Axis1EventsComboBox.Items.Add(aircraftControls.all_events[j]);
                Axis2EventsComboBox.Items.Add(aircraftControls.all_events[j]);
                Axis3EventsComboBox.Items.Add(aircraftControls.all_events[j]);
                Axis4EventsComboBox.Items.Add(aircraftControls.all_events[j]);
                Axis5EventsComboBox.Items.Add(aircraftControls.all_events[j]);
                Axis6EventsComboBox.Items.Add(aircraftControls.all_events[j]);
            }
            aircraftControls.all_events.Sort();
            for (int j = 0; j < aircraftControls.all_events.Count; j++)
            {
                //Button1Events.Items.Add(button_events[j]);
            }
        }

        /// <summary>
        /// Thread to read controllers connected
        /// </summary>
        public void ReadControllers()
        {
            available_controllers[0] =  "Controllers not loaded yet";
            while (available_controllers[0] == "Controllers not loaded yet")
            {
                for (int i = 0; i < mygamecontroller.controllers_reading.Count; i++)
                {
                    available_controllers[i] = mygamecontroller.controllers_reading[i];
                }
                Debug.WriteLine($"Controller gevonden: {available_controllers[0]}");
                if (available_controllers[0] == "Controllers not loaded yet")
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        ControllersComboBox.Text = "Controllers loading - connect controller";
                    });
                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        ControllersComboBox.Text = "Select controller...";
                        for (int j = 0; j < available_controllers.Length; j++)
                        {
                            Debug.WriteLine($"Controller: {available_controllers[j]}");
                            ControllersComboBox.Items.Add(available_controllers[j]);
                        }
                    });
                }
                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// Thread to read selected controllers buttons and indicate when pressed
        /// </summary>
        public void ReadControllersState()
        {
            while (true)
            {
                string controller_selected_name = "";
                int controller_selected = -1;
                this.Dispatcher.Invoke(() =>
                {
                    controller_selected = ControllersComboBox.SelectedIndex;
                });
                if (controller_selected != -1)
                {
                    this.Dispatcher.Invoke(() => { controller_selected_name = ControllersComboBox.SelectedItem.ToString(); });
                    int controller_id = Array.FindIndex(available_controllers, val => val.Equals(controller_selected_name));
                    Thread.Sleep(30);
                    bool button_pressed = false;
                    for (int i = 0; i < buttonArray.Length; i++)
                    {
                        buttonArray[i] = mygamecontroller.buttonArray[controller_id, i];
                        if (buttonArray[i] == true) { button_pressed = true; }
                    }

                    if (button_pressed)
                    { 
                        this.Dispatcher.Invoke(() =>
                        {
                            ControllerConnectedLabel.Content = "identified!";
                            ControllerConnectedLabel.Foreground = Brushes.Green;
                        });
                    }
                    else
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            ControllerConnectedLabel.Content = "(press button to identify)";
                            ControllerConnectedLabel.Foreground = Brushes.White;
                        });
                    }
                }
            }
        }

        /// <summary>
        /// On exit...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Convert datastructure aircraft_controls back to a valid controls.xml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if ((ACnamesComboBox.SelectedIndex != -1) && (ControllersComboBox.SelectedIndex != -1))
            {
                string selected_aircraft = ACnamesComboBox.SelectedItem.ToString();
                string selected_controller = ControllersComboBox.SelectedItem.ToString();
                Debug.WriteLine(selected_aircraft);
                //First, we look if the controller is already in the aircraft_controls definition
                var index = aircraftControls.aircraft_controls.FindIndex(c => c.controller_name == selected_controller);

                // if found, we check if the aircraft exists already for this controller
                if (index > -1)
                {
                    int pos = Array.IndexOf(aircraftControls.aircraft_controls[index].aircraft_names, selected_aircraft);
                    if (pos > -1) //yes, it is already defined
                    {
                        SetInputValuestoList(index, pos);
                    }
                    else
                    {
                        // For a new aircraft for existing controller
                        aircraftControls.aircraft_controls[index].num_aircraft += 1;
                        pos = aircraftControls.aircraft_controls[index].num_aircraft;
                        aircraftControls.aircraft_controls[index].AddAircraft(pos, selected_aircraft);
                        for (int j = 0; j < aircraftControls.aircraft_controls[index].max_nr_axis; j++)
                        {
                            aircraftControls.aircraft_controls[index].axis_events[pos, j] = "";
                            aircraftControls.aircraft_controls[index].axis_events2[pos, j] = "";
                        }
                        for (int j = 0; j < aircraftControls.aircraft_controls[index].max_nr_buttons; j++)
                        {
                            aircraftControls.aircraft_controls[index].button_events[pos, j] = "";
                        }
                        SetInputValuestoList(index, pos);
                    }
                }
                else
                {
                    //For a new controller (so everything will be new)
                    aircraftControls.aircraft_controls.Add(new AircraftControls.AircraftControlsData() { controller_name = selected_controller });
                    index = aircraftControls.aircraft_controls.FindIndex(c => c.controller_name == selected_controller);
                    aircraftControls.aircraft_controls[index].AddAircraft(1, selected_aircraft);
                    aircraftControls.aircraft_controls[index].num_aircraft += 1;
                    SetInputValuestoList(index, 1);
                }
                aircraftControls.SaveXML(aircraftControls);
            }
            else
            {
                MessageBox.Show("Please select controller and aircraft first.");
            }
            
        }

        /// <summary>
        /// Called to set the datastructure values
        /// </summary>
        /// <param name="index"></param>
        /// <param name="pos"></param>
        private void SetInputValuestoList(int index, int pos)
        {
            if (Axis1EventsComboBox.SelectedIndex != -1) { aircraftControls.aircraft_controls[index].axis_events[pos, 0] = Axis1EventsComboBox.SelectedItem.ToString(); aircraftControls.aircraft_controls[index].AddAxis(pos, 1); }
            if (Axis2EventsComboBox.SelectedIndex != -1) { aircraftControls.aircraft_controls[index].axis_events[pos, 1] = Axis2EventsComboBox.SelectedItem.ToString(); aircraftControls.aircraft_controls[index].AddAxis(pos, 2); }
            if (Axis3EventsComboBox.SelectedIndex != -1) { aircraftControls.aircraft_controls[index].axis_events[pos, 2] = Axis3EventsComboBox.SelectedItem.ToString(); aircraftControls.aircraft_controls[index].AddAxis(pos, 3); }
            if (Axis4EventsComboBox.SelectedIndex != -1) { aircraftControls.aircraft_controls[index].axis_events[pos, 3] = Axis4EventsComboBox.SelectedItem.ToString(); aircraftControls.aircraft_controls[index].AddAxis(pos, 4); }
            if (Axis5EventsComboBox.SelectedIndex != -1) { aircraftControls.aircraft_controls[index].axis_events[pos, 4] = Axis5EventsComboBox.SelectedItem.ToString(); aircraftControls.aircraft_controls[index].AddAxis(pos, 5); }
            if (Axis6EventsComboBox.SelectedIndex != -1) { aircraftControls.aircraft_controls[index].axis_events[pos, 5] = Axis6EventsComboBox.SelectedItem.ToString(); aircraftControls.aircraft_controls[index].AddAxis(pos, 6); }
            aircraftControls.aircraft_controls[index].axis_min[pos, 0] = Int32.Parse(MinAxis1TextBox.Text);
            aircraftControls.aircraft_controls[index].axis_min[pos, 1] = Int32.Parse(MinAxis2TextBox.Text);
            aircraftControls.aircraft_controls[index].axis_min[pos, 2] = Int32.Parse(MinAxis3TextBox.Text);
            aircraftControls.aircraft_controls[index].axis_min[pos, 3] = Int32.Parse(MinAxis4TextBox.Text);
            aircraftControls.aircraft_controls[index].axis_min[pos, 4] = Int32.Parse(MinAxis5TextBox.Text);
            aircraftControls.aircraft_controls[index].axis_min[pos, 5] = Int32.Parse(MinAxis6TextBox.Text);
            aircraftControls.aircraft_controls[index].axis_max[pos, 0] = Int32.Parse(MaxAxis1TextBox.Text);
            aircraftControls.aircraft_controls[index].axis_max[pos, 1] = Int32.Parse(MaxAxis2TextBox.Text);
            aircraftControls.aircraft_controls[index].axis_max[pos, 2] = Int32.Parse(MaxAxis3TextBox.Text);
            aircraftControls.aircraft_controls[index].axis_max[pos, 3] = Int32.Parse(MaxAxis4TextBox.Text);
            aircraftControls.aircraft_controls[index].axis_max[pos, 4] = Int32.Parse(MaxAxis5TextBox.Text);
            aircraftControls.aircraft_controls[index].axis_max[pos, 5] = Int32.Parse(MaxAxis6TextBox.Text);
            aircraftControls.aircraft_controls[index].axis_inverted[pos, 0] = (bool)InvertedAxis1CheckBox.IsChecked;
            aircraftControls.aircraft_controls[index].axis_inverted[pos, 1] = (bool)InvertedAxis2CheckBox.IsChecked;
            aircraftControls.aircraft_controls[index].axis_inverted[pos, 2] = (bool)InvertedAxis3CheckBox.IsChecked;
            aircraftControls.aircraft_controls[index].axis_inverted[pos, 3] = (bool)InvertedAxis4CheckBox.IsChecked;
            aircraftControls.aircraft_controls[index].axis_inverted[pos, 4] = (bool)InvertedAxis5CheckBox.IsChecked;
            aircraftControls.aircraft_controls[index].axis_inverted[pos, 5] = (bool)InvertedAxis6CheckBox.IsChecked;
        }


        /// <summary>
        /// What to do when an aircraft is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ACnamesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = -2;
            int pos = -2;
            string selected_aircraft = "";
            string selected_controller = "";

            if (ACnamesComboBox.SelectedIndex > -1) { selected_aircraft = ACnamesComboBox.SelectedItem.ToString(); }
            if (ControllersComboBox.SelectedIndex > -1) { selected_controller = ControllersComboBox.SelectedItem.ToString(); }
            Debug.WriteLine(selected_aircraft);
            //First, we look if the controller is already in the aircraft_controls definition
            index = aircraftControls.aircraft_controls.FindIndex(c => c.controller_name == selected_controller);
            Debug.WriteLine($"Found controller? {index}");

            // if found, we check if the aircraft exists already for this controller
            if (index > -1) 
            {
                pos = Array.IndexOf(aircraftControls.aircraft_controls[index].aircraft_names, selected_aircraft);
                Debug.WriteLine($"Found aircraft? {index}");
                if (pos > -1) //yes, it is already defined
                {
                    Axis1EventsComboBox.SelectedItem = aircraftControls.aircraft_controls[index].axis_events[pos, 0];
                    Axis2EventsComboBox.SelectedItem = aircraftControls.aircraft_controls[index].axis_events[pos, 1];
                    Axis3EventsComboBox.SelectedItem = aircraftControls.aircraft_controls[index].axis_events[pos, 2];
                    Axis4EventsComboBox.SelectedItem = aircraftControls.aircraft_controls[index].axis_events[pos, 3];
                    Axis5EventsComboBox.SelectedItem = aircraftControls.aircraft_controls[index].axis_events[pos, 4];
                    Axis6EventsComboBox.SelectedItem = aircraftControls.aircraft_controls[index].axis_events[pos, 5];
                    MinAxis1TextBox.Text = aircraftControls.aircraft_controls[index].axis_min[pos, 0].ToString();
                    MinAxis2TextBox.Text = aircraftControls.aircraft_controls[index].axis_min[pos, 1].ToString();
                    MinAxis3TextBox.Text = aircraftControls.aircraft_controls[index].axis_min[pos, 2].ToString();
                    MinAxis4TextBox.Text = aircraftControls.aircraft_controls[index].axis_min[pos, 3].ToString();
                    MinAxis5TextBox.Text = aircraftControls.aircraft_controls[index].axis_min[pos, 4].ToString();
                    MinAxis6TextBox.Text = aircraftControls.aircraft_controls[index].axis_min[pos, 5].ToString();
                    MaxAxis1TextBox.Text = aircraftControls.aircraft_controls[index].axis_max[pos, 0].ToString();
                    MaxAxis2TextBox.Text = aircraftControls.aircraft_controls[index].axis_max[pos, 1].ToString();
                    MaxAxis3TextBox.Text = aircraftControls.aircraft_controls[index].axis_max[pos, 2].ToString();
                    MaxAxis4TextBox.Text = aircraftControls.aircraft_controls[index].axis_max[pos, 3].ToString();
                    MaxAxis5TextBox.Text = aircraftControls.aircraft_controls[index].axis_max[pos, 4].ToString();
                    MaxAxis6TextBox.Text = aircraftControls.aircraft_controls[index].axis_max[pos, 5].ToString();
                    InvertedAxis1CheckBox.IsChecked = aircraftControls.aircraft_controls[index].axis_inverted[pos, 0];
                    InvertedAxis2CheckBox.IsChecked = aircraftControls.aircraft_controls[index].axis_inverted[pos, 1];
                    InvertedAxis3CheckBox.IsChecked = aircraftControls.aircraft_controls[index].axis_inverted[pos, 2];
                    InvertedAxis4CheckBox.IsChecked = aircraftControls.aircraft_controls[index].axis_inverted[pos, 3];
                    InvertedAxis5CheckBox.IsChecked = aircraftControls.aircraft_controls[index].axis_inverted[pos, 4];
                    InvertedAxis6CheckBox.IsChecked = aircraftControls.aircraft_controls[index].axis_inverted[pos, 5];
                }
                else
                {
                    Axis1EventsComboBox.SelectedIndex = -1;
                    Axis2EventsComboBox.SelectedIndex = -1;
                    Axis3EventsComboBox.SelectedIndex = -1;
                    Axis4EventsComboBox.SelectedIndex = -1;
                    Axis5EventsComboBox.SelectedIndex = -1;
                    Axis6EventsComboBox.SelectedIndex = -1;
                    MinAxis1TextBox.Text = "0";
                    MinAxis2TextBox.Text = "0";
                    MinAxis3TextBox.Text = "0";
                    MinAxis4TextBox.Text = "0";
                    MinAxis5TextBox.Text = "0";
                    MinAxis6TextBox.Text = "0";
                    MaxAxis1TextBox.Text = "16383";
                    MaxAxis2TextBox.Text = "16383";
                    MaxAxis3TextBox.Text = "16383";
                    MaxAxis4TextBox.Text = "16383";
                    MaxAxis5TextBox.Text = "16383";
                    MaxAxis6TextBox.Text = "16383";
                    InvertedAxis1CheckBox.IsChecked = false;
                    InvertedAxis2CheckBox.IsChecked = false;
                    InvertedAxis3CheckBox.IsChecked = false;
                    InvertedAxis4CheckBox.IsChecked = false;
                    InvertedAxis5CheckBox.IsChecked = false;
                    InvertedAxis6CheckBox.IsChecked = false;
                }
            }
        }

        /// <summary>
        /// What to do if a different controller is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControllersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected_aircraft = ACnamesComboBox.SelectedItem.ToString(); 
            ACnamesComboBox.SelectedIndex = -1;
            Axis1EventsComboBox.SelectedIndex = -1;
            Axis2EventsComboBox.SelectedIndex = -1;
            Axis3EventsComboBox.SelectedIndex = -1;
            Axis4EventsComboBox.SelectedIndex = -1;
            Axis5EventsComboBox.SelectedIndex = -1;
            Axis6EventsComboBox.SelectedIndex = -1;
            MinAxis1TextBox.Text = "0";
            MinAxis2TextBox.Text = "0";
            MinAxis3TextBox.Text = "0";
            MinAxis4TextBox.Text = "0";
            MinAxis5TextBox.Text = "0";
            MinAxis6TextBox.Text = "0";
            MaxAxis1TextBox.Text = "16383";
            MaxAxis2TextBox.Text = "16383";
            MaxAxis3TextBox.Text = "16383";
            MaxAxis4TextBox.Text = "16383";
            MaxAxis5TextBox.Text = "16383";
            MaxAxis6TextBox.Text = "16383";
            InvertedAxis1CheckBox.IsChecked = false;
            InvertedAxis2CheckBox.IsChecked = false;
            InvertedAxis3CheckBox.IsChecked = false;
            InvertedAxis4CheckBox.IsChecked = false;
            InvertedAxis5CheckBox.IsChecked = false;
            InvertedAxis6CheckBox.IsChecked = false;

            string selected_item = ControllersComboBox.SelectedItem.ToString();
            if (selected_item != "")
            {
                Debug.WriteLine(selected_item);
                //Check if controller exists in available_controllers;
                int pos = Array.IndexOf(available_controllers, selected_item);
                if (pos > -1)
                {
                    ControllerConnectedLabel.Visibility = Visibility.Visible;
                    ControllerConnectedLabel.Content = "(press button to identify)";
                }
                else
                {
                    ControllerConnectedLabel.Visibility = Visibility.Visible;
                    ControllerConnectedLabel.Content = "controller is not connected";
                }
                if (selected_aircraft != "") { ACnamesComboBox.SelectedItem = selected_aircraft; }
            }
            else
            {
                ControllerConnectedLabel.Visibility = Visibility.Hidden;
            }

        }

        /// <summary>
        /// What to do if new aircraft name is entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewAircraftName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var selected_item = ACnamesComboBox.SelectedItem;
                aircraftControls.aircraft.Add(NewAircraftName.Text);
                aircraftControls.aircraft.Sort();
                ACnamesComboBox.Items.Clear();
                for (int j = 0; j < aircraftControls.aircraft.Count; j++)
                {
                    ACnamesComboBox.Items.Add(aircraftControls.aircraft[j]);
                }
                ACnamesComboBox.SelectedItem = selected_item;
                NewAircraftName.Text = "";
            }
        }


        /// <summary>
        /// What to do if new event name is entered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewEventName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var selectedAxis1 = Axis1EventsComboBox.SelectedItem;
                var selectedAxis2 = Axis2EventsComboBox.SelectedItem;
                var selectedAxis3 = Axis3EventsComboBox.SelectedItem;
                var selectedAxis4 = Axis4EventsComboBox.SelectedItem;
                var selectedAxis5 = Axis5EventsComboBox.SelectedItem;
                var selectedAxis6 = Axis6EventsComboBox.SelectedItem;

                aircraftControls.all_events.Add(NewEventName.Text);
                aircraftControls.all_events.Sort();
                Axis1EventsComboBox.Items.Clear();
                Axis2EventsComboBox.Items.Clear();
                Axis3EventsComboBox.Items.Clear();
                Axis4EventsComboBox.Items.Clear();
                Axis5EventsComboBox.Items.Clear();
                Axis6EventsComboBox.Items.Clear();
                for (int j = 0; j < aircraftControls.all_events.Count; j++)
                {
                    Axis1EventsComboBox.Items.Add(aircraftControls.all_events[j]);
                    Axis2EventsComboBox.Items.Add(aircraftControls.all_events[j]);
                    Axis3EventsComboBox.Items.Add(aircraftControls.all_events[j]);
                    Axis4EventsComboBox.Items.Add(aircraftControls.all_events[j]);
                    Axis5EventsComboBox.Items.Add(aircraftControls.all_events[j]);
                    Axis6EventsComboBox.Items.Add(aircraftControls.all_events[j]);
                }
                NewEventName.Text = "";
                Axis1EventsComboBox.SelectedItem = selectedAxis1;
                Axis2EventsComboBox.SelectedItem = selectedAxis2;
                Axis3EventsComboBox.SelectedItem = selectedAxis3;
                Axis4EventsComboBox.SelectedItem = selectedAxis4;
                Axis5EventsComboBox.SelectedItem = selectedAxis5;
                Axis6EventsComboBox.SelectedItem = selectedAxis6;

            }
        }

        private void Add_button_event_button_Click(object sender, RoutedEventArgs e)
        {
            if ((ACnamesComboBox.SelectedIndex != -1) && (ControllersComboBox.SelectedIndex != -1))
            {
                string selected_aircraft = ACnamesComboBox.SelectedItem.ToString();
                string selected_controller = ControllersComboBox.SelectedItem.ToString();
                int controller_id = Array.FindIndex(available_controllers, val => val.Equals(selected_controller));
                AddButtonEventWindow addButtonEventWindow = new AddButtonEventWindow(aircraftControls, mygamecontroller, selected_aircraft, selected_controller, controller_id, mysimconnect);
                addButtonEventWindow.Closed += AddButtonEventWindow_Closed;
                addButtonEventWindow.Show();
            }
            else
            {
                MessageBox.Show("Please select controller and aircraft first.");
            }
            
        }

        private void AddButtonEventWindow_Closed(object sender, EventArgs e)
        {
            aircraftControls = aircraftControls.LoadControlsFile(aircraftControls);
            Debug.WriteLine("AddButtonWindow closed");

        }

        private void Add_2nd_axis_event_button_Click(object sender, RoutedEventArgs e)
        {
            if ((ACnamesComboBox.SelectedIndex != -1) && (ControllersComboBox.SelectedIndex != -1))
            {
                string selected_aircraft = ACnamesComboBox.SelectedItem.ToString();
                string selected_controller = ControllersComboBox.SelectedItem.ToString();
                Debug.WriteLine($"{selected_aircraft} and {selected_controller}");
                int controller_id = aircraftControls.defined_controllers.IndexOf(selected_controller);
                int pos = -1;
                Debug.WriteLine(controller_id);
                if (controller_id > -1)
                {
                    try
                    {
                        pos = Array.IndexOf(aircraftControls.aircraft_controls[controller_id].aircraft_names, selected_aircraft);
                    }
                    catch { }
                }
                Debug.WriteLine($"{aircraftControls.aircraft_controls[controller_id].aircraft_names.Length} en {controller_id} en {pos}");
                if (pos > -1)
                {
                    Add2ndAxisEventsWindow add2ndAxisEventsWindow = new Add2ndAxisEventsWindow(aircraftControls, mygamecontroller, selected_aircraft, selected_controller, controller_id);
                    add2ndAxisEventsWindow.Closed += Add2ndAxisEventsWindow_Closed;
                    add2ndAxisEventsWindow.Show();
                }
                else
                {
                    MessageBox.Show("Please save primary controller,aircraft and axis definitions first.");
                }
            }
            else
            {
                MessageBox.Show("Please select controller and aircraft first.");
            }
        }

        private void Add2ndAxisEventsWindow_Closed(object sender, EventArgs e)
        {
            aircraftControls = aircraftControls.LoadControlsFile(aircraftControls);
            Debug.WriteLine("Add2ndAxisEventsWindow closed");

        }

        private void AddAxis46CheckBoxChanged(object sender, EventArgs e)
        {
            if (AddAxis46CheckBox.IsChecked == true)
            {
                this.Height = 550;
                Axis45labels.Visibility = Visibility.Visible;
                StackPanel46Axis.Visibility = Visibility.Visible;
            }
            else
            {
                this.Height = 450;
                Axis45labels.Visibility = Visibility.Hidden;
                StackPanel46Axis.Visibility = Visibility.Hidden;
            }
        }

        private void MIDIMapperWindow_Click(object sender, RoutedEventArgs e)
        {
            MIDIMapWindow midiMapWindow = new MIDIMapWindow(str_selected_aircraft);
            midiMapWindow.Show();
        }
    }
}
