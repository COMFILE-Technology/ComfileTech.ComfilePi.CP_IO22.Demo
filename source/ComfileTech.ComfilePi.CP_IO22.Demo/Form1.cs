using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace ComfileTech.ComfilePi.CP_IO22.Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var uiContext = SynchronizationContext.Current;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var model = File.ReadAllText("/proc/device-tree/model").Trim();
                if (!model.Contains("Compute Module 4S") && !model.Contains("Compute Module 3"))
                {
                    MessageBox.Show("This application should only be run on a CPi-A, CPi-B, or CPi-S series panel PC.", Text);
                    Environment.Exit(0);
                }

                // Workaround for fullscreen on Raspberry Pi OS Bookworm with Wayland and LabWC
                LocationChanged += (s, e) => { Location = new Point(0, 0); };
            }

            // On Linux, bind the digital input lamps to the IO board's digital inputs
            var lamps = _digitalInputPanel.Controls.OfType<Lamp>().ToArray();

            int index = 0;
            foreach (var input in CP_IO22.Instance.DigitalInputs)
            {
                int i = index;

                var lamp = lamps[i];
                lamp.State = input.State;
                lamp.Text = input.Number.ToString();

                input.StateChanged += (di) =>
                {
                    if (uiContext != null)
                    {
                        uiContext.Post(_ =>
                        {
                            if (!IsDisposed && !lamp.IsDisposed)
                            {
                                lamp.State = di.State;
                            }
                        }, null);
                    }
                    else if (!IsDisposed && !lamp.IsDisposed)
                    {
                        lamp.State = di.State;
                    }
                };

                index++;
            }

            // On Linux, bind the digital output buttons to the IO board's digital outputs
            var buttons = _digitalOutputPanel.Controls.OfType<Button>().ToArray();

            index = 0;
            foreach (var output in CP_IO22.Instance.DigitalOutputs)
            {
                int i = index;

                var button = buttons[i];
                button.State = output.State;
                button.Text = output.Number.ToString();

                button.StateChanged += (s, e) =>
                {
                    output.State = button.State;
                };

                index++;
            }
        }

        private void _repositoryUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (sender is LinkLabel linkLabel)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = linkLabel.Text,
                    UseShellExecute = true
                });
            }
        }

        private void _closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
