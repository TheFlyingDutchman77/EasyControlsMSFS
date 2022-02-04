using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Threading;

namespace EasyControlforMSFS
{
    /// <summary>
    /// Interaction logic for AddButtonEvent.xaml
    /// </summary>
    public partial class AddButtonEventWindow : Window
    {
        public GameControllerReader mygamecontroller;
        public AircraftControls aircraftControls;
        public SimConnectImplementer mysimconnect;
        public string selected_aircraft;
        public string selected_controller;
        public int controller_id;
        int index;
        int pos;
        public bool[] buttonArray = new bool[160];

        public AddButtonEventWindow(AircraftControls aircraftControlsInput, GameControllerReader myGameControllerInput, string selected_aircraftInput, string selected_controllerInput, int controller_idInput, SimConnectImplementer mysimconnectInput)
        {
            InitializeComponent();
            aircraftControls = aircraftControlsInput;
            selected_controller = selected_controllerInput;
            selected_aircraft = selected_aircraftInput;
            controller_id = controller_idInput;
            mygamecontroller = myGameControllerInput;
            mysimconnect = mysimconnectInput;
            index = aircraftControls.aircraft_controls.FindIndex(c => c.controller_name == selected_controller);

            if (index > -1)
            {
                pos = Array.IndexOf(aircraftControls.aircraft_controls[index].aircraft_names, selected_aircraft);
            }

            //Now we add the elements to the comboboxes
            for (int i = 1; i < 129 ; i++)
            {
                ButtonNrComboBox.Items.Add("Button " + i);
            }
            aircraftControls.all_events.Sort();
            for (int j = 0; j < aircraftControls.all_events.Count; j++)
            {
                ButtonEventsComboBox.Items.Add(aircraftControls.all_events[j]);
            }
            if (index > -1)
            {
                int pos = Array.IndexOf(aircraftControls.aircraft_controls[index].aircraft_names, selected_aircraft);
                if (pos > -1)
                {
                    int nr_axes = aircraftControls.aircraft_controls[index].num_axis[pos];
                    for (int j = 0; j <= nr_axes; j++)
                    {
                        if (j == 0) { ButtonAxisLinkComboBox.Items.Add("None"); }
                        else { ButtonAxisLinkComboBox.Items.Add("Axis " + j); }
                    }
                }
            }

            Thread readControllersState = new Thread(ReadControllersState);
            readControllersState.IsBackground = true;
            readControllersState.Start();

        }

        private void ButtonNrComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((index > -1) && (pos > -1))
            {
                if (ButtonNrComboBox.SelectedIndex <= aircraftControls.aircraft_controls[index].num_buttons[pos])
                {
                    ButtonEventsComboBox.SelectedItem = aircraftControls.aircraft_controls[index].button_events[pos, ButtonNrComboBox.SelectedIndex];
                    ButtonAxisLinkComboBox.SelectedIndex = aircraftControls.aircraft_controls[index].button_axis_link[pos, ButtonNrComboBox.SelectedIndex];
                    ButtonIsSwitchCheckbox.IsChecked = aircraftControls.aircraft_controls[index].button_is_switch[pos, ButtonNrComboBox.SelectedIndex];

                    Debug.Write(ButtonNrComboBox.SelectedIndex);
                    Debug.WriteLine(aircraftControls.aircraft_controls[index].button_axis_link[pos, ButtonNrComboBox.SelectedIndex]);
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
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

        private void SetInputValuestoList(int index, int pos)
        {
            if (ButtonEventsComboBox.SelectedIndex != -1) 
            { 
                aircraftControls.aircraft_controls[index].button_events[pos, ButtonNrComboBox.SelectedIndex] = ButtonEventsComboBox.SelectedItem.ToString(); 
                aircraftControls.aircraft_controls[index].AddButton(pos, ButtonNrComboBox.SelectedIndex+1);
                if ((ButtonAxisLinkComboBox.SelectedIndex == -1) || (ButtonAxisLinkComboBox.SelectedItem.ToString() == "None"))
                {
                    aircraftControls.aircraft_controls[index].button_axis_link[pos, ButtonNrComboBox.SelectedIndex] = -1;
                }
                else
                {
                    aircraftControls.aircraft_controls[index].button_axis_link[pos, ButtonNrComboBox.SelectedIndex] = ButtonAxisLinkComboBox.SelectedIndex - 1;
                }
                aircraftControls.aircraft_controls[index].button_is_switch[pos, ButtonNrComboBox.SelectedIndex] = (bool)ButtonIsSwitchCheckbox.IsChecked;
            }
        }


        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NewEventName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var selectedButtonEvent = ButtonEventsComboBox.SelectedItem;
                
                aircraftControls.all_events.Add(NewEventName.Text);
                aircraftControls.all_events.Sort();
                ButtonEventsComboBox.Items.Clear();
                for (int j = 0; j < aircraftControls.all_events.Count; j++)
                {
                    ButtonEventsComboBox.Items.Add(aircraftControls.all_events[j]);
                }
                NewEventName.Text = "";
                ButtonEventsComboBox.SelectedItem = selectedButtonEvent;
            }
        }

        public void ReadControllersState()
        {
            while (true)
            {
                Thread.Sleep(30);
                bool auto_select = false;
                this.Dispatcher.Invoke(() =>
                {
                    auto_select = (bool)AutoSelectButtonCheckbox.IsChecked;
                });
                if (auto_select == true)
                {
                    for (int i = 0; i < buttonArray.Length; i++)
                    {
                        buttonArray[i] = mygamecontroller.buttonArray[controller_id, i];
                        //Debug.WriteLine($"Button state {i} is {buttonArray[i]}");
                        if (buttonArray[i] == true)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                ButtonNrComboBox.SelectedIndex = i;
                            });
                        }
                    }
                }
                
            }
        }

        private void SendEventButton_Click(object sender, RoutedEventArgs e)
        {
            string sim_event = NewEventName.Text;
            mysimconnect.SendEvent(sim_event, 1); Debug.WriteLine($"Event {sim_event} sent!");
        }
    }
}
