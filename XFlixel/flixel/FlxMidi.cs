using System;
using Midi;
using System.Threading;
using System.Collections.Generic;

namespace SuperHorrorFactory
{
    public class FlxMidi
    {
        public FlxMidi()
        { }

        public class Summarizer
        {
            public Summarizer(InputDevice inputDevice)
            {
                this.inputDevice = inputDevice;
                pitchesPressed = new Dictionary<Pitch, bool>();
                inputDevice.NoteOn += new InputDevice.NoteOnHandler(this.NoteOn);
                inputDevice.NoteOff += new InputDevice.NoteOffHandler(this.NoteOff);
                inputDevice.ControlChange += new InputDevice.ControlChangeHandler(this.ChangeControl);
                inputDevice.PitchBend += new InputDevice.PitchBendHandler(this.PitchBend);
                inputDevice.ProgramChange += new InputDevice.ProgramChangeHandler(this.ProgramChange);


                //PrintStatus();
            }

            private void PrintStatus()
            {

                // Print the currently pressed notes.
                List<Pitch> pitches = new List<Pitch>(pitchesPressed.Keys);
                pitches.Sort();
                Console.Write("Notes: ");
                for (int i = 0; i < pitches.Count; ++i)
                {
                    Pitch pitch = pitches[i];
                    if (i > 0)
                    {
                        Console.Write(", ");
                    }
                    Console.Write("{0}", pitch.NotePreferringSharps());
                    if (pitch.NotePreferringSharps() != pitch.NotePreferringFlats())
                    {
                        Console.Write(" or {0}", pitch.NotePreferringFlats());
                    }
                }
                Console.WriteLine();
                // Print the currently held down chord.
                List<Chord> chords = Chord.FindMatchingChords(pitches);
                Console.Write("Chords: ");
                for (int i = 0; i < chords.Count; ++i)
                {
                    Chord chord = chords[i];
                    if (i > 0)
                    {
                        Console.Write(", ");
                    }
                    Console.Write("{0}", chord);
                }
                Console.WriteLine();
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
                    Console.WriteLine("NoteOn {0} {1} {2} {3}", msg.Pitch, msg.Velocity, msg.Channel, msg.Time);
                    //pitchesPressed[msg.Pitch] = true;
                    //PrintStatus();
                }
            }

            public void NoteOff(NoteOffMessage msg)
            {
                lock (this)
                {
                    pitchesPressed.Remove(msg.Pitch);
                    //PrintStatus();
                }
            }

            private InputDevice inputDevice;
            private Dictionary<Pitch, bool> pitchesPressed;
        }

        public void Run()
        {
            InputDevice inputDevice = InputDevice.InstalledDevices[1];
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

            Summarizer summarizer = new Summarizer(inputDevice);

        }

        public void Close()
        {
            InputDevice inputDevice = InputDevice.InstalledDevices[1];
            inputDevice.StopReceiving();
            inputDevice.Close();
            inputDevice.RemoveAllEventHandlers();
        }
    }
}
