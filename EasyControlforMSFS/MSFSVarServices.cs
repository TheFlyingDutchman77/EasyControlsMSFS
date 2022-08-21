using System;
using System.Collections.Generic;
using System.Text;
using FSUIPC;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows;
using VS = FSUIPC.MSFSVariableServices;
using System.IO;



namespace EasyControlforMSFS
{



    public class MSFSVarServices
    {
        public List<LVarData> lVarData;
        public event EventHandler<string> LogResult = null;
        
        public bool writeLogFile = false;

        public class LVarData
        {
            public string lvar { get; set; }
            public float value { get; set; }
        }


        // variable to hold the LVar for listening for changes
        private FsLVar lVarListen = null;

        // Keep track of when the MSFSVariableServices starts and stops
        private bool started = false;


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
            VS.LVARUpdateFrequency = 0; // CONTROLLED BY THE WASM Check for changes in lvar values 10 times per second (Hz)
            VS.LogLevel = LOGLEVEL.LOG_LEVEL_INFO; // Set the level of logging
            //VS.LogLevel = LOGLEVEL.LOG_LEVEL_DEBUG; // Set the level of logging

            VS.Start();
            started = VS.IsRunning;
            Debug.WriteLine("MSFSServices has started");



        }

        public void VS_EventSet(string eventname, double value) 
        {
            FsLVar lvar = VS.LVars[eventname];
            if (lvar != null)
            {
                lvar.SetValue(value);
                Debug.WriteLine($"Event {eventname} set to {value}");
                LogResult?.Invoke(this, $"Event {eventname} set to {value}");
            }
        }


        public double VS_GetLvarValue(string eventname)
        {
            FsLVar lvar = VS.LVars[eventname];
            if (lvar != null)
            {
                //Debug.WriteLine($"Value requested, event {eventname} with value {lvar.Value}");
                return lvar.Value;
            }
            else
            {
                return 0;
            }
        }




        // Fired when the WASM module sends a log entry
        private void VS_OnLogEntryReceived(object sender, LogEventArgs e)
        {
            // Writing to the list box on the form must be done on the UI Thread.
            // This event handler might be on a different thread, so we must use invoke to get back to the main UI thread.
            Debug.WriteLine($"Log MSFSServices received {e.LogEntry}");
            LogResult?.Invoke(this, $"{e.LogEntry}");
            if (writeLogFile)
            {
                string logfile = AppDomain.CurrentDomain.BaseDirectory + "MSFSVarServices.log";
                using (StreamWriter writer = new StreamWriter(logfile, true)) //// true to append data to the file
                {
                    writer.WriteLine($"{DateTime.Now} - {e.LogEntry}");
                }
            }
            if (e.LogEntry.Contains("Error"))
            {
                VS.Stop();
                LogResult?.Invoke(this, $"MSFSVarServices Stop command given");
                //VS.Start();
            }
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

                  

        }

        // BE CAREFUL TO USE - MIGHT CRASH SIM
        public void Reload()
        {
            VS.Reload();
            LogResult?.Invoke(this, $"MSFSVarServices reload executed");
        }

    }

    


}
