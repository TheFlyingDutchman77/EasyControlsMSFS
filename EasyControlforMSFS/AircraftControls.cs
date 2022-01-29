using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Configuration;
using System.Diagnostics;

namespace EasyControlforMSFS
{
    public class AircraftControls
    {
        public List<string> all_events = new List<string>();
        public List<string> aircraft = new List<string>();
        public List<string> defined_controllers = new List<string>();


        public List<AircraftControlsData> aircraft_controls;


        public class AircraftControlsData
        {
            public static int max_num_ac = 10;
            public static int max_num_events = 10;
            public static int max_num_axis = 30;
            public static int max_num_buttons = 160;

            public int max_nr_axis = max_num_axis;
            public int max_nr_buttons = max_num_buttons;


            public string controller_name { get; set; }
            public int num_aircraft { get; set; }
            public int[] num_axis = new int[max_num_axis];
            public int[] num_buttons = new int[max_num_buttons];



            public string[] aircraft_names = new string[max_num_ac];
            public string[,] axis_events = new string[max_num_ac, max_num_axis];
            public string[,] axis_events2 = new string[max_num_ac, max_num_axis];
            public string[,] button_events = new string[max_num_ac, max_num_buttons];
            public int[,] button_axis_link = new int[max_num_ac, max_num_buttons];
            public bool[,] button_is_switch = new bool[max_num_ac, max_num_buttons];
            public bool[,] switch_on = new bool[max_num_ac, max_num_buttons];


            public int[,] axis_min = new int[max_num_ac, max_num_axis];
            public int[,] axis_min2 = new int[max_num_ac, max_num_axis];
            public int[,] axis_max = new int[max_num_ac, max_num_axis];
            public int[,] axis_max2 = new int[max_num_ac, max_num_axis];
            public bool[,] axis_inverted = new bool[max_num_ac, max_num_axis];
            public bool[,] axis_inverted2 = new bool[max_num_ac, max_num_axis];

            public void AddAxis(int aircraft, int nr_axis)
            {
                num_axis[aircraft] = nr_axis;
            }
            public void AddButton(int aircraft, int nr_button)
            {
                if (nr_button > num_buttons[aircraft])
                {
                    num_buttons[aircraft] = nr_button;
                }
            }
            public void AddAircraft(int aircraft, string aircraft_name)
            {
                aircraft_names[aircraft] = aircraft_name;
            }
            public void AddAxisEvent(int aircraft, int axis, string event_name)
            {
                axis_events[aircraft, axis] = event_name;
            }
            public void AddAxisEvent2(int aircraft, int axis, string event_name)
            {
                axis_events2[aircraft, axis] = event_name;
            }
            public void AddButtonEvent(int aircraft, int button, string event_name)
            {
                button_events[aircraft, button] = event_name;
            }
            public void AddButtonAxisLink(int aircraft, int button, int axis)
            {
                button_axis_link[aircraft, button] = axis;
            }
            public void AddButtonIsSwitch(int aircraft, int button, bool is_switch)
            {
                button_is_switch[aircraft, button] = is_switch;
            }
            public void SetSwitchStatus(int aircraft, int button, bool on_off)
            {
                switch_on[aircraft, button] = on_off;
            }
            public void AddAxisMin(int aircraft, int axis, int min_value)
            {
                axis_min[aircraft, axis] = min_value;
            }
            public void AddAxisMin2(int aircraft, int axis, int min_value)
            {
                axis_min2[aircraft, axis] = min_value;
            }
            public void AddAxisMax(int aircraft, int axis, int max_value)
            {
                axis_max[aircraft, axis] = max_value;
            }
            public void AddAxisMax2(int aircraft, int axis, int max_value)
            {
                axis_max2[aircraft, axis] = max_value;
            }
            public void AddAxisInverted(int aircraft, int axis, bool inverted)
            {
                axis_inverted[aircraft, axis] = inverted;
            }
            public void AddAxisInverted2(int aircraft, int axis, bool inverted)
            {
                axis_inverted2[aircraft, axis] = inverted;
            }
        }

        public AircraftControls()
        {
            aircraft_controls = new List<AircraftControlsData>();
        }

        /// <summary>
        /// This functions loads the current controls.xml file with all definitions
        /// </summary>
        public AircraftControls LoadControlsFile(AircraftControls aircraftControls)
        {

            string controls_file = AppDomain.CurrentDomain.BaseDirectory + "controls.xml";
            aircraftControls.all_events.Add("");



            int controller_id = 0;
            aircraftControls.aircraft_controls.Clear();
            foreach (XElement level1Element in XElement.Load(@controls_file).Elements())
            {
                //Debug.WriteLine(level1Element.Attribute("name").Value);
                aircraftControls.aircraft_controls.Add(new AircraftControls.AircraftControlsData() { controller_name = level1Element.Attribute("name").Value });
                aircraftControls.defined_controllers.Add(level1Element.Attribute("name").Value);
                int aircraft_id = 0;
                Debug.WriteLine($"Controller : {level1Element.Attribute("name").Value} is controller nr {controller_id}");
                

                foreach (XElement level2Element in level1Element.Elements())
                {
                    aircraft_id += 1;
                    //Debug.WriteLine("test");
                    aircraftControls.aircraft_controls[controller_id].AddAircraft(aircraft_id, level2Element.Attribute("name").Value);
                    aircraftControls.aircraft_controls[controller_id].num_aircraft = aircraft_id;
                    if (!aircraftControls.aircraft.Contains(level2Element.Attribute("name").Value)) { aircraftControls.aircraft.Add(level2Element.Attribute("name").Value); }
                    int axis_id = 0;
                    int button_id = 0;
                    for (int j = 0; j < aircraftControls.aircraft_controls[controller_id].max_nr_axis; j++)
                    {
                        aircraftControls.aircraft_controls[controller_id].axis_events[aircraft_id, j] = "";
                        aircraftControls.aircraft_controls[controller_id].axis_events2[aircraft_id, j] = "";
                    }
                    for (int j = 0; j < aircraftControls.aircraft_controls[controller_id].max_nr_buttons; j++)
                    {
                        aircraftControls.aircraft_controls[controller_id].button_events[aircraft_id, j] = "";
                    }
                    foreach (XElement level3Element in level2Element.Elements())
                    {
                        //Debug.WriteLine($"Value: {level3Element.Value}");
                        var item = level3Element.Name.ToString();
                        //Debug.WriteLine($"Item level 3: {item}");
                        if (item.Contains("axis"))
                        {
                            aircraftControls.aircraft_controls[controller_id].AddAxis(aircraft_id, axis_id + 1);
                            //Debug.WriteLine($"Nr of axis set to: {axis_id+1}");
                            foreach (XElement level4Element in level3Element.Elements())
                            {
                                var item_axis = level4Element.Name.ToString();
                                //Debug.WriteLine($"Item level 4: {item_axis}");
                                if (item_axis.Contains("event"))
                                { 
                                    if (item_axis.Contains("event2"))
                                    {
                                        aircraftControls.aircraft_controls[controller_id].AddAxisEvent2(aircraft_id, axis_id, level4Element.Value);
                                        if (!aircraftControls.all_events.Contains(level4Element.Value)) { aircraftControls.all_events.Add(level4Element.Value); }
                                    }
                                    else
                                    {
                                        aircraftControls.aircraft_controls[controller_id].AddAxisEvent(aircraft_id, axis_id, level4Element.Value);
                                        if (!aircraftControls.all_events.Contains(level4Element.Value)) { aircraftControls.all_events.Add(level4Element.Value); }
                                    }
                                }
                                if (item_axis.Contains("min")) 
                                {
                                    if (item_axis.Contains("min2"))
                                    {
                                        aircraftControls.aircraft_controls[controller_id].AddAxisMin2(aircraft_id, axis_id, Int32.Parse(level4Element.Value));
                                    }
                                    else
                                    {
                                        aircraftControls.aircraft_controls[controller_id].AddAxisMin(aircraft_id, axis_id, Int32.Parse(level4Element.Value));
                                    }
                                }
                                if (item_axis.Contains("max"))
                                {
                                    if (item_axis.Contains("max2"))
                                    {
                                        aircraftControls.aircraft_controls[controller_id].AddAxisMax2(aircraft_id, axis_id, Int32.Parse(level4Element.Value));
                                    }
                                    else
                                    {
                                        aircraftControls.aircraft_controls[controller_id].AddAxisMax(aircraft_id, axis_id, Int32.Parse(level4Element.Value));
                                    }
                                }
                                if (item_axis.Contains("inverted"))
                                {
                                    if (item_axis.Contains("inverted2"))
                                    {
                                        aircraftControls.aircraft_controls[controller_id].AddAxisInverted2(aircraft_id, axis_id, bool.Parse(level4Element.Value));
                                    }
                                    else
                                    {
                                        aircraftControls.aircraft_controls[controller_id].AddAxisInverted(aircraft_id, axis_id, bool.Parse(level4Element.Value));
                                    }
                                }
                            }
                            axis_id += 1;
                        }
                        if (item.Contains("button"))
                        {
                            button_id = Int32.Parse(item.Replace("button", ""));
                            //Debug.WriteLine($"Button nr: {button_id}");

                            aircraftControls.aircraft_controls[controller_id].AddButton(aircraft_id, button_id+1);
                            foreach (XElement level4Element in level3Element.Elements())
                            {
                                var item_button = level4Element.Name.ToString();
                                //Debug.WriteLine(item_button);
                                if (item_button.Contains("event"))
                                {
                                    aircraftControls.aircraft_controls[controller_id].AddButtonEvent(aircraft_id, button_id, level4Element.Value);
                                    if (!aircraftControls.all_events.Contains(level4Element.Value)) { aircraftControls.all_events.Add(level4Element.Value); }
                                }
                                //Debug.WriteLine(level4Element.Value);
                                if (item_button.Contains("axis_link")) 
                                {
                                    if (level4Element.Value != "")
                                    {
                                        aircraftControls.aircraft_controls[controller_id].AddButtonAxisLink(aircraft_id, button_id, Int32.Parse(level4Element.Value));
                                    }
                                    else
                                    {
                                        aircraftControls.aircraft_controls[controller_id].AddButtonAxisLink(aircraft_id, button_id, -1);
                                    }
                                }
                                if (item_button.Contains("is_switch"))
                                {
                                    if (level4Element.Value != "")
                                    {
                                        aircraftControls.aircraft_controls[controller_id].AddButtonIsSwitch(aircraft_id, button_id, bool.Parse(level4Element.Value));
                                    }
                                }
                            }
                            //button_id += 1;
                        }
                    }
                }
                //Debug line
                controller_id += 1;
            }

            for (int j = 0; j < aircraftControls.aircraft_controls.Count; j++)
            {
                var controller = aircraftControls.aircraft_controls[j];
                //Debug.WriteLine($"{aircraft_controls.Count}, Controller: {controller.controller_name}");
                for (int i = 1; i <= controller.num_aircraft; i++)
                {
                    //Debug.WriteLine($"Controller: {controller.controller_name} with Aircraft: {controller.aircraft_names[i]}, heeft {controller.num_axis} assen met controls: axis1: {controller.axis_events[i,1]} en axis2: {controller.axis_events[i,2]}, en {controller.num_buttons} buttons");
                }

            }
            return aircraftControls;
        }


        /// <summary>
        /// Creating the actual xml file
        /// </summary>
        public void SaveXML(AircraftControls aircraftControls)
        {
            string output_file = "";
            // we loop over the controls
            output_file += "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n";
            output_file += "<root>\r\n";
            for (int i = 0; i < aircraftControls.aircraft_controls.Count; i++)
            {
                output_file += "\t<joystick name=\"" + aircraftControls.aircraft_controls[i].controller_name + "\">\r\n";
                for (int j = 1; j <= aircraftControls.aircraft_controls[i].num_aircraft; j++)
                {
                    output_file += "\t\t<aircraft name=\"" + aircraftControls.aircraft_controls[i].aircraft_names[j] + "\">\r\n";
                    for (int k = 0; k < aircraftControls.aircraft_controls[i].num_axis[j]; k++)
                    {
                        if (aircraftControls.aircraft_controls[i].axis_events[j, k] != "")
                        {
                            output_file += "\t\t\t<axis" + k + ">\r\n";
                            output_file += "\t\t\t\t<event>" + aircraftControls.aircraft_controls[i].axis_events[j, k] + "</event>\r\n";
                            output_file += "\t\t\t\t<min>" + aircraftControls.aircraft_controls[i].axis_min[j, k] + "</min>\r\n";
                            output_file += "\t\t\t\t<max>" + aircraftControls.aircraft_controls[i].axis_max[j, k] + "</max>\r\n";
                            output_file += "\t\t\t\t<inverted>" + aircraftControls.aircraft_controls[i].axis_inverted[j, k] + "</inverted>\r\n";
                            if (aircraftControls.aircraft_controls[i].axis_events2[j, k] != "")
                            {
                                //Debug.WriteLine($"Event 2 is {aircraftControls.aircraft_controls[i].axis_events2[j, k]}");
                                output_file += "\t\t\t\t<event2>" + aircraftControls.aircraft_controls[i].axis_events2[j, k] + "</event2>\r\n";
                                output_file += "\t\t\t\t<min2>" + aircraftControls.aircraft_controls[i].axis_min2[j, k] + "</min2>\r\n";
                                output_file += "\t\t\t\t<max2>" + aircraftControls.aircraft_controls[i].axis_max2[j, k] + "</max2>\r\n";
                                output_file += "\t\t\t\t<inverted2>" + aircraftControls.aircraft_controls[i].axis_inverted2[j, k] + "</inverted2>\r\n";
                            }
                            output_file += "\t\t\t</axis" + k + ">\r\n";
                        }
                    }
                    for (int k = 0; k < aircraftControls.aircraft_controls[i].num_buttons[j]; k++)
                    {
                        if (aircraftControls.aircraft_controls[i].button_events[j, k] != "")
                        {
                            //Debug.WriteLine($"Button {k} event is:{aircraftControls.aircraft_controls[i].button_events[j, k]}");
                            output_file += "\t\t\t<button" + k + ">\r\n";
                            output_file += "\t\t\t\t<event>" + aircraftControls.aircraft_controls[i].button_events[j, k] + "</event>\r\n";
                            if (aircraftControls.aircraft_controls[i].button_axis_link[j, k] != -1)
                            {
                                output_file += "\t\t\t\t<axis_link>" + aircraftControls.aircraft_controls[i].button_axis_link[j, k] + "</axis_link>\r\n";
                            }
                            else { output_file += "\t\t\t\t<axis_link></axis_link>\r\n"; }
                            output_file += "\t\t\t\t<is_switch>" + aircraftControls.aircraft_controls[i].button_is_switch[j, k] + "</is_switch>\r\n";
                            output_file += "\t\t\t</button" + k + ">\r\n";
                        }
                    }
                    output_file += "\t\t</aircraft>\r\n";
                }
                output_file += "\t</joystick>\r\n";
            }
            output_file += "</root>\r\n";
            string controls_file = AppDomain.CurrentDomain.BaseDirectory + "controls.xml";
            File.WriteAllTextAsync(controls_file, output_file);
        }

    }


}
