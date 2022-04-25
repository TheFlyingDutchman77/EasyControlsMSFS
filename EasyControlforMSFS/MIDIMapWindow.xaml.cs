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
using System.Xml.Linq;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace EasyControlforMSFS
{
    /// <summary>
    /// Interaction logic for MIDIMapWindow.xaml
    /// </summary>
    public partial class MIDIMapWindow : Window
    {
        public event EventHandler<string> LogResult = null;
        public static int max_num_ac = 30;
        public static int max_num_buttons = 48;
        public static int max_num_knobs = 48;

        public string MIDIcontroller_name;
        public string[] aircraft = new string[max_num_ac];
        public string[,] button_events = new string[max_num_ac, max_num_buttons]; //16 knobs x 2 + 8 knob presses x 2
        public string[,] button_events_off = new string[max_num_ac, max_num_buttons]; //16 knobs x 2 + 8 knob presses x 2
        public string[,] button_status_var = new string[max_num_ac, max_num_buttons]; //16 knobs x 2 + 8 knob presses x 2
        public string[,] knob_events_left = new string[max_num_ac, max_num_knobs]; //including volume lever
        public string[,] knob_events_right = new string[max_num_ac, max_num_knobs]; //including volume lever
        public string[,] knob_events_left_alt = new string[max_num_ac, max_num_knobs]; //including volume lever
        public string[,] knob_events_right_alt = new string[max_num_ac, max_num_knobs]; //including volume lever

        public string selected_aircraft = "";
        public int selected_ac_id = -1;
        private int button_pressed = -1;
        private bool ModeA = true;
        public string str_selected_aircraft = "";

        public MIDIMapWindow(string inputSelectedAircraft)
        {
            InitializeComponent();
            LoadControlsFile();

            //Add aircraft to the combobox
            for (int j = 0; j < max_num_ac; j++)
            {
                if (aircraft[j] != null) { ComboBoxACnames.Items.Add(aircraft[j]); }
            }
            
            selected_ac_id = 0;
            str_selected_aircraft = inputSelectedAircraft;
            TextBoxCurrentActiveProfile.Text = str_selected_aircraft;

            
            int index = Array.IndexOf(aircraft, str_selected_aircraft);
            if (index > -1) { ComboBoxACnames.SelectedIndex = index; }
            else { ComboBoxACnames.SelectedIndex = 0; }

            TextBoxMIDIEventOn.Text = "Select knob or button...";
            TextBoxMIDIEventOFF.Text = "Select knob or button...";
            TextBoxMIDIEventLeft.Text = "Select knob or button...";
            TextBoxMIDIEventRight.Text = "Select knob or button...";
            TextBoxMIDIStatusVar.Text = "Select knob or button...";
            SetAllKnobsButtonsUnselected();
        }

        // IMPORTANT: KNOBS EVENTS ARE STORED UNDER IDs 1-8; BUTTONS UNDER IDs 8-23 (second set (B mode) + 24)


        private void MIDIKnob1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ShowKnobEvents();
            if (button_pressed != 0 || LabelEventLeft.Content.ToString() != "Event left turn:")
            {
                button_pressed = 0;
                if (!ModeA) { button_pressed += 24; }
                LabelEventLeft.Content = "Event left turn:";
                LabelEventRight.Content = "Event right turn:";
                TextBoxMIDIEventOn.Text = button_events[selected_ac_id, button_pressed];
                TextBoxMIDIEventOFF.Text = button_events_off[selected_ac_id, button_pressed];
                TextBoxMIDIStatusVar.Text = button_status_var[selected_ac_id, button_pressed];
                TextBoxMIDIEventLeft.Text = knob_events_left[selected_ac_id, button_pressed];
                TextBoxMIDIEventRight.Text = knob_events_right[selected_ac_id, button_pressed];
            }
            else
            {
                LabelEventLeft.Content = "ALT Event left turn:";
                LabelEventRight.Content = "ALT Event right turn:";
                TextBoxMIDIEventLeft.Text = knob_events_left_alt[selected_ac_id, button_pressed];
                TextBoxMIDIEventRight.Text = knob_events_right_alt[selected_ac_id, button_pressed];
            }
            MIDIKnob1.Fill = Brushes.Yellow;
        }

        private void MIDIKnob2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ShowKnobEvents();
            if (button_pressed != 1 || LabelEventLeft.Content.ToString() != "Event left turn:")
            {
                button_pressed = 1;
                if (!ModeA) { button_pressed += 24; }
                LabelEventLeft.Content = "Event left turn:";
                LabelEventRight.Content = "Event right turn:";
                TextBoxMIDIEventOn.Text = button_events[selected_ac_id, button_pressed];
                TextBoxMIDIEventOFF.Text = button_events_off[selected_ac_id, button_pressed];
                TextBoxMIDIStatusVar.Text = button_status_var[selected_ac_id, button_pressed];
                TextBoxMIDIEventLeft.Text = knob_events_left[selected_ac_id, button_pressed];
                TextBoxMIDIEventRight.Text = knob_events_right[selected_ac_id, button_pressed];
            }
            else
            {
                LabelEventLeft.Content = "ALT Event left turn:";
                LabelEventRight.Content = "ALT Event right turn:";
                TextBoxMIDIEventLeft.Text = knob_events_left_alt[selected_ac_id, button_pressed];
                TextBoxMIDIEventRight.Text = knob_events_right_alt[selected_ac_id, button_pressed];
            }
            MIDIKnob2.Fill = Brushes.Yellow;
        }

        private void MIDIKnob3_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ShowKnobEvents();
            if (button_pressed != 2 || LabelEventLeft.Content.ToString() != "Event left turn:")
            {
                button_pressed = 2;
                if (!ModeA) { button_pressed += 24; }
                LabelEventLeft.Content = "Event left turn:";
                LabelEventRight.Content = "Event right turn:";
                TextBoxMIDIEventOn.Text = button_events[selected_ac_id, button_pressed];
                TextBoxMIDIEventOFF.Text = button_events_off[selected_ac_id, button_pressed];
                TextBoxMIDIStatusVar.Text = button_status_var[selected_ac_id, button_pressed];
                TextBoxMIDIEventLeft.Text = knob_events_left[selected_ac_id, button_pressed];
                TextBoxMIDIEventRight.Text = knob_events_right[selected_ac_id, button_pressed];
            }
            else
            {
                LabelEventLeft.Content = "ALT Event left turn:";
                LabelEventRight.Content = "ALT Event right turn:";
                TextBoxMIDIEventLeft.Text = knob_events_left_alt[selected_ac_id, button_pressed];
                TextBoxMIDIEventRight.Text = knob_events_right_alt[selected_ac_id, button_pressed];
            }
            MIDIKnob3.Fill = Brushes.Yellow;

        }

        private void MIDIKnob4_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ShowKnobEvents();
            if (button_pressed != 3 || LabelEventLeft.Content.ToString() != "Event left turn:")
            {
                button_pressed = 3;
                if (!ModeA) { button_pressed += 24; }
                LabelEventLeft.Content = "Event left turn:";
                LabelEventRight.Content = "Event right turn:";
                TextBoxMIDIEventOn.Text = button_events[selected_ac_id, button_pressed];
                TextBoxMIDIEventOFF.Text = button_events_off[selected_ac_id, button_pressed];
                TextBoxMIDIStatusVar.Text = button_status_var[selected_ac_id, button_pressed];
                TextBoxMIDIEventLeft.Text = knob_events_left[selected_ac_id, button_pressed];
                TextBoxMIDIEventRight.Text = knob_events_right[selected_ac_id, button_pressed];
            }
            else
            {
                LabelEventLeft.Content = "ALT Event left turn:";
                LabelEventRight.Content = "ALT Event right turn:";
                TextBoxMIDIEventLeft.Text = knob_events_left_alt[selected_ac_id, button_pressed];
                TextBoxMIDIEventRight.Text = knob_events_right_alt[selected_ac_id, button_pressed];
            }
            MIDIKnob4.Fill = Brushes.Yellow;
        }

        private void MIDIKnob5_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ShowKnobEvents();
            if (button_pressed != 4 || LabelEventLeft.Content.ToString() != "Event left turn:")
            {
                button_pressed = 4;
                if (!ModeA) { button_pressed += 24; }
                LabelEventLeft.Content = "Event left turn:";
                LabelEventRight.Content = "Event right turn:";
                TextBoxMIDIEventOn.Text = button_events[selected_ac_id, button_pressed];
                TextBoxMIDIEventOFF.Text = button_events_off[selected_ac_id, button_pressed];
                TextBoxMIDIStatusVar.Text = button_status_var[selected_ac_id, button_pressed];
                TextBoxMIDIEventLeft.Text = knob_events_left[selected_ac_id, button_pressed];
                TextBoxMIDIEventRight.Text = knob_events_right[selected_ac_id, button_pressed];
            }
            else
            {
                LabelEventLeft.Content = "ALT Event left turn:";
                LabelEventRight.Content = "ALT Event right turn:";
                TextBoxMIDIEventLeft.Text = knob_events_left_alt[selected_ac_id, button_pressed];
                TextBoxMIDIEventRight.Text = knob_events_right_alt[selected_ac_id, button_pressed];
            }
            MIDIKnob5.Fill = Brushes.Yellow;
        }

        private void MIDIKnob6_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ShowKnobEvents();
            if (button_pressed != 5 || LabelEventLeft.Content.ToString() != "Event left turn:")
            {
                button_pressed = 5;
                if (!ModeA) { button_pressed += 24; }
                LabelEventLeft.Content = "Event left turn:";
                LabelEventRight.Content = "Event right turn:";
                TextBoxMIDIEventOn.Text = button_events[selected_ac_id, button_pressed];
                TextBoxMIDIEventOFF.Text = button_events_off[selected_ac_id, button_pressed];
                TextBoxMIDIStatusVar.Text = button_status_var[selected_ac_id, button_pressed];
                TextBoxMIDIEventLeft.Text = knob_events_left[selected_ac_id, button_pressed];
                TextBoxMIDIEventRight.Text = knob_events_right[selected_ac_id, button_pressed];
            }
            else
            {
                LabelEventLeft.Content = "ALT Event left turn:";
                LabelEventRight.Content = "ALT Event right turn:";
                TextBoxMIDIEventLeft.Text = knob_events_left_alt[selected_ac_id, button_pressed];
                TextBoxMIDIEventRight.Text = knob_events_right_alt[selected_ac_id, button_pressed];
            }
            MIDIKnob6.Fill = Brushes.Yellow;
        }

        private void MIDIKnob7_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ShowKnobEvents();
            if (button_pressed != 6 || LabelEventLeft.Content.ToString() != "Event left turn:")
            {
                button_pressed = 6;
                if (!ModeA) { button_pressed += 24; }
                LabelEventLeft.Content = "Event left turn:";
                LabelEventRight.Content = "Event right turn:";
                TextBoxMIDIEventOn.Text = button_events[selected_ac_id, button_pressed];
                TextBoxMIDIEventOFF.Text = button_events_off[selected_ac_id, button_pressed];
                TextBoxMIDIStatusVar.Text = button_status_var[selected_ac_id, button_pressed];
                TextBoxMIDIEventLeft.Text = knob_events_left[selected_ac_id, button_pressed];
                TextBoxMIDIEventRight.Text = knob_events_right[selected_ac_id, button_pressed];
            }
            else
            {
                LabelEventLeft.Content = "ALT Event left turn:";
                LabelEventRight.Content = "ALT Event right turn:";
                TextBoxMIDIEventLeft.Text = knob_events_left_alt[selected_ac_id, button_pressed];
                TextBoxMIDIEventRight.Text = knob_events_right_alt[selected_ac_id, button_pressed];
            }
            MIDIKnob7.Fill = Brushes.Yellow;
        }

        private void MIDIKnob8_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ShowKnobEvents();
            if (button_pressed != 7 || LabelEventLeft.Content.ToString() != "Event left turn:")
            {
                button_pressed = 7;
                if (!ModeA) { button_pressed += 24; }
                LabelEventLeft.Content = "Event left turn:";
                LabelEventRight.Content = "Event right turn:";
                TextBoxMIDIEventOn.Text = button_events[selected_ac_id, button_pressed];
                TextBoxMIDIEventOFF.Text = button_events_off[selected_ac_id, button_pressed];
                TextBoxMIDIStatusVar.Text = button_status_var[selected_ac_id, button_pressed];
                TextBoxMIDIEventLeft.Text = knob_events_left[selected_ac_id, button_pressed];
                TextBoxMIDIEventRight.Text = knob_events_right[selected_ac_id, button_pressed];
            }
            else
            {
                LabelEventLeft.Content = "ALT Event left turn:";
                LabelEventRight.Content = "ALT Event right turn:";
                TextBoxMIDIEventLeft.Text = knob_events_left_alt[selected_ac_id, button_pressed];
                TextBoxMIDIEventRight.Text = knob_events_right_alt[selected_ac_id, button_pressed];
            }
            MIDIKnob8.Fill = Brushes.Yellow;
        }

        private void MIDIButton8_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            button_pressed = 8;
            if (!ModeA) { button_pressed += 24; }
            HideKnobEvents();
            TextBoxMIDIEventOn.Text = button_events[selected_ac_id, button_pressed];
            TextBoxMIDIEventOFF.Text = button_events_off[selected_ac_id, button_pressed];
            TextBoxMIDIStatusVar.Text = button_status_var[selected_ac_id, button_pressed];
            MIDIButton8.Fill = Brushes.Yellow;
        }

        private void MIDIButton9_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            button_pressed = 9;
            if (!ModeA) { button_pressed += 24; }
            HideKnobEvents();
            TextBoxMIDIEventOn.Text = button_events[selected_ac_id, button_pressed];
            TextBoxMIDIEventOFF.Text = button_events_off[selected_ac_id, button_pressed];
            TextBoxMIDIStatusVar.Text = button_status_var[selected_ac_id, button_pressed];

            MIDIButton9.Fill = Brushes.Yellow;
        }

        private void MIDIButton10_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            button_pressed = 10;
            if (!ModeA) { button_pressed += 24; }
            HideKnobEvents();
            TextBoxMIDIEventOn.Text = button_events[selected_ac_id, button_pressed];
            TextBoxMIDIEventOFF.Text = button_events_off[selected_ac_id, button_pressed];
            TextBoxMIDIStatusVar.Text = button_status_var[selected_ac_id, button_pressed];

            MIDIButton10.Fill = Brushes.Yellow;
        }

        private void MIDIButton11_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            button_pressed = 11;
            if (!ModeA) { button_pressed += 24; }
            HideKnobEvents();
            TextBoxMIDIEventOn.Text = button_events[selected_ac_id, button_pressed];
            TextBoxMIDIEventOFF.Text = button_events_off[selected_ac_id, button_pressed];
            TextBoxMIDIStatusVar.Text = button_status_var[selected_ac_id, button_pressed];

            MIDIButton11.Fill = Brushes.Yellow;
        }

        private void MIDIButton12_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            button_pressed = 12;
            if (!ModeA) { button_pressed += 24; }
            HideKnobEvents();
            TextBoxMIDIEventOn.Text = button_events[selected_ac_id, button_pressed];
            TextBoxMIDIEventOFF.Text = button_events_off[selected_ac_id, button_pressed];
            TextBoxMIDIStatusVar.Text = button_status_var[selected_ac_id, button_pressed];

            MIDIButton12.Fill = Brushes.Yellow;
        }

        private void MIDIButton13_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            button_pressed = 13;
            if (!ModeA) { button_pressed += 24; }
            HideKnobEvents();
            TextBoxMIDIEventOn.Text = button_events[selected_ac_id, button_pressed];
            TextBoxMIDIEventOFF.Text = button_events_off[selected_ac_id, button_pressed];
            TextBoxMIDIStatusVar.Text = button_status_var[selected_ac_id, button_pressed];

            MIDIButton13.Fill = Brushes.Yellow;
        }

        private void MIDIButton14_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            button_pressed = 14;
            if (!ModeA) { button_pressed += 24; }
            HideKnobEvents();
            TextBoxMIDIEventOn.Text = button_events[selected_ac_id, button_pressed];
            TextBoxMIDIEventOFF.Text = button_events_off[selected_ac_id, button_pressed];
            TextBoxMIDIStatusVar.Text = button_status_var[selected_ac_id, button_pressed];

            MIDIButton14.Fill = Brushes.Yellow;
        }

        private void MIDIButton15_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            button_pressed = 15;
            if (!ModeA) { button_pressed += 24; }
            HideKnobEvents();
            TextBoxMIDIEventOn.Text = button_events[selected_ac_id, button_pressed];
            TextBoxMIDIEventOFF.Text = button_events_off[selected_ac_id, button_pressed];
            TextBoxMIDIStatusVar.Text = button_status_var[selected_ac_id, button_pressed];

            MIDIButton15.Fill = Brushes.Yellow;
        }

        private void MIDIButton16_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            button_pressed = 16;
            if (!ModeA) { button_pressed += 24; }
            HideKnobEvents();
            TextBoxMIDIEventOn.Text = button_events[selected_ac_id, button_pressed];
            TextBoxMIDIEventOFF.Text = button_events_off[selected_ac_id, button_pressed];
            TextBoxMIDIStatusVar.Text = button_status_var[selected_ac_id, button_pressed];

            MIDIButton16.Fill = Brushes.Yellow;
        }

        private void MIDIButton17_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            button_pressed = 17;
            if (!ModeA) { button_pressed += 24; }
            HideKnobEvents();
            TextBoxMIDIEventOn.Text = button_events[selected_ac_id, button_pressed];
            TextBoxMIDIEventOFF.Text = button_events_off[selected_ac_id, button_pressed];
            TextBoxMIDIStatusVar.Text = button_status_var[selected_ac_id, button_pressed];

            MIDIButton17.Fill = Brushes.Yellow;
        }

        private void MIDIButton18_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            button_pressed = 18;
            if (!ModeA) { button_pressed += 24; }
            HideKnobEvents();
            TextBoxMIDIEventOn.Text = button_events[selected_ac_id, button_pressed];
            TextBoxMIDIEventOFF.Text = button_events_off[selected_ac_id, button_pressed];
            TextBoxMIDIStatusVar.Text = button_status_var[selected_ac_id, button_pressed];

            MIDIButton18.Fill = Brushes.Yellow;
        }

        private void MIDIButton19_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            button_pressed = 19;
            if (!ModeA) { button_pressed += 24; }
            HideKnobEvents();
            TextBoxMIDIEventOn.Text = button_events[selected_ac_id, button_pressed];
            TextBoxMIDIEventOFF.Text = button_events_off[selected_ac_id, button_pressed];
            TextBoxMIDIStatusVar.Text = button_status_var[selected_ac_id, button_pressed];

            MIDIButton19.Fill = Brushes.Yellow;
        }

        private void MIDIButton20_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            button_pressed = 20;
            if (!ModeA) { button_pressed += 24; }
            HideKnobEvents();
            TextBoxMIDIEventOn.Text = button_events[selected_ac_id, button_pressed];
            TextBoxMIDIEventOFF.Text = button_events_off[selected_ac_id, button_pressed];
            TextBoxMIDIStatusVar.Text = button_status_var[selected_ac_id, button_pressed];

            MIDIButton20.Fill = Brushes.Yellow;
        }

        private void MIDIButton21_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            button_pressed = 21;
            if (!ModeA) { button_pressed += 24; }
            HideKnobEvents();
            TextBoxMIDIEventOn.Text = button_events[selected_ac_id, button_pressed];
            TextBoxMIDIEventOFF.Text = button_events_off[selected_ac_id, button_pressed];
            TextBoxMIDIStatusVar.Text = button_status_var[selected_ac_id, button_pressed];

            MIDIButton21.Fill = Brushes.Yellow;
        }

        private void MIDIButton22_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            button_pressed = 22;
            if (!ModeA) { button_pressed += 24; }
            HideKnobEvents();
            TextBoxMIDIEventOn.Text = button_events[selected_ac_id, button_pressed];
            TextBoxMIDIEventOFF.Text = button_events_off[selected_ac_id, button_pressed];
            TextBoxMIDIStatusVar.Text = button_status_var[selected_ac_id, button_pressed];

            MIDIButton22.Fill = Brushes.Yellow;
        }

        private void MIDIButton23_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            button_pressed = 23;
            if (!ModeA) { button_pressed += 24; }
            HideKnobEvents();
            TextBoxMIDIEventOn.Text = button_events[selected_ac_id, button_pressed];
            TextBoxMIDIEventOFF.Text = button_events_off[selected_ac_id, button_pressed];
            TextBoxMIDIStatusVar.Text = button_status_var[selected_ac_id, button_pressed];

            MIDIButton23.Fill = Brushes.Yellow;
        }


        private void MIDISlider_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ShowKnobEvents();
            button_pressed = 8;
            if (!ModeA) { button_pressed = 9; }
            LabelEventLeft.Content = "Event left turn:";
            LabelEventRight.Content = "Event right turn:";
            TextBoxMIDIEventLeft.Text = knob_events_left[selected_ac_id, button_pressed];
            TextBoxMIDIEventRight.Text = knob_events_right[selected_ac_id, button_pressed];
            TextBoxMIDIEventOn.Text = "";
            TextBoxMIDIEventOFF.Text = "";

        }

        public void ShowKnobEvents()
        {
            StackPanel_EventLeft.Visibility = Visibility.Visible;
            StackPanel_EventRight.Visibility = Visibility.Visible;
            LabelStatusVar.Visibility = Visibility.Hidden;
            TextBoxMIDIStatusVar.Visibility = Visibility.Hidden;
            SetAllKnobsButtonsUnselected();
        }

        public void HideKnobEvents()
        {
            StackPanel_EventLeft.Visibility = Visibility.Hidden;
            StackPanel_EventRight.Visibility = Visibility.Hidden;
            LabelStatusVar.Visibility = Visibility.Visible;
            TextBoxMIDIStatusVar.Visibility = Visibility.Visible;
            SetAllKnobsButtonsUnselected(); 
        }

        public void SetAllKnobsButtonsUnselected()
        {
            MIDIKnob1.Fill = Brushes.Black;
            MIDIKnob2.Fill = Brushes.Black;
            MIDIKnob3.Fill = Brushes.Black;
            MIDIKnob4.Fill = Brushes.Black;
            MIDIKnob5.Fill = Brushes.Black;
            MIDIKnob6.Fill = Brushes.Black;
            MIDIKnob7.Fill = Brushes.Black;
            MIDIKnob8.Fill = Brushes.Black;
            MIDIButton8.Fill = Brushes.LightGray;
            MIDIButton9.Fill = Brushes.LightGray;
            MIDIButton10.Fill = Brushes.LightGray;
            MIDIButton11.Fill = Brushes.LightGray;
            MIDIButton12.Fill = Brushes.LightGray;
            MIDIButton13.Fill = Brushes.LightGray;
            MIDIButton14.Fill = Brushes.LightGray;
            MIDIButton15.Fill = Brushes.LightGray;
            MIDIButton16.Fill = Brushes.LightGray;
            MIDIButton17.Fill = Brushes.LightGray;
            MIDIButton18.Fill = Brushes.LightGray;
            MIDIButton19.Fill = Brushes.LightGray;
            MIDIButton20.Fill = Brushes.LightGray;
            MIDIButton21.Fill = Brushes.LightGray;
            MIDIButton22.Fill = Brushes.LightGray;
            MIDIButton23.Fill = Brushes.LightGray;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (button_pressed > -1)
            {
                Debug.WriteLine($"Button pressed for saving is {button_pressed}");
                if (TextBoxMIDIEventOn.Text != "") { button_events[selected_ac_id, button_pressed] = TextBoxMIDIEventOn.Text; }
                if (TextBoxMIDIEventOFF.Text != "") { button_events_off[selected_ac_id, button_pressed] = TextBoxMIDIEventOFF.Text; }
                if (TextBoxMIDIStatusVar.Text != "") { button_status_var[selected_ac_id, button_pressed] = TextBoxMIDIStatusVar.Text; }
                if (LabelEventLeft.Visibility == Visibility.Visible && LabelEventLeft.Content.ToString() == "Event left turn:")
                {
                    if (TextBoxMIDIEventLeft.Text != "") { knob_events_left[selected_ac_id, button_pressed] = TextBoxMIDIEventLeft.Text; }
                    if (TextBoxMIDIEventRight.Text != "") { knob_events_right[selected_ac_id, button_pressed] = TextBoxMIDIEventRight.Text; }
                }
                if (LabelEventLeft.Visibility == Visibility.Visible && LabelEventLeft.Content.ToString() == "ALT Event left turn:")
                {
                    if (TextBoxMIDIEventLeft.Text != "") { knob_events_left_alt[selected_ac_id, button_pressed] = TextBoxMIDIEventLeft.Text; }
                    if (TextBoxMIDIEventRight.Text != "") { knob_events_right_alt[selected_ac_id, button_pressed] = TextBoxMIDIEventRight.Text; }
                }
                SaveXML();
            }
            else
            {
                MessageBox.Show("Nothing to save...");
            }
        }



        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ComboBoxACnames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxACnames.SelectedIndex > -1)
            {
                selected_aircraft = ComboBoxACnames.SelectedItem.ToString();
                selected_ac_id = Array.IndexOf(aircraft, selected_aircraft);
                button_pressed = -1;
                TextBoxMIDIEventOn.Text = "Select knob or button...";
                TextBoxMIDIEventOFF.Text = "Select knob or button...";
                TextBoxMIDIEventLeft.Text = "Select knob or button...";
                TextBoxMIDIEventRight.Text = "Select knob or button...";
                TextBoxMIDIStatusVar.Text = "Select knob or button...";
                SetAllKnobsButtonsUnselected();
            }
        }




        public void LoadControlsFile()
        {
            string controls_file = AppDomain.CurrentDomain.BaseDirectory + "MIDIcontrols.xml";
            bool controller_loaded = false;

            string MIDIcontroller_name = "";
            try
            {
                foreach (XElement level1Element in XElement.Load(@controls_file).Elements())
                {
                    if (controller_loaded) { LogResult?.Invoke(this, "MIDI ERROR: More than 1 MIDI controller in input file. Only 1st controllers will be loaded! "); }
                    else // this is the first controller
                    {
                        MIDIcontroller_name = level1Element.Attribute("name").Value;
                        controller_loaded = true;
                        int aircraft_id = 0;
                        foreach (XElement level2Element in level1Element.Elements())
                        {
                            aircraft[aircraft_id] = level2Element.Attribute("name").Value;
                            Debug.WriteLine($"Aircraft definition found: {aircraft[aircraft_id]}");
                            foreach (XElement level3Element in level2Element.Elements())
                            {
                                var item = level3Element.Name.ToString();
                                Debug.WriteLine($"Item level 3: {item}");
                                //Read button data
                                if (item.Contains("button"))
                                {
                                    int button_nr = Int32.Parse(item.Replace("button", ""));
                                    Debug.WriteLine($"Button nr found: {button_nr}");
                                    foreach (XElement level4Element in level3Element.Elements())
                                    {
                                        var item_button = level4Element.Name.ToString();
                                        Debug.WriteLine($"Item level 4: {item_button}");
                                        if (item_button.Contains("event") && item_button != "eventOFF")
                                        {
                                            button_events[aircraft_id, button_nr] = level4Element.Value;
                                            Debug.WriteLine($"Event: {button_events[aircraft_id, button_nr]} for aircraft {aircraft[aircraft_id]} and button {button_nr} loaded from file.");
                                        }
                                        if (item_button == "eventOFF")
                                        {
                                            button_events_off[aircraft_id, button_nr] = level4Element.Value;
                                            Debug.WriteLine($"Event OFF: {button_events_off[aircraft_id, button_nr]} for aircraft {aircraft[aircraft_id]} and button {button_nr} loaded from file.");
                                        }
                                        if (item_button.Contains("status_var"))
                                        {
                                            button_status_var[aircraft_id, button_nr] = level4Element.Value;
                                        }

                                    }
                                }
                                // Now for knobs
                                if (item.Contains("knob"))
                                {
                                    int knob_nr = Int32.Parse(item.Replace("knob", ""));
                                    Debug.WriteLine($"Knob nr found: {knob_nr}");
                                    foreach (XElement level4Element in level3Element.Elements())
                                    {
                                        var item_knob = level4Element.Name.ToString();
                                        Debug.WriteLine($"Item level 4: {item_knob}");
                                        if (item_knob.Contains("event_left") && item_knob.Substring(0, 3) != "alt")
                                        {
                                            knob_events_left[aircraft_id, knob_nr] = level4Element.Value;
                                        }
                                        if (item_knob.Contains("event_right") && item_knob.Substring(0, 3) != "alt")
                                        {
                                            knob_events_right[aircraft_id, knob_nr] = level4Element.Value;
                                        }
                                        if (item_knob.Contains("alt_event_left"))
                                        {
                                            knob_events_left_alt[aircraft_id, knob_nr] = level4Element.Value;
                                        }
                                        if (item_knob.Contains("alt_event_right"))
                                        {
                                            knob_events_right_alt[aircraft_id, knob_nr] = level4Element.Value;
                                        }

                                    }
                                }
                            }
                            aircraft_id++;
                        }
                    }
                }

            }
            catch (System.Xml.XmlException e)
            {
                Debug.WriteLine($"Error in loading xml file: {e}");
                Thread.Sleep(2000);
            }
        }

        private void MIDIButtonA_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ModeA = true;
            MIDIButtonB.Fill = Brushes.LightGray;
            MIDIButtonA.Fill = Brushes.Yellow;
            button_pressed = -1;
            TextBoxMIDIEventOn.Text = "Select knob or button...";
            TextBoxMIDIEventOFF.Text = "Select knob or button...";
            TextBoxMIDIEventLeft.Text = "Select knob or button...";
            TextBoxMIDIEventRight.Text = "Select knob or button...";
            TextBoxMIDIStatusVar.Text = "Select knob or button...";
            SetAllKnobsButtonsUnselected();
        }

        private void MIDIButtonB_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ModeA = false;
            MIDIButtonB.Fill = Brushes.Yellow;
            MIDIButtonA.Fill = Brushes.LightGray;
            button_pressed = -1;
            TextBoxMIDIEventOn.Text = "Select knob or button...";
            TextBoxMIDIEventOFF.Text = "Select knob or button...";
            TextBoxMIDIEventLeft.Text = "Select knob or button...";
            TextBoxMIDIEventRight.Text = "Select knob or button...";
            TextBoxMIDIStatusVar.Text = "Select knob or button...";
            SetAllKnobsButtonsUnselected();
        }


        public void SaveXML()
        {
            string output_file = "";
            // we loop over the controls
            output_file += "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n";
            output_file += "<root>\r\n";

            output_file += "\t<MIDIcontroller name=\"X-TOUCH MINI\">\r\n";

            for (int i = 0; i < aircraft.Length; i++)
            {
                if (aircraft[i] != null)
                {
                    output_file += "\t\t<aircraft name=\"" + aircraft[i] + "\">\r\n";
                    for (int j = 0; j < max_num_knobs; j++)
                    {
                        if (knob_events_left[i, j] != null && knob_events_left[i, j] != "")
                        {
                            Debug.WriteLine($"{i} {j} {knob_events_left[i, j]} ");
                            output_file += "\t\t\t<knob" + j + ">\r\n";
                            output_file += "\t\t\t\t<event_left>" + knob_events_left[i, j] + "</event_left>\r\n";
                            output_file += "\t\t\t\t<event_right>" + knob_events_right[i, j] + "</event_right>\r\n";
                            if (knob_events_left_alt[i, j] != null && knob_events_left_alt[i, j] != "")
                            {
                                output_file += "\t\t\t\t<alt_event_left>" + knob_events_left_alt[i, j] + "</alt_event_left>\r\n";
                                output_file += "\t\t\t\t<alt_event_right>" + knob_events_right_alt[i, j] + "</alt_event_right>\r\n";
                            }
                            output_file += "\t\t\t</knob" + j + ">\r\n";
                        }
                    }
                    for (int j = 0; j < max_num_buttons; j++)
                    {
                        if (button_events[i, j] != null && button_events[i,j] != "")
                        {
                            output_file += "\t\t\t<button" + j + ">\r\n";
                            output_file += "\t\t\t\t<event>" + button_events[i, j] + "</event>\r\n";
                            if (button_events_off[i,j] != null & button_events_off[i, j] != "") { output_file += "\t\t\t\t<eventOFF>" + button_events_off[i, j] + "</eventOFF>\r\n"; }
                            if (button_status_var[i, j] != null & button_status_var[i, j] != "") { output_file += "\t\t\t\t<status_var>" + button_status_var[i, j] + "</status_var>\r\n"; }

                            output_file += "\t\t\t</button" + j + ">\r\n";
                        }
                    }
                    output_file += "\t\t</aircraft>\r\n";
                }


            }
            output_file += "\t</MIDIcontroller>\r\n";

            output_file += "</root>\r\n";
            string controls_file = AppDomain.CurrentDomain.BaseDirectory + "MIDIcontrols.xml";
            File.WriteAllTextAsync(controls_file, output_file);
        }

        private void ButtonAddProfile_Click(object sender, RoutedEventArgs e)
        {
            int selected_index = -1;
            int index = Array.IndexOf(aircraft, TextBoxCurrentActiveProfile.Text);
            if (index > -1)
            {
                MessageBox.Show("Profile already exists!");
            }
            else
            {
                for (int j = 0; j < max_num_ac; j++)
                {
                    if (aircraft[j] == null)
                    {
                        aircraft[j] = TextBoxCurrentActiveProfile.Text;
                        selected_index = j;
                        break;
                    }
                }
                ComboBoxACnames.Items.Clear();
                for (int j = 0; j < max_num_ac; j++)
                {
                    if (aircraft[j] != null) { ComboBoxACnames.Items.Add(aircraft[j]); }
                }
                ComboBoxACnames.SelectedIndex = selected_index;
            }

        }
    }


    
}
