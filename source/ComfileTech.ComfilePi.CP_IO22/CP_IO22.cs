using System.Collections.Generic;

namespace ComfileTech.ComfilePi.CP_IO22
{
    /// <summary>
    /// Represents the CP-IO22 IO board connected to the ComfilePi.
    /// </summary>
    public class CP_IO22 : System.IDisposable
    {
        /// <summary>
        /// The singleton instance of this class.
        /// </summary>
        public static CP_IO22 Instance { get; } = new CP_IO22();

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

        bool _disposed;

        /// <summary>
        /// Releases the GPIO resources used by the IO board.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            DigitalInput.DisposeGpioController();
            DigitalOutput.DisposeGpioController();

            _disposed = true;
            System.GC.SuppressFinalize(this);
        }
    }
}
