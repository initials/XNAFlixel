using System;
using Midi;
using System.Threading;
using System.Collections.Generic;

namespace org.flixel
{
    public class FlxMidi
    {
        public InputDevice inputDevice;

        public FlxMidi()
        {
            inputDevice = InputDevice.InstalledDevices[1];

            //this.inputDevice = inputDevice;
            //pitchesPressed = new Dictionary<Pitch, bool>();
            inputDevice.NoteOn += new InputDevice.NoteOnHandler(this.NoteOn);
            inputDevice.NoteOff += new InputDevice.NoteOffHandler(this.NoteOff);
            inputDevice.ControlChange += new InputDevice.ControlChangeHandler(this.ChangeControl);
            inputDevice.PitchBend += new InputDevice.PitchBendHandler(this.PitchBend);
            inputDevice.ProgramChange += new InputDevice.ProgramChangeHandler(this.ProgramChange);
        }

        public void ProgramChange(ProgramChangeMessage msg)
        {
            lock (this)
            {
                Console.WriteLine("Program Change  {0} {1} {2} ", msg.Channel, msg.Instrument, msg.Device);

            }
        }

        public void PitchBend(PitchBendMessage msg)
        {
            lock (this)
            {
                Console.WriteLine("PitchBend " + msg.Channel);

            }
        }

        public void ChangeControl(ControlChangeMessage msg)
        {
            lock (this)
            {
                Console.WriteLine("ChangeControl {0} {1} {2} {3} ", msg.Channel, msg.Control, msg.Time, msg.Value);

            }
        }

        public void NoteOn(NoteOnMessage msg)
        {
            lock (this)
            {
                Console.WriteLine("NoteOn {0} {1} {2} {3} {4} ", msg.Pitch, msg.Velocity, msg.Pitch.Octave(), msg.Pitch.PositionInOctave(), (int)msg.Pitch);
                //pitchesPressed[msg.Pitch] = true;
                //PrintStatus();
            }
        }

        public void NoteOff(NoteOffMessage msg)
        {
            lock (this)
            {
                //pitchesPressed.Remove(msg.Pitch);
                //PrintStatus();
            }
        }

        public void Run()
        {
            
            if (inputDevice.IsOpen)
            {
                return;
            }
            if (inputDevice == null)
            {
                Console.WriteLine("No input devices, so can't run this example.");
                return;
            }
            inputDevice.Open();
            inputDevice.StartReceiving(null);

            //Summarizer summarizer = new Summarizer(inputDevice);

        }

        public void Close()
        {
            if (inputDevice.IsReceiving)
            {
                inputDevice.StopReceiving();
                inputDevice.Close();
                inputDevice.RemoveAllEventHandlers();
            }

        }
    }
}
