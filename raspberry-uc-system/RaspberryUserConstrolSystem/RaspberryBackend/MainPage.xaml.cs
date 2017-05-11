﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Runtime.Serialization;
using System.Xml;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HelloWorld
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        const int LED_PIN_5 = 5;
        const int LED_PIN_6 = 6;
        DispatcherTimer timer;
        GpioPinValue pinValue1;
        GpioPinValue pinValue2;
        GpioPin pin1;
        GpioPin pin2;
        public MainPage()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
            InitGpio();
            if(pin1 != null || pin2 != null)
            {
                timer.Start();
            }

            this.InitializeComponent();
            createListenerAsync();
           
        }

        /// <summary>
        /// creates TCP socket that is listening on port 7777 for requests
        /// requests will be handeled by SocketListener_ConnectionReceived
        /// </summary>
        private async Task createListenerAsync()
        {

            //Create a StreamSocketListener to start listening for TCP connections.
            Windows.Networking.Sockets.StreamSocketListener socketListener = new Windows.Networking.Sockets.StreamSocketListener();

            //Hook up an event handler to call when connections are received.
            socketListener.ConnectionReceived += SocketListener_ConnectionReceived;
            Debug.WriteLine("create Listener");

            try
            {
                //Start listening for incoming TCP connections on the specified port
                await socketListener.BindServiceNameAsync("13370");
                Debug.WriteLine("created Listener");
            }
            catch (Exception e)
            {
                //Handle exception.
            }
        }

        /// <summary>
        /// 1. reads a string from the socket
        /// 2. prints the string on debug
        /// 3. deserializes the string into an object of type Request
        /// 4. prints variables of the request object on debug
        /// </summary>
        private async void SocketListener_ConnectionReceived(Windows.Networking.Sockets.StreamSocketListener sender,
            Windows.Networking.Sockets.StreamSocketListenerConnectionReceivedEventArgs args)
        {

   

            //Read line from the remote client.
            Stream inStream = args.Socket.InputStream.AsStreamForRead();
            StreamReader reader = new StreamReader(inStream);
            string request = await reader.ReadLineAsync();

            Debug.WriteLine(request);

            //Deserialize the received string into an object of Type Request
            Request r = (Request) Serializer.Deserialize(request, typeof(Request));

            Debug.WriteLine(r.methodName);
            Debug.WriteLine(r.parameter);
        }

        private void Timer_Tick(object sender, object e)
        {
            //System.Diagnostics.Debug.WriteLine(pin1.Read());
            //System.Diagnostics.Debug.WriteLine(pin2.Read());
            //return;
            /*if (pinValue == GpioPinValue.High)
            {
                pinValue = GpioPinValue.Low;
                pin.Write(pinValue);
                //LED.Fill = redBrush;
            }
            else
            {
                pinValue = GpioPinValue.High;
                pin.Write(pinValue);
                //LED.Fill = grayBrush;
            }*/
        }

        private void InitGpio()
        {
            var gpio = GpioController.GetDefault();

            if(gpio == null)
            {
                pin1 = null;
                pin2 = null;
                return;
            }
            pin1 = gpio.OpenPin(LED_PIN_5);
            pin2 = gpio.OpenPin(LED_PIN_6);
            //pinValue = GpioPinValue.High;
            //pin.Write(pinValue);
            pin1.SetDriveMode(GpioPinDriveMode.InputPullDown);
            pin2.SetDriveMode(GpioPinDriveMode.InputPullDown);
            pin1.ValueChanged += val_changed;
            pin2.ValueChanged += val_changed;
        }

        private void val_changed(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("Pin {0} changed to {1}",sender.PinNumber, sender.Read());
        }
    }
}


/// <summary>
/// helper class to serialize and deserialize
/// it uses strings as the result of serialization
/// </summary>
public class Serializer
{
    /// <summary>
    /// serializes a object into a string
    /// </summary>
    /// <param name="obj">
    /// the object to serialize
    /// </param>
    /// <returns>
    /// the serialized object
    /// </returns>
    public static string Serialize(object obj)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        using (StreamReader reader = new StreamReader(memoryStream))
        {
            DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
            serializer.WriteObject(memoryStream, obj);
            memoryStream.Position = 0;
            return reader.ReadToEnd();
        }
    }

    /// <summary>
    /// deserializes a an object from a string
    /// </summary>
    /// <param name="xml">
    /// the string to deserialize
    /// </param>
    /// <param name="toType">
    /// the type of the object, that you want to get as a result by deserialization
    /// </param>
    /// <returns>
    /// the deserialized string
    /// </returns>
    public static object Deserialize(string xml, Type toType)
    {
        using (Stream stream = new MemoryStream())
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);
            stream.Write(data, 0, data.Length);
            stream.Position = 0;
            DataContractSerializer deserializer = new DataContractSerializer(toType);
            return deserializer.ReadObject(stream);
        }
    }
}


/// <summary>
/// Unit of transfer by the RequestConnClient Class
/// is only as a container for the two variables methodName and parameter
/// note: this class uses the default contract namespace
/// </summary>
[DataContract]
public class Request
{
    public Request(string methodName, Object parameter)
    {
        this.methodName = methodName;
        this.parameter = parameter;
    }

    [DataMember]
    public string methodName;

    [DataMember]
    public Object parameter;
}