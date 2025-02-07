﻿using System;
using System.Configuration;
using System.Collections.Specialized;
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
using System.Threading;
using System.Configuration;
using System.Diagnostics;


namespace EasyControlforMSFS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static readonly MSFSVarServices myMSFSVarServices = new MSFSVarServices();
        public MQTTclient myMQTTclient;
        public GameControllerReader mygamecontroller;
        public string[] available_controllers = new string[10];
        public AircraftControls aircraftControls;
        public ProfilesMap profilesMap;
        public MIDIcontroller myMIDIcontroller;
        public static int max_axis = 10;
        public static int max_buttons = 164;
        public double[,] axisArray = new double[10, max_axis]; //max 10 controllers with 10 axes each
        public bool[,] buttonArray = new bool[10, max_buttons]; //max 10 controllers with 30 buttons each
        public SimConnectImplementer mysimconnect; 
        public bool addDefWindowOpened = false;
        public string[] args;
        public string title;
        string sAttr;
        bool MIDIused = false;
        bool MIDIconnected = false;

        public MainWindow()
        {
            mygamecontroller = new GameControllerReader();
            args = Environment.GetCommandLineArgs();
            title = "";

            profilesMap = new ProfilesMap();
            profilesMap = profilesMap.LoadProfilesMapFile(profilesMap);

            InitializeComponent();


            myMSFSVarServices.LogResult += OnAddResult;
            myMSFSVarServices.InitMSFSServices();

            
            
            aircraftControls = new AircraftControls();
            aircraftControls = aircraftControls.LoadControlsFile(aircraftControls);
            if (!aircraftControls.XMLImportSuccess)
            {
                Debug.WriteLine($"ERROR IN XML CONTROLS FILE");
                MessageTextBox.AppendText($"POTENTIAL ERROR IN XML CONTROLS FILE \n Check syntax of controls.xml \r\n");
                MessageTextBox.ScrollToEnd();

            }



            //Now we add aircraft to the aircraft selector combobox
            aircraftControls.aircraft.Sort();
            for (int j = 0; j < aircraftControls.aircraft.Count; j++)
            {
                SelectAircraftComboBox.Items.Add(aircraftControls.aircraft[j]);
            }

            //If an aircraft has been given as input to the programme
            if (args.Length > 1)
            { 
                MessageTextBox.Text += "Preselected aircraft: " + args[1] + "\r\n"; 
                SelectAircraftComboBox.SelectedItem = args[1]; 
            }
            

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Start threads for controller overview and inputs
            Thread readControllers = new Thread(ReadControllers);
            readControllers.IsBackground = true;
            readControllers.Start();

            Thread readControllersState = new Thread(ReadControllersState);
            readControllersState.IsBackground = true;
            readControllersState.Start();

            Thread connectSimConnect = new Thread(ConnectSimConnect);
            connectSimConnect.IsBackground = true;
            connectSimConnect.Start();

            //Thread connectMQTT = new Thread(ConnectMQTT);
            //connectMQTT.IsBackground = true;
            //connectMQTT.Start();

            sAttr = ConfigurationManager.AppSettings.Get("MIDI");
            MIDIused = System.Convert.ToBoolean(sAttr);
            Debug.WriteLine($"MIDI key is set to: {sAttr} {MIDIused}");
            MessageTextBox.AppendText($"MIDI key is set to: {MIDIused} \r\n");
            MessageTextBox.ScrollToEnd();

            if (MIDIused)
            {
                Thread connectMIDI = new Thread(ConnectMIDI);
                connectMIDI.IsBackground = true;
                connectMIDI.Start();
            }

        }

        public void ConnectMQTT()
        {
            myMQTTclient = new MQTTclient();
            myMQTTclient.LogResult += OnAddResult;
        }


        public void ConnectMIDI()
        {
            while (true)
            {
                if (mysimconnect != null)
                {
                    if (mysimconnect.bSimConnected)
                    {
                        myMIDIcontroller = new MIDIcontroller(mysimconnect);
                        if (!myMIDIcontroller.XMLloadedsuccessfully)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                MessageTextBox.AppendText("MIDI DEVICE NOT CONNECTED OR ERROR IN LOADING MIDI XML FILE! \n Check connection or MIDI XML file syntax" + "\r\n");
                                MessageTextBox.ScrollToEnd();
                            });
                        }
                        myMIDIcontroller.LogResult += OnAddResult;
                        MIDIconnected = myMIDIcontroller.MIDIconnected;
                        break;
                    }
                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        MessageTextBox.AppendText("MIDI connector waiting for Simconnect connection" + "\r\n");
                        MessageTextBox.ScrollToEnd();
                    });
                    Thread.Sleep(1000);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!addDefWindowOpened)
            {
                string SelectedAircraftString = "";
                if (SelectAircraftComboBox.SelectedIndex > -1) { SelectedAircraftString = SelectAircraftComboBox.SelectedItem.ToString(); }
                AddDefinitionWindow addDefWindow = new AddDefinitionWindow(aircraftControls, mygamecontroller, mysimconnect, SelectedAircraftString );
                addDefWindow.Closed += AddDefWindow_Closed;
                addDefWindow.Show();
                addDefWindowOpened = true;
            }
        }

        private void AddDefWindow_Closed(object sender, EventArgs e)
        {
            aircraftControls = aircraftControls.LoadControlsFile(aircraftControls);

            //Now we again add aircraft to the aircraft selector combobox
            var selected_item = SelectAircraftComboBox.SelectedItem;
            SelectAircraftComboBox.Items.Clear();
            aircraftControls.aircraft.Sort();
            for (int j = 0; j < aircraftControls.aircraft.Count; j++)
            {
                SelectAircraftComboBox.Items.Add(aircraftControls.aircraft[j]);
            }
            SelectAircraftComboBox.SelectedItem = selected_item;
            addDefWindowOpened = false;

            //Reload MIDI controls
            if (myMIDIcontroller != null)
            {
                myMIDIcontroller.LoadControlsFile();
            }
        }

        public void ConnectSimConnect()
        {
            mysimconnect = new SimConnectImplementer();
            mysimconnect.LogResult += OnAddResult;

            //Debug.WriteLine($"Simconnect started");
            bool localbSimConnected = false;
            bool title_registred = false;
            while (true)
            {
                //Debug.Write($"Start loop");
                Thread.Sleep(1000);
                localbSimConnected = mysimconnect.bSimConnected;
                if (localbSimConnected == false)
                {
                    //Debug.WriteLine($"Start inner if ");
                    Thread.Sleep(1000);
                    this.Dispatcher.Invoke(() =>
                    {
                        //Debug.WriteLine($"Just before simconnect call");
                        mysimconnect.Connect();
                        Thread.Sleep(200);
                        localbSimConnected = mysimconnect.bSimConnected;
                        Debug.WriteLine($"Simconnect status loop: {localbSimConnected}");
                        if (localbSimConnected)
                        {
                            SimconnectStatusEllipse.Fill = Brushes.Green;
                            //int i = mysimconnect.RegisterSimVar("TITLE", "unit", "string", "NOT_USED");
                            //title_registred = true;
                            //Debug.WriteLine($"Title registred {i}");
                        
                        }
                        else
                        {
                            SimconnectStatusEllipse.Fill = Brushes.Red;
                            MessageTextBox.Text += "Looking for simulator...\r\n";
                            Thread.Sleep(200);
                        }
                    });
                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        SimconnectStatusEllipse.Fill = Brushes.Green;
                    }); 
                    if (!title_registred) 
                    {
                        int i = mysimconnect.RegisterSimVar("Title", "unit", "string", "NOT_USED");
                        title_registred = true;
                        Debug.WriteLine($"Title registred {i}");
                    }
                    
                }
            }
        }

        private void OnAddResult(object sender, string sResult)
        {
            if (sResult.Contains("AUTOPILOT ") || sResult.Contains("LIGHT ") || sResult.Contains("PLANE HEADING DEGREES ")) { myMIDIcontroller.NewSimConnectData(sResult); }
            
            this.Dispatcher.Invoke(() =>
            {
                if (!sResult.Contains("|"))
                {
                    MessageTextBox.AppendText(sResult + "\r\n");
                    MessageTextBox.ScrollToEnd();
                }
                //Debug.WriteLine(sResult);

            });

            if (sResult.Contains("Title"))
            {
                title = sResult.Split("|")[1];
                title = title.Replace("&", "");
                TitleTextBlock.Text = title;
                CheckTitleReceived(title);
            }
        }

        private void CheckTitleReceived(string title)
        {
            int title_id = -1;
            int profile_id = -1;
            for (int i = 0; i < profilesMap.profiles_map.Count; i++)
            {
                title_id = Array.IndexOf(profilesMap.profiles_map[i].titles, title);
                if (title_id > -1)
                {
                    profile_id = i;
                    Debug.WriteLine($"Title index: {title_id} {title}");
                }
            }
            if (profile_id > -1) //title is already mapped to profile
            {
                SelectAircraftComboBox.SelectedItem = profilesMap.profiles_map[profile_id].profile_name;
                if (MIDIused)
                {
                    myMIDIcontroller.SetTitle(profilesMap.profiles_map[profile_id].profile_name);
                }
            }
            else
            {
                SelectAircraftComboBox.SelectedIndex = -1;
                MapTitleButton.Visibility = Visibility.Visible;
                MessageBox.Show("Title has not yet been mapped to a profile. Select a profile, and click map button to automatically load correct profile for this title next time.");
            }
        }


        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            mysimconnect.Disconnect();
            if (MIDIused && MIDIconnected) { myMIDIcontroller.StopMIDIconnection(); }
            Environment.Exit(0);
            System.Windows.Application.Current.Shutdown();
        }



        private void SelectAircraftComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Checking if selected aircraft has any controls defined for all available controllers
            string selected_aircraft = "";
            if (SelectAircraftComboBox.SelectedIndex > -1)
            {
                selected_aircraft = SelectAircraftComboBox.SelectedItem.ToString();
                if (MIDIused)
                {
                    if (MIDIconnected) { myMIDIcontroller.SetTitle(selected_aircraft); }
                    
                }
            }

            Debug.WriteLine(available_controllers[0]);
            for (int i = 0; i < available_controllers.Length; i++)
            {
                string controller = available_controllers[i];
                for (int j = 0; j < aircraftControls.aircraft_controls.Count; j++)
                {
                    if (aircraftControls.aircraft_controls[j].controller_name == available_controllers[i])
                    {
                        if ((selected_aircraft != "") && (!aircraftControls.aircraft_controls[j].aircraft_names.Contains(selected_aircraft)))
                        {
                            //Debug.WriteLine(aircraftControls.aircraft_controls[j].aircraft_names[0]);
                            MessageTextBox.Text += $"{selected_aircraft} has no events for controller {controller} \n";
                        }
                        else { }
                    }
                }
            }
        }

        /// <summary>
        /// Thread to read controllers connected
        /// </summary>
        public void ReadControllers()
        {
            available_controllers = mygamecontroller.ReadAvailableGameControllers();
            while (available_controllers[0] == "Controllers not loaded yet")
            {
                available_controllers = mygamecontroller.ReadAvailableGameControllers();
                //Debug.WriteLine($"Controller gevonden: {available_controllers[0]}");
                Thread.Sleep(1000);
            }
            for (int i = 0; i < available_controllers.Length; i++)
            {
                mygamecontroller.StartReadingController(available_controllers[i]);
                Debug.WriteLine($"MainWindow: Reading controller {available_controllers[i]} started");
                Thread.Sleep(300); // if controllers are started to quickly, they overlap and are not initiated properly
            }
        }

        /// <summary>
        /// Thread to read selected controllers buttons and indicate when pressed
        /// </summary>
        public void ReadControllersState()
        {
            int aircraft_selected = -1;
            string str_aircraft_selected = "";
            while (true)
            {
                this.Dispatcher.Invoke(() =>
                {
                    aircraft_selected = SelectAircraftComboBox.SelectedIndex;
                    if (aircraft_selected != -1) { str_aircraft_selected = SelectAircraftComboBox.SelectedItem.ToString(); }
                });
                Thread.Sleep(30);
                if ((aircraft_selected != -1) && (mysimconnect.bSimConnected) && (addDefWindowOpened==false))
                {
                    //First we updated all the axis and values
                    //We loop through all available controllers 
                    for (int i = 0; i < available_controllers.Length; i++)
                    {
                        //We loop through all axis
                        for (int j = 0; j < max_axis; j++)
                        {
                            //We sync the local axisArray with readings from my gameController --> CAN POTENTIALLY BE REMOVED!
                            axisArray[i, j] = mygamecontroller.axisArraySmooth[i, j];
                            // We loop through all aircraft control definitions
                            for (int k = 0; k < aircraftControls.aircraft_controls.Count; k++)
                            {
                                // If an aicraft control defintions controllers name matches the available controller we are checking..
                                if (aircraftControls.aircraft_controls[k].controller_name == available_controllers[i])
                                {
                                    // We search for the selected aircraft
                                    int aircraft_id = Array.IndexOf(aircraftControls.aircraft_controls[k].aircraft_names, str_aircraft_selected);
                                    //Debug.WriteLine(aircraft_id);//If the current axis is within the axis defined...
                                    if (aircraft_id > -1)
                                    {
                                        if (j < aircraftControls.aircraft_controls[k].num_axis[aircraft_id])
                                        {
                                            if ((str_aircraft_selected == "Airbus A320neo") && (j == 0))
                                            {
                                                if (axisArray[i, j] > 0) //Special case for spoiler A320
                                                {
                                                    Debug.WriteLine($"Event not skipped for A320 neo {axisArray[i, j]}");
                                                    ExecuteAxisSimEvent(i, j, k, aircraft_id); 
                                                }
                                            }
                                            else
                                            {
                                                ExecuteAxisSimEvent(i, j, k, aircraft_id);
                                                //Debug.WriteLine($"{i},{j},{k},{l}");
                                            }

                                        }
                                    }

                                }
                            
                            }
                        }

                        // now we loop through the buttons, following the same logic
                        for (int j = 0; j < max_buttons; j++)
                        {
                            buttonArray[i, j] = false;
                            buttonArray[i, j] = mygamecontroller.buttonArray[i, j];
                            //if (buttonArray[i,j] == true) { Debug.WriteLine($"Controller {available_controllers[i]}, Button {j + 1} pressed"); }
                            {
                                for (int k = 0; k < aircraftControls.aircraft_controls.Count; k++)
                                {
                                    if (aircraftControls.aircraft_controls[k].controller_name == available_controllers[i])
                                    {
                                        int aircraft_id = Array.IndexOf(aircraftControls.aircraft_controls[k].aircraft_names, str_aircraft_selected);
                                        if ((aircraft_id > -1) && (j < aircraftControls.aircraft_controls[k].num_buttons[aircraft_id]))
                                        {
                                            if (aircraftControls.aircraft_controls[k].button_is_switch[aircraft_id, j] == true)
                                            {
                                                //Deal with switches
                                                //First case switch is off
                                                if (aircraftControls.aircraft_controls[k].switch_on[aircraft_id, j] == false)
                                                {
                                                    //If switch is set to on, we fire event and record switch as on
                                                    if (buttonArray[i, j] == true)
                                                    {
                                                        ExecuteButtonSimEvent(i, j, k, aircraft_id, true); //execute button event with state true (turn event on)
                                                        aircraftControls.aircraft_controls[k].switch_on[aircraft_id, j] = true;
                                                        Debug.WriteLine("EVENT ON SEND");
                                                    }
                                                }
                                                else // Case switch is on
                                                {
                                                    if (buttonArray[i, j] == false) //Switch no longer on 
                                                    {
                                                        ExecuteButtonSimEvent(i, j, k, aircraft_id, false); //execute button event with state true (turn event off)
                                                        aircraftControls.aircraft_controls[k].switch_on[aircraft_id, j] = false;
                                                        Debug.WriteLine("EVENT OFF SEND");
                                                    }
                                                }
                                            }
                                        
                                            else
                                            {
                                                if ((buttonArray[i, j] == true) && (aircraftControls.aircraft_controls[k].button_events[aircraft_id, j] != ""))
                                                {
                                                    ExecuteButtonSimEvent(i, j, k, aircraft_id, true); //execute button event with state true (turn event on)
                                                    Debug.WriteLine($"EVENT NOT SWITCH SEND event: {i} {j} {k} {aircraftControls.aircraft_controls[k].button_events[aircraft_id, j]}");
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }


        public void ExecuteAxisSimEvent(int controller, int axis, int defined_controller, int aircraft)
        {
            int i = controller;
            int j = axis;
            int k = defined_controller;
            int l = aircraft;
            bool axis_surpressed= false;
            int set_value2 = 0;

            string sim_event = aircraftControls.aircraft_controls[k].axis_events[l, j];
            string sim_event2 = aircraftControls.aircraft_controls[k].axis_events2[l, j];

            double set_value_double = 0;
            double set_value_double2 = 0;
            if (!aircraftControls.aircraft_controls[k].axis_inverted[l, j])
            {
                set_value_double = aircraftControls.aircraft_controls[k].axis_min[l, j] + (axisArray[i, j]) * ((aircraftControls.aircraft_controls[k].axis_max[l, j] - aircraftControls.aircraft_controls[k].axis_min[l, j]));
            }
            else
            {
                set_value_double = aircraftControls.aircraft_controls[k].axis_max[l, j] - (axisArray[i, j]) * ((aircraftControls.aircraft_controls[k].axis_max[l, j] - aircraftControls.aircraft_controls[k].axis_min[l, j]));
            }
            int set_value = (int)set_value_double;

            // If 2nd event linked to axis
            if (sim_event2 != "")
            {
                if (!aircraftControls.aircraft_controls[k].axis_inverted2[l, j])
                {
                    set_value_double2 = aircraftControls.aircraft_controls[k].axis_min2[l, j] + (axisArray[i, j]) * ((aircraftControls.aircraft_controls[k].axis_max2[l, j] - aircraftControls.aircraft_controls[k].axis_min2[l, j]));
                }
                else
                {
                    set_value_double2 = aircraftControls.aircraft_controls[k].axis_max2[l, j] - (axisArray[i, j]) * ((aircraftControls.aircraft_controls[k].axis_max2[l, j] - aircraftControls.aircraft_controls[k].axis_min2[l, j]));
                }
                set_value2 = (int)set_value_double2;
            }

            //Debug.Write($"EVENT found for aircraft_control {k}, aircraft{l} with name {aircraftControls.aircraft_controls[k].aircraft_names[l]} and axis {j} with value {axisArray[i, j]} and set value: {set_value}"); Debug.WriteLine(sim_event);
            
            // Check if axis linked to detent button
            for (int m = 0; m < aircraftControls.aircraft_controls[k].num_buttons[l]; m++)
            {
                if (aircraftControls.aircraft_controls[k].button_axis_link[l, m] == j)
                {
                    if (buttonArray[i, m] == true) { axis_surpressed = true; Debug.WriteLine($"Axis supressed due to button {m} for axis {j}"); }
                }
            }
            if (!axis_surpressed) //fire events if axis is not surpressed
            {
                if (sim_event != "")
                {
                    if (sim_event.Length > 6)
                    {
                        if (sim_event.Substring(0, 6) == "FSUIPC")
                        {
                            string sim_event_new = sim_event.Replace("FSUIPC.", "");
                            //int current_value = (int)MainWindow.myMSFSVarServices.VS_GetLvarValue(sim_event_new);
                            //double check = Math.Abs(current_value - Math.Round(set_value_double));
                            //Debug.WriteLine($"Set value {sim_event}  {set_value_double}");
                            //if (Math.Abs(current_value - Math.Round(set_value_double)) > 1) {

                            //SPECIFIC FOR FSS
                            if (sim_event.Contains("FSS_EXX_EVT_LEVER_THROTTLE"))
                            {
                                set_value_double = set_value_double / 100;
                            }


                            myMSFSVarServices.VS_EventSet(sim_event_new, set_value_double); Thread.Sleep(10);


                            //SPECIFIC FOR FSS
                            if (sim_event.Contains("FSS_EXX_EVT_LEVER_THROTTLE_L"))
                            {
                                myMSFSVarServices.VS_EventSet("FSS_EXX_THR_HW_INPUT_L_ACTIVE", 1);
                            }
                            if (sim_event.Contains("FSS_EXX_EVT_LEVER_THROTTLE_R"))
                            {
                                myMSFSVarServices.VS_EventSet("FSS_EXX_THR_HW_INPUT_R_ACTIVE", 1);
                            }

                        }
                    else {
                            if (sim_event.Contains("AXIS_SPOILER_SET") && (set_value < -17999))
                            {
                                Debug.WriteLine("axis spoiler skipped, too low");
                            }
                            else
                            {
                                mysimconnect.SendEvent(sim_event, set_value); Debug.WriteLine($"Set value axis {j}:  {sim_event}  {set_value}");
                            }
                        }
                    }
                }
                
                if (sim_event2 != "") 
                {
                    if (sim_event2.Substring(0, 6) == "FSUIPC")
                    {
                        string sim_event_new2 = sim_event2.Replace("FSUIPC.", "");
                        int current_value = (int)MainWindow.myMSFSVarServices.VS_GetLvarValue(sim_event_new2);
                        if (Math.Abs(current_value - Math.Round(set_value_double2)) > 1) { myMSFSVarServices.VS_EventSet(sim_event_new2, set_value_double2); Thread.Sleep(10); }
                    }
                    else
                    { mysimconnect.SendEvent(sim_event2, set_value2); }
                }
            }
            else { Debug.WriteLine("Axis supressed, no event sent!"); }
        }

        public void ExecuteButtonSimEvent(int controller, int button, int defined_controller, int aircraft, bool set_value)
        {
            int i = controller;
            int j = button;
            int k = defined_controller;
            int l = aircraft;
            bool turnon = set_value; //true = turn event on 

            string sim_event = aircraftControls.aircraft_controls[k].button_events[l, j];
            string sim_eventWAIT = aircraftControls.aircraft_controls[k].button_eventsWAIT[l, j];
            if (sim_event != "")
            {
                if (turnon)
                {
                    if (sim_event.Substring(0, 6) == "FSUIPC")
                    {
                        string sim_event_new = sim_event.Replace("FSUIPC.", "");
                        myMSFSVarServices.VS_EventSet(sim_event_new, 1);
                        Debug.WriteLine($"Button {j} event ON {sim_event} sent!");
                    }
                    else 
                    {
                        if (!sim_event.Contains("ONZIN"))
                        {
                            mysimconnect.SendEvent(sim_event, 1);
                            Debug.WriteLine($"Button {j} event ON {sim_event} sent!");
                        }
                        else
                        {
                            Debug.WriteLine($"Button {j} event ON SKIPPED {sim_event}!");
                        }
                    }
                    if (!sim_event.Contains("ONZIN"))
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            MessageTextBox.AppendText($"Button {j} event ON {sim_event} sent!\r\n");
                            MessageTextBox.ScrollToEnd();
                        });
                    }
                    Thread.Sleep(200);
                    if (sim_eventWAIT != null)
                    {
                        Debug.WriteLine("WAIT event detected");
                        sim_event = sim_eventWAIT;
                        Thread.Sleep(300);
                        if (sim_event.Substring(0, 6) == "FSUIPC")
                        {
                            string sim_event_new = sim_event.Replace("FSUIPC.", "");
                            myMSFSVarServices.VS_EventSet(sim_event_new, 1);
                        }
                        else { mysimconnect.SendEvent(sim_event, 1); }
                        Debug.WriteLine($"Button {j} event ON {sim_event} sent!");
                        this.Dispatcher.Invoke(() =>
                        {
                            MessageTextBox.AppendText($"Button {j} event ON {sim_event} sent!\r\n");
                            MessageTextBox.ScrollToEnd();
                        });
                    }

                }
                else
                {
                    if (aircraftControls.aircraft_controls[k].button_eventsOFF[l, j] != "")
                    {
                        sim_event = aircraftControls.aircraft_controls[k].button_eventsOFF[l, j];
                    }
                    if (sim_event.Substring(0, 6) == "FSUIPC")
                    {
                        string sim_event_new = sim_event.Replace("FSUIPC.", "");
                        myMSFSVarServices.VS_EventSet(sim_event_new, 0);
                    }
                    else 
                    { mysimconnect.SendEvent(sim_event, 0); }
                    Debug.WriteLine($"Button {j} event OFF {sim_event} sent!");
                    this.Dispatcher.Invoke(() =>
                    {
                        MessageTextBox.AppendText($"Button {j} event OFF {sim_event} sent!\r\n");
                        MessageTextBox.ScrollToEnd();
                    });
                    Thread.Sleep(200);
                    if (sim_eventWAIT != null)
                    {
                        Debug.WriteLine("WAIT event detected");
                        sim_event = sim_eventWAIT;
                        Thread.Sleep(300);
                        if (sim_event.Substring(0, 6) == "FSUIPC")
                        {
                            string sim_event_new = sim_event.Replace("FSUIPC.", "");
                            myMSFSVarServices.VS_EventSet(sim_event_new, 0);
                        }
                        else { mysimconnect.SendEvent(sim_event, 0); }
                        Debug.WriteLine($"Button {j} event ON {sim_event} sent!");
                        this.Dispatcher.Invoke(() =>
                        {
                            MessageTextBox.AppendText($"Button {j} event ON {sim_event} sent!\r\n");
                            MessageTextBox.ScrollToEnd();
                        });
                        
                    }
                }
            }
        }

        private void MapTitleButton_Click(object sender, RoutedEventArgs e)
        {
            string profile_selected = "";
            if (SelectAircraftComboBox.SelectedIndex > -1)
            {
                profile_selected = SelectAircraftComboBox.SelectedItem.ToString();
            }
            if (profile_selected == "")
            {
                MessageBox.Show("Please select profile first.");
            }
            bool new_profile = true;
            for (int i = 0; i < profilesMap.profiles_map.Count; i++)
            { 
                if (profilesMap.profiles_map[i].profile_name == profile_selected)
                {
                    new_profile = false;   
                    Debug.WriteLine($"Aantal titles: {profilesMap.profiles_map[i].nr_titles}");
                    profilesMap.profiles_map[i].AddTitle(profilesMap.profiles_map[i].nr_titles,title);
                    profilesMap.profiles_map[i].nr_titles += 1;    
                }
            }
            if (new_profile == true)
            {
                Debug.WriteLine($"Title added: {title}");
                profilesMap.profiles_map.Add(new ProfilesMap.ProfilesMapData() { profile_name = profile_selected });
                profilesMap.profiles_map[profilesMap.profiles_map.Count - 1].AddTitle(0, title);
                profilesMap.profiles_map[profilesMap.profiles_map.Count - 1].nr_titles += 1;
            }
            profilesMap.SaveXML(profilesMap);
            MapTitleButton.Visibility = Visibility.Hidden;

        }


        private void FSUIPCReload_Click(object sender, RoutedEventArgs e)
        {
            myMSFSVarServices.Reload();
        }

        private void MidiReload_Click(object sender, RoutedEventArgs e)
        {
            myMIDIcontroller.RestartMIDIconnection();  
            
        }
    }
}

