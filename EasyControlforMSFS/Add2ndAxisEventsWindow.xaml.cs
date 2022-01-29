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

namespace EasyControlforMSFS
{
    /// <summary>
    /// Interaction logic for Add2ndAxisEventsWindow.xaml
    /// </summary>
    public partial class Add2ndAxisEventsWindow : Window
    {
        public AircraftControls aircraftControls;
        public GameControllerReader mygamecontroller;
        public string selected_aircraft;
        public string selected_controller;
        public int controller_id;
        public int aircraft_id;


        public Add2ndAxisEventsWindow(AircraftControls aircraftControlsInput, GameControllerReader myGameControllerInput, string selected_aircraftInput, string selected_controllerInput, int controller_idInput)
        {
            InitializeComponent();
            aircraftControls = aircraftControlsInput;
            selected_controller = selected_controllerInput;
            selected_aircraft = selected_aircraftInput;
            controller_id = controller_idInput;
            mygamecontroller = myGameControllerInput;

            aircraft_id = Array.IndexOf(aircraftControls.aircraft_controls[controller_id].aircraft_names, selected_aircraft);
            //Now we add the elements to the comboboxes
            aircraftControls.all_events.Sort();
            for (int j = 0; j < aircraftControls.all_events.Count; j++)
            {
                AxisEventsComboBox.Items.Add(aircraftControls.all_events[j]);
            }
            Debug.WriteLine($"Aantal assen: {aircraft_id}, {aircraftControls.aircraft_controls[controller_id].num_axis[aircraft_id]}");
            for (int i = 0; i < aircraftControls.aircraft_controls[controller_id].num_axis[aircraft_id]; i++)
            {
                int axis_nr = i + 1;
                SelectAxisComboBox.Items.Add("Axis " + axis_nr);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(selected_aircraft);
            if (aircraft_id > -1) //yes, it is already defined
            {
                SetInputValuestoList(controller_id, aircraft_id);
            }
            else
            {
            //not happening, window will only load with aircraft id > -1
            }
            aircraftControls.SaveXML(aircraftControls);
        }

        private void SetInputValuestoList(int index, int pos)
        {
            if (AxisEventsComboBox.SelectedIndex != -1)
            {
                aircraftControls.aircraft_controls[index].axis_events2[pos, SelectAxisComboBox.SelectedIndex] = AxisEventsComboBox.SelectedItem.ToString();
                aircraftControls.aircraft_controls[index].axis_min2[pos, SelectAxisComboBox.SelectedIndex] = Int32.Parse(MinAxisTextBox.Text);
                aircraftControls.aircraft_controls[index].axis_max2[pos, SelectAxisComboBox.SelectedIndex] = Int32.Parse(MaxAxisTextBox.Text);
                aircraftControls.aircraft_controls[index].axis_inverted2[pos, SelectAxisComboBox.SelectedIndex] = (bool)InvertedAxisCheckBox.IsChecked;
                
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
                var selectedAxisEvent = AxisEventsComboBox.SelectedItem;

                aircraftControls.all_events.Add(NewEventName.Text);
                aircraftControls.all_events.Sort();
                AxisEventsComboBox.Items.Clear();
                for (int j = 0; j < aircraftControls.all_events.Count; j++)
                {
                    AxisEventsComboBox.Items.Add(aircraftControls.all_events[j]);
                }
                NewEventName.Text = "";
                AxisEventsComboBox.SelectedItem = selectedAxisEvent;
            }
        }

        private void SelectAxisComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AxisEventsComboBox.SelectedItem = aircraftControls.aircraft_controls[controller_id].axis_events2[aircraft_id, SelectAxisComboBox.SelectedIndex];
            MinAxisTextBox.Text = aircraftControls.aircraft_controls[controller_id].axis_min2[aircraft_id, SelectAxisComboBox.SelectedIndex].ToString();
            MaxAxisTextBox.Text = aircraftControls.aircraft_controls[controller_id].axis_max2[aircraft_id, SelectAxisComboBox.SelectedIndex].ToString();
            InvertedAxisCheckBox.IsChecked = aircraftControls.aircraft_controls[controller_id].axis_inverted2[aircraft_id, SelectAxisComboBox.SelectedIndex];
            
            Debug.Write(SelectAxisComboBox.SelectedIndex);
            Debug.WriteLine(aircraftControls.aircraft_controls[controller_id].axis_events2[aircraft_id, SelectAxisComboBox.SelectedIndex]);
        }
    }
}
