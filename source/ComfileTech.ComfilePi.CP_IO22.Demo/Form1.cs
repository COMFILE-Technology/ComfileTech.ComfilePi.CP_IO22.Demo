﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ComfileTech.ComfilePi.CP_IO22.Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
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
                    lamp.State = di.State;
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
            var linkLabel = sender as LinkLabel;
            System.Diagnostics.Process.Start(linkLabel.Text);
        }

        private void _closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
