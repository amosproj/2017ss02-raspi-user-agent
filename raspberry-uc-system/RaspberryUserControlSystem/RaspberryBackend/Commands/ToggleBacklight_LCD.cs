﻿using System.Diagnostics;

namespace RaspberryBackend
{
    class ToggleBacklight_LCD : Command
    {
        private const byte ON = 0x01;
        private const byte OFF = 0x00;

        public ToggleBacklight_LCD(RaspberryPi raspberryPi) : base(raspberryPi)
        {
            RequestController.Instance.addRequestedCommand("ToggleBacklight_LCD", this);
        }

        public override void execute(object parameter)
        {
            string requestedParameter = parameter.ToString();

            if (requestedParameter.Equals("1"))
            {
                Debug.WriteLine("Received command ToggleBacklightLCD On!");
                switchToState(ON);

            }
            else if (requestedParameter.Equals("0"))
            {
                Debug.WriteLine("Received command ToggleBacklightLCD Off!");
                switchToState(OFF);
            }


        }

        private void switchToState(byte targetState)
        {
            RaspberryPi.LcdDisplay.backLight = targetState;
            RaspberryPi.LcdDisplay.sendCommand(targetState);
            Debug.WriteLine("Backlight state changed!");

        }
    }
}
