﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    class RequestController
    {
        //GPIOinterface == Singleton? GPIOinterface needs to be only one instance
        protected static GPIOinterface gpio = new GPIOinterface();

        public static void handleRequest(Request r)
        {
            if (r != null)
            {

                string command = "HelloWorld." + r.command;


                try
                {
                    // Create dynamically an instance of the requested Command type 
                    Assembly executingAssembly = typeof(LightLED).GetTypeInfo().Assembly;
                    Type commandType = executingAssembly.GetType(command);
                    Command com = (Command)Activator.CreateInstance(commandType);
                    //com 

                    // execute the requested command
                    Debug.WriteLine("Found the following type in Request: " + com != null ? com.GetType().FullName : "none");
                    com.execute(r.parameter);
                }
                catch (ArgumentNullException an)
                {
                    Debug.WriteLine("The requestet command was not found:" + an.Message);
                }
                catch (Exception)
                {
                    Debug.WriteLine("Something went wrong :( ");
                }

            }
        }
    }

    public interface Command
    {
        void execute(Object parameter);

        void undo();
    }

    class LightLED : RequestController, Command
    {
        //Suggestion: A instance variable which helds the last known state in order to revert it
        //private static Object lastState;

        public void execute(Object parameter)
        {
            string par = parameter.ToString();

            if (par.Equals("1"))
            {
                //Execute appropiate method in GPIOinterface like e.g. gpio.led(1);
                Debug.WriteLine("LED switched On");
            }

            else if (par.Equals("0"))
            {
                //Execute appropiate method in GPIOinterface like e.g. gpio.led(0);
                Debug.WriteLine("LED switched Off");
            }

            else
            {
                Debug.WriteLine("no valid parameter");
            }
        }

        public void undo()
        {
            throw new NotImplementedException();
        }
    }
}
