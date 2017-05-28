﻿using System;
using System.Diagnostics;
using Windows.Devices.Gpio;

namespace RaspberryBackend
{
    /// <summary>
    /// This class represents a Command. It it can be used to read a spefic gpio pin of the RaspberryPi. 
    /// </summary>
    class ReadPin : Command
    {
        public GpioPinValue currentState;

        public ReadPin(RaspberryPi raspberryPi) : base(raspberryPi)
        {
        }

        /// <summary>
        /// executes the Command ReadPin 
        /// </summary>
        /// <param name="parameter">represents the GpioPin to read from</param>
        public override void execute(Object parameter)
        {
            UInt16 id = 0;
            if (parameter.GetType() == typeof(UInt16))
            {
                id = (UInt16)parameter;
                RaspberryPi.GpioInterface.setToInput(id);
                currentState = RaspberryPi.GpioInterface.readPin(id);
                Debug.Write(string.Format("Pin {0} has currently the state: ", parameter.ToString()));
                Debug.WriteLine(currentState);
            }
            else
            {
                return;
            }

        }

    }
}
