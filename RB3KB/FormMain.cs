using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Midi;

namespace RB3KB_USB2MIDI {
    public partial class FormMain : Form {

        #region Dictionary Program
        //Numbers are +1 of the enum in the Midi library.
        private static Dictionary<int, string> dictionaryProgram = new Dictionary<int, string>() {
            { 1, "Acoustic Grand Piano" },
            { 2, "Bright Acoustic Piano" },
            { 3, "Electric Grand Piano" },
            { 4, "Honky-tonk Piano" },
            { 5, "Electric Piano 1" },
            { 6, "Electric Piano 2" },
            { 7, "Harpsichord" },
            { 8, "Clavi" },
            { 9, "Celesta" },
            { 10, "Glockenspiel" },
            { 11, "Music Box" },
            { 12, "Vibraphone" },
            { 13, "Marimba" },
            { 14, "Xylophone" },
            { 15, "Tubular Bells" },
            { 16, "Dulcimer" },
            { 17, "Drawbar Organ" },
            { 18, "Percussive Organ" },
            { 19, "Rock Organ" },
            { 20, "Church Organ" },
            { 21, "Reed Organ" },
            { 22, "Accordion" },
            { 23, "Harmonica" },
            { 24, "Tango Accordion" },
            { 25, "Acoustic Guitar (Nylon)" },
            { 26, "Acoustic Guitar (Steel)" },
            { 27, "Electric Guitar (Jazz)" },
            { 28, "Electric Guitar (Clean)" },
            { 29, "Electric Guitar (Muted)" },
            { 30, "Overdriven Guitar" },
            { 31, "Distortion Guitar" },
            { 32, "Guitar harmonics" },
            { 33, "Acoustic Bass" },
            { 34, "Electric Bass (Finger)" },
            { 35, "Electric Bass (Pick)" },
            { 36, "Fretless Bass" },
            { 37, "Slap Bass 1" },
            { 38, "Slap Bass 2" },
            { 39, "Synth Bass 1" },
            { 40, "Synth Bass 2" },
            { 41, "Violin" },
            { 42, "Viola" },
            { 43, "Cello" },
            { 44, "Contrabass" },
            { 45, "Tremolo Strings" },
            { 46, "Pizzicato Strings" },
            { 47, "Orchestral Harp" },
            { 48, "Timpani" },
            { 49, "String Ensemble 1" },
            { 50, "String Ensemble 2" },
            { 51, "SynthStrings 1" },
            { 52, "SynthStrings 2" },
            { 53, "Choir Aahs" },
            { 54, "Voice Oohs" },
            { 55, "Synth Voice" },
            { 56, "Orchestra Hit" },
            { 57, "Trumpet" },
            { 58, "Trombone" },
            { 59, "Tuba" },
            { 60, "Muted Trumpet" },
            { 61, "French Horn" },
            { 62, "Brass Section" },
            { 63, "SynthBrass 1" },
            { 64, "SynthBrass 2" },
            { 65, "Soprano Sax" },
            { 66, "Alto Sax" },
            { 67, "Tenor Sax" },
            { 68, "Baritone Sax" },
            { 69, "Oboe" },
            { 70, "English Horn" },
            { 71, "Bassoon" },
            { 72, "Clarinet" },
            { 73, "Piccolo" },
            { 74, "Flute" },
            { 75, "Recorder" },
            { 76, "Pan Flute" },
            { 77, "Blown Bottle" },
            { 78, "Shakuhachi" },
            { 79, "Whistle" },
            { 80, "Ocarina" },
            { 81, "Lead 1 (Square)" },
            { 82, "Lead 2 (Sawtooth)" },
            { 83, "Lead 3 (Calliope)" },
            { 84, "Lead 4 (Chiff)" },
            { 85, "Lead 5 (Charang)" },
            { 86, "Lead 6 (Voice)" },
            { 87, "Lead 7 (Fifths)" },
            { 88, "Lead 8 (Bass + Lead)" },
            { 89, "Pad 1 (New Age)" },
            { 90, "Pad 2 (Warm)" },
            { 91, "Pad 3 (Polysynth)" },
            { 92, "Pad 4 (Choir)" },
            { 93, "Pad 5 (Bowed)" },
            { 94, "Pad 6 (Metallic)" },
            { 95, "Pad 7 (Halo)" },
            { 96, "Pad 8 (Sweep)" },
            { 97, "FX 1 (Rain)" },
            { 98, "FX 2 (Soundtrack)" },
            { 99, "FX 3 (Crystal)" },
            { 100, "FX 4 (Atmosphere)" },
            { 101, "FX 5 (Brightness)" },
            { 102, "FX 6 (Goblins)" },
            { 103, "FX 7 (Echoes)" },
            { 104, "FX 8 (Sci-Fi)" },
            { 105, "Sitar" },
            { 106, "Banjo" },
            { 107, "Shamisen" },
            { 108, "Koto" },
            { 109, "Kalimba" },
            { 110, "Bag pipe" },
            { 111, "Fiddle" },
            { 112, "Shanai" },
            { 113, "Tinkle Bell" },
            { 114, "Agogo" },
            { 115, "Steel Drums" },
            { 116, "Woodblock" },
            { 117, "Taiko Drum" },
            { 118, "Melodic Tom" },
            { 119, "Synth Drum" },
            { 120, "Reverse Cymbal" },
            { 121, "Guitar Fret Noise" },
            { 122, "Breath Noise" },
            { 123, "Seashore" },
            { 124, "Bird Tweet" },
            { 125, "Telephone Ring" },
            { 126, "Helicopter" },
            { 127, "Applause" },
            { 128, "Gunshot" },
        };
        #endregion

        private static OutputDevice MIDI_OUT = null;
        private static USBKB keyboard = null;

        enum PedalMode { Expression, ChannelVolume, FootController };

        Pitch[] map_drum = new Pitch[]{
            Pitch.B1, Pitch.C2, Pitch.D2, Pitch.E2, Pitch.F2, Pitch.B2, Pitch.D3, Pitch.FSharp2, Pitch.ASharp2, Pitch.CSharp3, Pitch.DSharp3, Pitch.F3
        };

        Thread thread = null;
        private int Octave = 4;
        private int Program = 0;
        private bool DrumMapping = false;
        private bool SwapModulationPitchBend = false;
        private bool LEDAnimations = false;
        private bool ModIgnoreZero = false;
        private bool PitchIgnoreZero = false;

        // --- NEW: Extra button MIDI mapping configuration ---
        // You can change these MIDI note numbers if you want different notes.
        private const int IDX_DPAD_UP = 1000;
        private const int IDX_DPAD_DOWN = 1001;
        private const int IDX_DPAD_LEFT = 1002;
        private const int IDX_DPAD_RIGHT = 1003;
        private const int IDX_TRIANGLE = 1004;
        private const int IDX_SQUARE = 1005;
        private const int IDX_CROSS = 1006;
        private const int IDX_CIRCLE = 1007;
        private const int IDX_START = 1008;
        private const int IDX_SELECT = 1009;
        private const int IDX_OVERDRIVE = 1010;

        // MIDI note numbers for those indices (changeable)
        private static Dictionary<int, Pitch> ExtraButtonToPitch = new Dictionary<int, Pitch>() {
            { IDX_DPAD_UP, (Pitch)96 },     // DPad Up -> MIDI note 96
            { IDX_DPAD_DOWN, (Pitch)97 },   // DPad Down -> MIDI note 97
            { IDX_DPAD_LEFT, (Pitch)98 },   // DPad Left -> MIDI note 98
            { IDX_DPAD_RIGHT, (Pitch)99 },  // DPad Right -> MIDI note 99
            { IDX_TRIANGLE, (Pitch)100 },   // Triangle -> MIDI note 100
            { IDX_SQUARE, (Pitch)101 },     // Square -> MIDI note 101
            { IDX_CROSS, (Pitch)102 },      // Cross -> MIDI note 102
            { IDX_CIRCLE, (Pitch)103 },     // Circle -> MIDI note 103
            { IDX_START, (Pitch)104 },      // Start -> MIDI note 104
            { IDX_SELECT, (Pitch)105 },     // Select -> MIDI note 105
            { IDX_OVERDRIVE, (Pitch)106 }   // Overdrive -> MIDI note 106
        };

        // fixed velocity for extra buttons (change if you prefer)
        private const int EXTRA_BUTTON_VELOCITY = 100;

        #region Dpad (Drum / Swap Mod and Pitchbend)
        private void chkDrumMapping_CheckedChanged(object sender, EventArgs e) {
            DrumMapping = chkDrumMapping.Checked;
        }

        public void SetDrumMapping(bool set) {
            DrumMapping = set;

            this.Invoke((MethodInvoker)delegate {
                chkDrumMapping.Checked = DrumMapping;
            });
        }

        private void chkSwapModulationPitchBand_CheckedChanged(object sender, EventArgs e) {
            SwapModulationPitchBend = chkSwapModulationPitchBand.Checked;
        }

        public void SetModPitchBend(bool set) {
            SwapModulationPitchBend = set;

            this.Invoke((MethodInvoker)delegate {
                chkSwapModulationPitchBand.Checked = SwapModulationPitchBend;
            });
        }
        #endregion

        #region Set Octave
        public void SetOctave(int octave) {
            octave = Math.Min(octave, 8);
            octave = Math.Max(octave, 0);
            Octave = octave;

            this.Invoke((MethodInvoker)delegate {
                txtOctave.Text = Octave.ToString();
            });
        }

        private void txtOctave_ValueChanged(object sender, EventArgs e) {
            int octave;
            if (Int32.TryParse(txtOctave.Text, out octave))
                SetOctave(octave);
        }
        #endregion

        #region Set Program / Instrument
        public void SetProgram(int program) {
            program = Math.Min(program, 127);
            program = Math.Max(program, 0);
            Program = program;

            if(MIDI_OUT != null && MIDI_OUT.IsOpen)
                MIDI_OUT.SendProgramChange(Channel.Channel1, (Instrument)Program);

            this.Invoke((MethodInvoker)delegate {
                txtInstrumentNum.Text = (Program + 1).ToString();
                ddlInstrument.SelectedItem = dictionaryProgram[Program + 1];
            });
        }

        private void ddlInstrument_SelectedIndexChanged(object sender, EventArgs e) {
            SetProgram(dictionaryProgram.FirstOrDefault(x => x.Value == ddlInstrument.SelectedItem.ToString()).Key - 1);
        }

        private void txtInstrumentNum_ValueChanged(object sender, EventArgs e) {
            int program;
            if (Int32.TryParse(txtInstrumentNum.Text, out program))
                SetProgram(program - 1);
        }
        #endregion

        public FormMain() {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e) {
            foreach (OutputDevice device in OutputDevice.InstalledDevices) {
                ddlMidiOut.Items.Add(device.Name);

                if (device.Name == Properties.Settings.Default.LastUsedMidiOut)
                    ddlMidiOut.SelectedIndex = ddlMidiOut.Items.Count - 1;
            }

            foreach(KeyValuePair<int, string> pair in dictionaryProgram)
                ddlInstrument.Items.Add(pair.Value);

            SetOctave(4);
            SetProgram(0);

            LEDAnimations = chkLEDAnimations.Checked = Properties.Settings.Default.LastUsedLEDAnimations;
        }

        private void Start() {
            MIDI_OUT.Open();
            keyboard = new USBKB();
        }

        private void End() {
            if(keyboard != null)
                keyboard.Close();

            if(MIDI_OUT != null && MIDI_OUT.IsOpen)
                MIDI_OUT.Close();
        }

        private void USB2MIDI() {
            if (LEDAnimations) {
                if(DrumMapping)
                    keyboard.SetLED(8, true);
                else
                    keyboard.SetLED(1, true);
            }

            bool holdPanic = false;
            bool holdPedal = false;

            MIDI_OUT.SendProgramChange(Channel.Channel1, (Instrument)Program);

            Dictionary<int, Tuple<Channel, Pitch>> HeldKey = new Dictionary<int, Tuple<Channel, Pitch>>();

            KBData previous = keyboard.Read();
            while (true) {
                KBData data = keyboard.Read();

                #region Panic: All Notes Off (Select, Home and Start together)
                if(data.SelectMinus && data.Home && data.StartPlus && !holdPanic) {
                    holdPanic = true;
                    int max = (8 * 12) + 25;
                    for (int i = 0; i < max; i++)
                        MIDI_OUT.SendNoteOff(Channel.Channel1, (Pitch)i, 0);
                    for(int i = 0; i < map_drum.Length; i++)
                        MIDI_OUT.SendNoteOff(Channel.Channel10, map_drum[i], 0);
                } else if (!data.SelectMinus && !data.Home && !data.StartPlus && holdPanic) {
                    holdPanic = false;
                }
                #endregion

                #region Pedal (unchanged)
                if (data.Pedal && !holdPedal) {
                    holdPedal = true;
                    MIDI_OUT.SendControlChange(Channel.Channel1, Midi.Control.SustainPedal, 127);
                } else if (!data.Pedal && holdPedal) {
                    MIDI_OUT.SendControlChange(Channel.Channel1, Midi.Control.SustainPedal, 0);
                    holdPedal = false;
                }
                #endregion

                #region Keys (unchanged)
                for (int i = 0; i < data.Key.Length; i++) {
                    if (data.Key[i] && !previous.Key[i]) {
                        int pitch = (Octave * 12) + i;

                        //Probably only correct when only pressing keys further right of already pressed keys.
                        int velocity = 80;
                        for (int v = data.Velocity.Length - 1; v >= 0; v--) {
                            if (data.Velocity[v] != 0) {
                                velocity = data.Velocity[v];
                                break;
                            }
                        }

                        if (i < map_drum.Length && DrumMapping) {
                            HeldKey.Add(i, new Tuple<Channel, Pitch>(Channel.Channel10, map_drum[i]));
                            MIDI_OUT.SendNoteOn(Channel.Channel10, map_drum[i], velocity);
                        } else {
                            HeldKey.Add(i, new Tuple<Channel, Pitch>(Channel.Channel1, (Pitch)pitch));
                            MIDI_OUT.SendNoteOn(Channel.Channel1, (Pitch)pitch, velocity);
                        }
                    } else if (!data.Key[i] && previous.Key[i] && HeldKey.ContainsKey(i)) {
                        MIDI_OUT.SendNoteOff(HeldKey[i].Item1, HeldKey[i].Item2, 0);
                        HeldKey.Remove(i);
                    }
                }
                #endregion

                #region Extra Buttons -> MIDI Notes (DPad directions, Triangle, Square, Cross, Circle, Start, Select, Overdrive)
                // For each button: on rising edge -> NoteOn; on falling edge -> NoteOff.
                void HandleExtraButton(bool currentState, bool prevState, int idx) {
                    if (currentState && !prevState) {
                        // Note On
                        if (!HeldKey.ContainsKey(idx)) {
                            Pitch p = ExtraButtonToPitch[idx];
                            HeldKey.Add(idx, new Tuple<Channel, Pitch>(Channel.Channel1, p));
                            MIDI_OUT.SendNoteOn(Channel.Channel1, p, EXTRA_BUTTON_VELOCITY);
                        }
                    } else if (!currentState && prevState) {
                        // Note Off
                        if (HeldKey.ContainsKey(idx)) {
                            MIDI_OUT.SendNoteOff(HeldKey[idx].Item1, HeldKey[idx].Item2, 0);
                            HeldKey.Remove(idx);
                        }
                    }
                }

                // DPad: data.Dpad codes: 0=Up, 2=Right, 4=Down, 6=Left, 8=None
                HandleExtraButton(data.Dpad == 0, previous.Dpad == 0, IDX_DPAD_UP);
                HandleExtraButton(data.Dpad == 4, previous.Dpad == 4, IDX_DPAD_DOWN);
                HandleExtraButton(data.Dpad == 6, previous.Dpad == 6, IDX_DPAD_LEFT);
                HandleExtraButton(data.Dpad == 2, previous.Dpad == 2, IDX_DPAD_RIGHT);

                // Face / function buttons
                HandleExtraButton(data.Triangle2, previous.Triangle2, IDX_TRIANGLE);
                HandleExtraButton(data.Square1, previous.Square1, IDX_SQUARE);
                HandleExtraButton(data.CrossA, previous.CrossA, IDX_CROSS);
                HandleExtraButton(data.CircleB, previous.CircleB, IDX_CIRCLE);

                // Start / Select / Overdrive
                HandleExtraButton(data.StartPlus, previous.StartPlus, IDX_START);
                HandleExtraButton(data.SelectMinus, previous.SelectMinus, IDX_SELECT);
                HandleExtraButton(data.Overdrive, previous.Overdrive, IDX_OVERDRIVE);
                #endregion

                #region Slider (now independent of Overdrive) -> Modulation or Pitch Bend based on SwapModulationPitchBend
                if (data.Slider != previous.Slider) {
                    if (SwapModulationPitchBend) {
                        // Use slider as pitch bend
                        Console.WriteLine(data.Slider);
                        if(data.Slider == 0 && PitchIgnoreZero) {
                            // Ignore
                        } else if (data.Slider == 50 || data.Slider == 0)
                            MIDI_OUT.SendPitchBend(Channel.Channel1, 8192);
                        else
                            MIDI_OUT.SendPitchBend(Channel.Channel1, (data.Slider * 140));
                    } else {
                        // Use slider as modulation wheel
                        if (data.Slider == 0 && ModIgnoreZero) {
                            // Ignore
                        } else
                            MIDI_OUT.SendControlChange(Channel.Channel1, Midi.Control.ModulationWheel, data.Slider);
                    }
                }
                #endregion

                previous = data;
            }
        }

        private void ddlMidiOut_SelectedIndexChanged(object sender, EventArgs e) {
            MIDI_OUT = OutputDevice.InstalledDevices[ddlMidiOut.SelectedIndex];

            Properties.Settings.Default.LastUsedMidiOut = ddlMidiOut.SelectedItem.ToString();
            Properties.Settings.Default.Save();
        }

        private void btnRunLoop_Click(object sender, EventArgs e) {
            if (thread == null || !thread.IsAlive) {
                if (MIDI_OUT != null) {
                    Start();

                    if (keyboard.CanUse()) {
                        thread = new Thread(() => USB2MIDI());
                        thread.Start();

                        ddlMidiOut.Enabled = false;
                        btnRunLoop.Text = "Running";
                    } else {
                        End();
                        ddlMidiOut.Enabled = true;
                        btnRunLoop.Text = "Keyboard not found - missing filter?";
                    }
                }
            } else {
                thread.Abort();
                End();

                ddlMidiOut.Enabled = true;
                btnRunLoop.Text = "Doing Nothing";
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e) {
            if(thread != null)
                thread.Abort();
            End();
        }

        private void chkLEDAnimations_CheckedChanged(object sender, EventArgs e) {
            LEDAnimations = chkLEDAnimations.Checked;

            Properties.Settings.Default.LastUsedLEDAnimations = LEDAnimations;
            Properties.Settings.Default.Save();
        }

        private void chkModPitchIgnoreZero_CheckedChanged(object sender, EventArgs e) {
            ModIgnoreZero = chkModIgnoreZero.Checked;
        }

        private void chkPitchIgnoreZero_CheckedChanged(object sender, EventArgs e) {
            PitchIgnoreZero = chkPitchIgnoreZero.Checked;
        }
    }
}
