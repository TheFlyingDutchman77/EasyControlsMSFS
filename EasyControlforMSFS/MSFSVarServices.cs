using System;
using System.Collections.Generic;
using System.Text;
using FSUIPC;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows;
using VS = FSUIPC.MSFSVariableServices;


namespace EasyControlforMSFS
{



    public class MSFSVarServices
    {
        public List<LVarData> lVarData;

        public class LVarData
        {
            public string lvar { get; set; }
            public float value { get; set; }
        }


        // variable to hold the LVar for listening for changes
        private FsLVar lVarListen = null;

        // Keep track of when the MSFSVariableServices starts and stops
        private bool started = false;

        public MSFSVarServices()
        {
            lVarData = new List<LVarData>();
        }


        public void InitMSFSServices()
        {
            // Get a new instance of the MSFSVariableServices class
            //this.VS = new MSFSVariableServices();

            // Handle events
            VS.OnLogEntryReceived += VS_OnLogEntryReceived; // Fired when the WASM module sends a log entry
            VS.OnVariableListChanged += VS_VariableListChanged; // Fired when the list of available variables is changed
            VS.OnValuesChanged += VS_OnValuesChanged; // Fired when any LVAR value changes
                                                      // Initialise and start

            Window mSFSServicesWindow = new Window();
            var helper = new WindowInteropHelper(mSFSServicesWindow);
            helper.EnsureHandle();
            var wih = new WindowInteropHelper(mSFSServicesWindow).Handle;


            VS.Init(wih); // Initialise by passing in the windows handle of this form
            VS.LVARUpdateFrequency = 10; // Check for changes in lvar values 10 times per second (Hz)
            VS.LogLevel = LOGLEVEL.LOG_LEVEL_INFO; // Set the level of logging

            VS.Start();
            started = VS.IsRunning;
            Debug.WriteLine("MSFSServices has started");

            AddLvarsToList();

        }

        public void AddLvarsToList()
        {
            lVarData.Add(new LVarData() { lvar = "ASCRJ_FCP_WHEEL_INFO", value = 0 });
            lVarData.Add(new LVarData() { lvar = "ASCRJ_FCP_SPEED_INFO", value = 0 });
            lVarData.Add(new LVarData() { lvar = "ASCRJ_FCP_HDG_INFO", value = 0 });
            lVarData.Add(new LVarData() { lvar = "ASCRJ_FCP_APPR_LED", value = 0 });
            lVarData.Add(new LVarData() { lvar = "ASCRJ_FCP_VS_LED", value = 0 });
            lVarData.Add(new LVarData() { lvar = "ASCRJ_APU_STARTSTOP_AVAIL", value = 0 });
            lVarData.Add(new LVarData() { lvar = "ASCRJ_ENGS_CONT_IGN_ON", value = 0 });
            lVarData.Add(new LVarData() { lvar = "ASCRJ_FUEL_PUMP_L", value = 0 });
            lVarData.Add(new LVarData() { lvar = "ASCRJ_FUEL_PUMP_L_ON", value = 0 });
            lVarData.Add(new LVarData() { lvar = "ASCRJ_FUEL_PUMP_L_INOP", value = 0 });
            lVarData.Add(new LVarData() { lvar = "ASCRJ_FUEL_PUMP_R", value = 0 });
            lVarData.Add(new LVarData() { lvar = "ASCRJ_FUEL_PUMP_R_ON", value = 0 });
            lVarData.Add(new LVarData() { lvar = "ASCRJ_FUEL_PUMP_R_INOP", value = 0 });
            lVarData.Add(new LVarData() { lvar = "ASCRJ_ENGS_START_L_ON", value = 0 });
            lVarData.Add(new LVarData() { lvar = "ASCRJ_ENGS_START_R_ON", value = 0 });
            lVarData.Add(new LVarData() { lvar = "XMLVAR_A320_WeatherRadar_Sys", value = 0 });
            lVarData.Add(new LVarData() { lvar = "A32NX_SWITCH_RADAR_PWS_Position", value = 0 });


        }


        public void VS_EventSet(string eventname, double value) 
        {
            VS.LVars[eventname].SetValue(value);
            Debug.WriteLine($"Event {eventname} set to {value}");            
        }



        // Fired when the WASM module sends a log entry
        private void VS_OnLogEntryReceived(object sender, LogEventArgs e)
        {
            // Writing to the list box on the form must be done on the UI Thread.
            // This event handler might be on a different thread, so we must use invoke to get back to the main UI thread.
            Debug.WriteLine($"Log MSFSServices received {e.LogEntry}");

        }



        // Fired when the list of available variables has changed
        private void VS_VariableListChanged(object sender, EventArgs e)
        {
            // Changing controls on the form must be done on the main UI thread.
            // This event handler will be on a different thread, so we must use invoke to get back to the main UI thread.

            // Populate the comboboxes with the new variable list

        }

        // Fired when any LVAR value changes
        private void VS_OnValuesChanged(object sender, EventArgs e)
        {
            // Displaying the values must be done on the main UI thread.
            // This event handler will be on a different thread, so we must use invoke to get back to the main UI thread.
            foreach (FsLVar lvar in VS.LVars)
            {
                // Do something with each LVar... e.g.
                //Debug.WriteLine($"Lvar received: {lvar.Name}");
            }

            foreach (LVarData item in lVarData)
            {
                try
                {
                    FsLVar myLVar = VS.LVars[item.lvar];
                    if (myLVar is null)
                    {

                    }
                    else
                    {
                        item.value = (float)myLVar.Value;
                        //Debug.WriteLine($"LVar {myLVar.Name} en value {myLVar.Value}");
                    }
                }
                catch { };
            }

        }


    }

    


}
