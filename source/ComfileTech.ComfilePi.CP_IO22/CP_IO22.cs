using Iot.Device.Ads1115;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Device.I2c;
using System.Runtime.InteropServices;
using System.Threading;

namespace ComfileTech.ComfilePi.CP_IO22
{
    /// <summary>
    /// Represents the CP-IO22 IO board connected to the ComfilePi.
    /// </summary>
    public class CP_IO22
    {
        static CP_IO22()
        {
            Instance = new CP_IO22();
        }

        /// <summary>
        /// The singleton instance of this class.
        /// </summary>
        public static CP_IO22 Instance
        {
            get; private set;
        }

        private CP_IO22()
        {
            // Create the digital inputs
            {
                var list = new List<DigitalInput>();
                for (int i = 4; i <= 13; i++)
                {
                    list.Add(new DigitalInput(i));
                }
                list.Add(new DigitalInput(16));
                DigitalInputs = list.AsReadOnly();
            }

            // Create the digital outputs
            {
                var list = new List<DigitalOutput>();
                for (int i = 17; i <= 27; i++)
                {
                    list.Add(new DigitalOutput(i));
                }
                DigitalOutputs = list.AsReadOnly();
            }
        }

        /// <summary>
        /// The digital inputs for the IO board.
        /// </summary>
        public IReadOnlyList<DigitalInput> DigitalInputs
        {
            get;
        }

        /// <summary>
        /// The digital outputs for the IO board.
        /// </summary>
        public IReadOnlyList<DigitalOutput> DigitalOutputs
        {
            get;
        }
    }
}
