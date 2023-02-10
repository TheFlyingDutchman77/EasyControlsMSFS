using System;
using System.Collections.Generic;
using System.Text;
using Melanchall.DryWetMidi.Multimedia;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Common;
using System.Diagnostics;
using System.Threading;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace EasyControlforMSFS
{
    public class MIDIcontroller
    {
        public event EventHandler<string> LogResult = null;
        private static IInputDevice inputDevice;
        private static IOutputDevice outputDevice;
        public MQTTclient myMQTTclientforMIDI;
        public SimConnectImplementer mysimconnect;
        public string ACtitle = "";
        public int aircraft_id = 0;
        public bool MIDIconnected = false;
        public bool XMLloadedsuccessfully = false;

        public static int max_num_ac = 100;
        public static int max_num_buttons = 48;
        public static int max_num_knobs = 48;
        public string MIDIcontroller_name;
        public string[] aircraft = new string[max_num_ac];
        public string[,] button_events = new string[max_num_ac, max_num_buttons]; //16 knobs x 2 + 8 knob presses x 2
        public string[,] button_events_off = new string[max_num_ac, max_num_buttons]; //16 knobs x 2 + 8 knob presses x 2
        public string[,] button_events_wait = new string[max_num_ac, max_num_buttons]; //16 knobs x 2 + 8 knob presses x 2
        public string[,] button_status_var = new string[max_num_ac, max_num_buttons]; //16 knobs x 2 + 8 knob presses x 2
        public string[,] knob_events_left = new string[max_num_ac, max_num_knobs]; //including volume lever
        public string[,] knob_events_right = new string[max_num_ac, max_num_knobs]; //including volume lever
        public string[,] knob_events_left_alt = new string[max_num_ac, max_num_knobs]; //including volume lever
        public string[,] knob_events_right_alt = new string[max_num_ac, max_num_knobs]; //including volume lever
        public bool[] button_status = new bool[48];

        public int SimConnect_AP = 0;
        public int SimConnect_HDG = 0;
        public int SimConnect_NAV = 0;
        public int SimConnect_ALT = 0;
        public int SimConnect_VS = 0;
        public int SimConnect_FLC = 0;
        public int SimConnect_SPD = 0;
        public int SimConnect_FD = 0;
        public int SimConnect_YD = 0;
        public int SimConnect_APPR = 0;
        public int SimConnect_HDG_DIR_MAG = 0;
        public string[] SimConnectEventNamesAP = { "AP", "HDG", "NAV", "ALT", "VS", "FLC", "SPD", "FD", "YD", "APPR" };
        public int[] SimConnectEventStatusAP = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public string[] SimConnectEventNamesLights = { "LDG", "TAXI", "STRB", "NAVL", "BCN", "RECOG", "PNL", "LOGO", "WING" };
        public int[] SimConnectEventStatusLights = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };


        public MIDIcontroller(SimConnectImplementer mysimconnectInput)
        {
            mysimconnect = mysimconnectInput;
            try
            {
                inputDevice = InputDevice.GetByName("X-TOUCH MINI");
                inputDevice.EventReceived += OnEventReceived;
                inputDevice.StartEventsListening();
                outputDevice = OutputDevice.GetByName("X-TOUCH MINI");
                outputDevice.EventSent += OnEventSent;


                LogResult?.Invoke(this, "MIDI input initialised for X-TOUCH MINI");
                Debug.WriteLine("MIDI input initialised for X-TOUCH MINI");
                LoadControlsFile();

                MIDIconnected = true;

                Thread lightsMonitor = new Thread(LightsMonitor);
                lightsMonitor.IsBackground = true;
                lightsMonitor.Start();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

        }



        public void OnEventReceived(object sender, MidiEventReceivedEventArgs e)
        {
            Debug.WriteLine(e.Event.ToString());
            string str_event = e.Event.ToString();
            string str_note_nr = str_event.Substring(str_event.LastIndexOf('('));
            string str_value = str_note_nr.Substring(str_note_nr.LastIndexOf(','));
            str_note_nr = str_note_nr.Substring(0, str_note_nr.IndexOf(',') + 1);
            str_note_nr = str_note_nr.Remove(0, 1);
            str_note_nr = str_note_nr.Remove(str_note_nr.Length - 1);
            int note_nr = Int32.Parse(str_note_nr);

            str_value = str_value.Remove(0, 2);
            str_value = str_value.Remove(str_value.Length - 1);
            int value = Int32.Parse(str_value);

            if (e.Event.EventType.ToString() == "ControlChange")
            {
                string direction = "";
                if (value > 64)
                {
                    direction = "right";
                }
                else
                {
                    direction = "left";
                }
                Debug.WriteLine($"Knob: {note_nr}, direction: {direction}");
                
                // HERE WE REDUCE KNOB NR BY 1 BECAUSE KNOBS START AT 1 AND BUTTONS AT 0 IN THE MIDI LIBRARY
                if (note_nr != 10 )
                {
                    SendKnobEvent(note_nr - 1, direction, aircraft_id);
                    Debug.WriteLine($"Event send for knob {note_nr - 1}");
                }
                else
                {
                    if (value == 127)
                    {
                        SendKnobEvent(note_nr - 1, direction, aircraft_id);
                        Debug.WriteLine($"Event send for knob {note_nr - 1}");
                    }
                    if (value == 0)
                    {
                        SendKnobEvent(note_nr - 1, direction, aircraft_id);
                        Debug.WriteLine($"Event send for knob {note_nr - 1}");
                    }
                }
                
            }
            if (e.Event.EventType.ToString() == "NoteOn")
            {
                SendButtonEvent(note_nr, aircraft_id);
            }
        }


        private void OnEventSent(object sender, MidiEventSentEventArgs e)
        {
            var midiDevice = (MidiDevice)sender;
            //Debug.WriteLine($"Event sent to '{midiDevice.Name}' at {DateTime.Now}: {e.Event}");
        }


        public void StopMIDIconnection()
        {
            inputDevice.StopEventsListening();

        }


        public void SetTitle(string title_input)
        {
            ACtitle = title_input;
            aircraft_id = Array.IndexOf(aircraft, ACtitle);
            if (aircraft_id < 0) { aircraft_id = Array.IndexOf(aircraft, "Generic"); } // In case ACtitle is not defined in MIDIcontrols.xml, generic profile will be used.

            Debug.WriteLine($"Title for MIDI controller set to {ACtitle} with ID {aircraft_id}");
        }



        public void SendButtonEvent(int button, int aircraft_id)
        {
            string sim_event = "";
            sim_event = button_events[aircraft_id, button];
            string sim_event_off = "";
            bool has_off_event = false;
            if (button_events_off[aircraft_id, button] != null)
            {
                sim_event_off = button_events_off[aircraft_id, button];
                has_off_event = true;
            }
            // No sim event found, check for generic event
            if (sim_event == null)
            {
                int aircraft_id_generic = Array.IndexOf(aircraft, "Generic");
                sim_event = button_events[aircraft_id_generic, button];
            }
            if (sim_event != null)
            {
                // First the case with ON/OFF events in the event name and the button is ON (so we need an OFF event to fire)
                if (sim_event.Contains("_ON") && button_status[button] && !has_off_event)
                {
                    Debug.WriteLine("ON OFF EVENT FOUND!");
                    sim_event = sim_event.Replace("_ON", "_OFF");
                    Debug.WriteLine($"New sim event is {sim_event}");
                    if (sim_event.Substring(0, 6) == "FSUIPC")
                    {
                        string sim_event_new = sim_event.Replace("FSUIPC.", "");
                        MainWindow.myMSFSVarServices.VS_EventSet(sim_event_new, 1);
                    }
                    else { mysimconnect.SendEvent(sim_event, 0); }
                    button_status[button] = false;
                }
                else // if not ON OFF event in eventname or button is OFF
                {
                    if (button_status[button]) //if button is on
                    {
                        if (has_off_event) // if there is off event defined
                        {
                            if (sim_event_off.Substring(0, 6) == "FSUIPC")
                            {
                                string sim_event_new_off = sim_event_off.Replace("FSUIPC.", "");
                                MainWindow.myMSFSVarServices.VS_EventSet(sim_event_new_off, 1); // we send a 1 to the off event
                            }
                            else { mysimconnect.SendEvent(sim_event_off, 1); } // we send a 1 to the off event
                        }
                        else //if no off event defined, we fire standard event with 0
                        {
                            if (sim_event.Substring(0, 6) == "FSUIPC")
                            {
                                string sim_event_new = sim_event.Replace("FSUIPC.", "");
                                MainWindow.myMSFSVarServices.VS_EventSet(sim_event_new, 0); // we send a 0 instead of 1 for the off event
                            }
                            else { mysimconnect.SendEvent(sim_event, 0); }  // we send a 0 instead of 1 for the off event
                        }
                        button_status[button] = false;
                        if (button_events_wait[aircraft_id, button] != null)
                        {
                            Thread.Sleep(50);
                            string sim_event_wait = button_events_wait[aircraft_id, button];
                            if (sim_event_wait.Substring(0, 6) == "FSUIPC")
                            {
                                string sim_event_new = sim_event_wait.Replace("FSUIPC.", "");
                                MainWindow.myMSFSVarServices.VS_EventSet(sim_event_new, 1);
                                Debug.WriteLine($"Button WAIT event {sim_event_wait} sent with value 1!");
                            }
                            else
                            {
                                mysimconnect.SendEvent(sim_event_wait, 1);
                                Debug.WriteLine($"Button WAIT event {sim_event_wait} sent!");
                            }
                        }
                    }
                    else // button is OFF all cases
                    {
                        if (sim_event.Substring(0, 6) == "FSUIPC")
                        {
                            string sim_event_new = sim_event.Replace("FSUIPC.", "");
                            MainWindow.myMSFSVarServices.VS_EventSet(sim_event_new, 1);
                            Debug.WriteLine($"Button event {sim_event} sent with value 1!");
                        }
                        else
                        {
                            mysimconnect.SendEvent(sim_event, 1);
                            Debug.WriteLine($"Button event {sim_event} sent!");
                        }
                        button_status[button] = true; //button now set to on
                        if (button_events_wait[aircraft_id, button] != null)
                        {
                            Thread.Sleep(50);
                            string sim_event_wait = button_events_wait[aircraft_id, button];
                            if (sim_event_wait.Substring(0, 6) == "FSUIPC")
                            {
                                string sim_event_new = sim_event_wait.Replace("FSUIPC.", "");
                                MainWindow.myMSFSVarServices.VS_EventSet(sim_event_new, 1);
                                Debug.WriteLine($"Button WAIT event {sim_event_wait} sent with value 1!");
                            }
                            else
                            {
                                mysimconnect.SendEvent(sim_event_wait, 1);
                                Debug.WriteLine($"Button WAIT event {sim_event_wait} sent!");
                            }
                        }
                    };
                }
                Debug.WriteLine($"Button event {sim_event} sent!");
                Thread.Sleep(10);
                LogResult?.Invoke(this, $"Button {button} event {sim_event} send to sim");
            }
            else { LogResult?.Invoke(this, $"Button {button} has no events associated"); }
            //Enable click for alternative actions (e.g. slow or fast rotation)
            if (button < 8 && sim_event == null)
            {
                if (button_status[button])
                {
                    button_status[button] = false;
                }
                else
                {
                    button_status[button] = true;
                }
            }
            // Exception for heading set
            if (sim_event == "HEADING_BUG_SET")
            {
                mysimconnect.SendEvent(sim_event, SimConnect_HDG_DIR_MAG);
                button_status[button] = false;
            }

        }


        public void SendKnobEvent(int knob, string direction, int aircraft_id)
        {
            Debug.WriteLine($"aircraft id: {aircraft_id}, direction: {direction}, knob: {knob}");
            string sim_event = "";
            if (direction == "left")
            {
                sim_event = knob_events_left[aircraft_id, knob];
                //Alternative event only working for knobs 0-7, no alt events allowed for knobs 10-17!!!!!
                if (button_status[knob] && knob_events_left_alt[aircraft_id, knob] != null && knob < 9) { sim_event = knob_events_left_alt[aircraft_id, knob]; }
            }
            else
            {
                sim_event = knob_events_right[aircraft_id, knob];
                if (button_status[knob] && knob_events_right_alt[aircraft_id, knob] != null && knob < 9) { sim_event = knob_events_right_alt[aircraft_id, knob]; }
            }
            // No sim event found, check for generic event
            if (sim_event == null)
            {
                Debug.WriteLine("Generic event used");
                int aircraft_id_generic = Array.IndexOf(aircraft, "Generic");
                if (direction == "left")
                {
                    sim_event = knob_events_left[aircraft_id_generic, knob];
                    Debug.WriteLine($"Sim event set {sim_event} for knob {knob}");
                    if (button_status[knob] && knob_events_left_alt != null) { sim_event = knob_events_left_alt[aircraft_id_generic, knob]; }
                    if (knob == 8 || knob == 9)
                    {
                        sim_event = knob_events_left[aircraft_id_generic, knob];
                    }
                }
                else
                {
                    sim_event = knob_events_right[aircraft_id_generic, knob];
                    if (button_status[knob] && knob_events_right_alt != null) { sim_event = knob_events_right_alt[aircraft_id_generic, knob]; }
                    if (knob == 8 || knob == 9)
                    {
                        sim_event = knob_events_right[aircraft_id_generic, knob];
                    }
                }
            }
            if (sim_event != null)
            {
                if (sim_event.Substring(0, 6) == "FSUIPC")
                {
                    string sim_event_new = sim_event.Replace("FSUIPC.", "");
                    MainWindow.myMSFSVarServices.VS_EventSet(sim_event_new, 1);
                }
                else
                {
                    mysimconnect.SendEvent(sim_event, 1);
                }
                LogResult?.Invoke(this, $"Knob {knob} event {sim_event} send to sim for aircraft id {aircraft_id}");
                Debug.WriteLine($"Knob event {sim_event} sent for aircraft id {aircraft_id}!");
                //Thread.Sleep(2);
            }
            else { LogResult?.Invoke(this, $"Knob {knob} has no events associated for aircraft id {aircraft_id}"); }

        }


        public void LightsMonitor()
        {
            mysimconnect.RegisterSimVar("AUTOPILOT MASTER", "Bool", "float", "NOT_USED");
            mysimconnect.RegisterSimVar("AUTOPILOT HEADING LOCK", "Bool", "float", "NOT_USED");
            mysimconnect.RegisterSimVar("AUTOPILOT NAV1 LOCK", "Bool", "float", "NOT_USED");
            mysimconnect.RegisterSimVar("AUTOPILOT ALTITUDE LOCK", "Bool", "float", "NOT_USED");
            mysimconnect.RegisterSimVar("AUTOPILOT VERTICAL HOLD", "Bool", "float", "NOT_USED");
            mysimconnect.RegisterSimVar("AUTOPILOT FLIGHT LEVEL CHANGE", "Bool", "float", "NOT_USED");
            mysimconnect.RegisterSimVar("AUTOPILOT AIRSPEED HOLD", "Bool", "float", "NOT_USED");
            mysimconnect.RegisterSimVar("AUTOPILOT FLIGHT DIRECTOR ACTIVE", "Bool", "float", "NOT_USED");
            mysimconnect.RegisterSimVar("AUTOPILOT YAW DAMPER", "Bool", "float", "NOT_USED");
            mysimconnect.RegisterSimVar("AUTOPILOT APPROACH HOLD", "Bool", "float", "NOT_USED");
            mysimconnect.RegisterSimVar("PLANE HEADING DEGREES MAGNETIC", "degrees", "float", "NOT_USED");

            mysimconnect.RegisterSimVar("LIGHT LANDING", "Bool", "float", "NOT_USED");
            mysimconnect.RegisterSimVar("LIGHT TAXI", "Bool", "float", "NOT_USED");
            mysimconnect.RegisterSimVar("LIGHT STROBE", "Bool", "float", "NOT_USED");
            mysimconnect.RegisterSimVar("LIGHT NAV", "Bool", "float", "NOT_USED");
            mysimconnect.RegisterSimVar("LIGHT BEACON", "Bool", "float", "NOT_USED");
            mysimconnect.RegisterSimVar("LIGHT RECOGNITION", "Bool", "float", "NOT_USED");
            mysimconnect.RegisterSimVar("LIGHT PANEL", "Bool", "float", "NOT_USED");
            mysimconnect.RegisterSimVar("LIGHT LOGO", "Bool", "float", "NOT_USED");
            mysimconnect.RegisterSimVar("LIGHT WING", "Bool", "float", "NOT_USED");



            while (true && mysimconnect.bSimConnected)
            {
                for (int i = 1; i < 48; i++)
                {
                    if (button_status_var[aircraft_id, i] != null)
                    {
                        string status_var = button_status_var[aircraft_id, i];
                        int current_value = 999;
                        int note = i - 8;
                        if (i > 21) { note = i; }

                        if (status_var.Length > 6)
                        {
                            if (status_var.Substring(0, 6) == "FSUIPC")
                            {
                                current_value = (int)MainWindow.myMSFSVarServices.VS_GetLvarValue(status_var.Replace("FSUIPC.", ""));
                                //Debug.WriteLine($"Check status of button {i} with status_var {status_var} with result {current_value}");
                            }
                            else
                            {
                                int index = Array.IndexOf(SimConnectEventNamesAP, status_var);
                                if (index > -1) { current_value = SimConnectEventStatusAP[index]; }
                                else
                                {
                                    index = Array.IndexOf(SimConnectEventNamesLights, status_var);
                                    if (index > -1) { current_value = SimConnectEventStatusLights[index]; }
                                }
                            }
                        }
                        else
                        {
                            int index = Array.IndexOf(SimConnectEventNamesAP, status_var);
                            if (index > -1) { current_value = SimConnectEventStatusAP[index]; }
                            else
                            {
                                index = Array.IndexOf(SimConnectEventNamesLights, status_var);
                                if (index > -1) { current_value = SimConnectEventStatusLights[index]; }
                            }
                        }
                        if (current_value == 1)
                        {
                            outputDevice.SendEvent(new NoteOnEvent((SevenBitNumber)note, (SevenBitNumber)1));
                            button_status[i] = true;
                            Debug.WriteLine($"Button {i} note {note} set to true");
                        }
                        else
                        {
                            if (current_value == 0)
                            {
                                outputDevice.SendEvent(new NoteOffEvent((SevenBitNumber)note, (SevenBitNumber)0));
                                button_status[i] = false;
                            }
                        }
                        //Debug.WriteLine($"Check status of knob {i} with status_var {status_var} with result {current_value}");


                    }
                }
                Thread.Sleep(500);
            }
        }

        public void NewSimConnectData(string sResult)
        {
            //Debug.WriteLine($"NewSimConnectData for MIDI called {sResult}");
            if (sResult.Contains("AUTOPILOT MASTER")) { SimConnect_AP = (int)float.Parse(sResult.Split("|")[1]); SimConnectEventStatusAP[0] = SimConnect_AP; }
            if (sResult.Contains("AUTOPILOT HEADING LOCK |")) { SimConnect_HDG = (int)float.Parse(sResult.Split("|")[1]); SimConnectEventStatusAP[1] = SimConnect_HDG; }
            if (sResult.Contains("AUTOPILOT NAV1 LOCK")) { SimConnect_NAV = (int)float.Parse(sResult.Split("|")[1]); SimConnectEventStatusAP[2] = SimConnect_NAV; }
            if (sResult.Contains("AUTOPILOT ALTITUDE LOCK |")) { SimConnect_ALT = (int)float.Parse(sResult.Split("|")[1]); SimConnectEventStatusAP[3] = SimConnect_ALT; }
            if (sResult.Contains("AUTOPILOT VERTICAL HOLD |")) { SimConnect_VS = (int)float.Parse(sResult.Split("|")[1]); SimConnectEventStatusAP[4] = SimConnect_VS; }
            if (sResult.Contains("AUTOPILOT FLIGHT LEVEL CHANGE")) { SimConnect_FLC = (int)float.Parse(sResult.Split("|")[1]); SimConnectEventStatusAP[5] = SimConnect_FLC; }
            if (sResult.Contains("AUTOPILOT AIRSPEED HOLD |")) { SimConnect_SPD = (int)float.Parse(sResult.Split("|")[1]); SimConnectEventStatusAP[6] = SimConnect_SPD; }
            if (sResult.Contains("AUTOPILOT FLIGHT DIRECTOR ACTIVE")) { SimConnect_FD = (int)float.Parse(sResult.Split("|")[1]); SimConnectEventStatusAP[7] = SimConnect_FD; }
            if (sResult.Contains("AUTOPILOT YAW DAMPER")) { SimConnect_YD = (int)float.Parse(sResult.Split("|")[1]); SimConnectEventStatusAP[8] = SimConnect_YD; }
            if (sResult.Contains("AUTOPILOT APPROACH HOLD")) { SimConnect_APPR = (int)float.Parse(sResult.Split("|")[1]); SimConnectEventStatusAP[9] = SimConnect_APPR; }
            if (sResult.Contains("PLANE HEADING DEGREES MAGNETIC")) { SimConnect_HDG_DIR_MAG = (int)float.Parse(sResult.Split("|")[1]); }

            if (sResult.Contains("LIGHT LANDING")) { int setpoint = (int)float.Parse(sResult.Split("|")[1]); SimConnectEventStatusLights[0] = setpoint; }
            if (sResult.Contains("LIGHT TAXI")) { int setpoint = (int)float.Parse(sResult.Split("|")[1]); SimConnectEventStatusLights[1] = setpoint; }
            if (sResult.Contains("LIGHT STROBE")) { int setpoint = (int)float.Parse(sResult.Split("|")[1]); SimConnectEventStatusLights[2] = setpoint; }
            if (sResult.Contains("LIGHT NAV")) { int setpoint = (int)float.Parse(sResult.Split("|")[1]); SimConnectEventStatusLights[3] = setpoint; }
            if (sResult.Contains("LIGHT BEACON")) { int setpoint = (int)float.Parse(sResult.Split("|")[1]); SimConnectEventStatusLights[4] = setpoint; }
            if (sResult.Contains("LIGHT RECOGNITION")) { int setpoint = (int)float.Parse(sResult.Split("|")[1]); SimConnectEventStatusLights[5] = setpoint; }
            if (sResult.Contains("LIGHT PANEL")) { int setpoint = (int)float.Parse(sResult.Split("|")[1]); SimConnectEventStatusLights[6] = setpoint; }
            if (sResult.Contains("LIGHT LOGO")) { int setpoint = (int)float.Parse(sResult.Split("|")[1]); SimConnectEventStatusLights[7] = setpoint; }
            if (sResult.Contains("LIGHT WING")) { int setpoint = (int)float.Parse(sResult.Split("|")[1]); SimConnectEventStatusLights[8] = setpoint; }


        }





        /// <summary>
        /// Loading MIDIcontrols.xml file
        /// </summary>
        public void LoadControlsFile()
        {
            string controls_file = AppDomain.CurrentDomain.BaseDirectory + "MIDIcontrols.xml";
            bool controller_loaded = false;

            Array.Clear(button_events,0, button_events.Length);
            Array.Clear(button_events_off, 0, button_events_off.Length);
            Array.Clear(button_status_var, 0, button_status_var.Length);
            Array.Clear(knob_events_left, 0, knob_events_left.Length);
            Array.Clear(knob_events_left_alt, 0, knob_events_left_alt.Length);
            Array.Clear(knob_events_right, 0, knob_events_right.Length);
            Array.Clear(knob_events_right_alt, 0, knob_events_right_alt.Length);

            MIDIcontroller_name = "";
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
                        XMLloadedsuccessfully = true;
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
                                        if (item_button.Contains("event") && item_button != "eventOFF" && item_button != "eventWAIT")
                                        {
                                            button_events[aircraft_id, button_nr] = level4Element.Value;
                                            Debug.WriteLine($"Event: {button_events[aircraft_id, button_nr]} for aircraft {aircraft[aircraft_id]} and button {button_nr} loaded from file.");
                                        }
                                        if (item_button == "eventOFF")
                                        {
                                            button_events_off[aircraft_id, button_nr] = level4Element.Value;
                                            Debug.WriteLine($"Event OFF: {button_events_off[aircraft_id, button_nr]} for aircraft {aircraft[aircraft_id]} and button {button_nr} loaded from file.");
                                        }
                                        if (item_button == "eventWAIT")
                                        {
                                            button_events_wait[aircraft_id, button_nr] = level4Element.Value;
                                            Debug.WriteLine($"Event WAIT: {button_events_wait[aircraft_id, button_nr]} for aircraft {aircraft[aircraft_id]} and button {button_nr} loaded from file.");
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
                LogResult?.Invoke(this, $"Error in loading xml file: {e}");
                Thread.Sleep(2000);
            }
            if (!XMLloadedsuccessfully)
            {
                LogResult?.Invoke(this, "ERROR IN XML FILE...");
                Debug.WriteLine("Error in XML");
            }


        }

        

    }

}
