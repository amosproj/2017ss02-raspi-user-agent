﻿using System;

namespace RaspberryBackend
{

    /// <summary>
    /// This class represents a Command. It it can be used to reset a spefic gpio pin of the RaspberryPi. 
    /// </summary>
    class ResetPin : Command
    {

        public ResetPin(GPIOinterface gpioInterface) : base(gpioInterface)
        {
            RequestController.Instance.addRequestetCommand("ResetPin", this);
        }

        /// <summary>
        /// executes the Command ResetPin
        /// </summary>
        /// <param name="parameter">represents the GpioPin which shall be reset</param>
        public override void execute(Object parameter)
        {
            UInt16 id = 0;
            if (parameter.GetType() == typeof(UInt16))
            {
                id = (UInt16)parameter;
                _gpioInterface.setToOutput(id);
                _gpioInterface.writePin(id, 0);

            }
            else
            {
                return;
            }

        }

    }
}