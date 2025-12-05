# Updated for 2026 play on YARG:

I Modified the original code so that all buttons are mapped to a MIDI button (in the latest release from 2016 only the keys 
had MIDI events assigned to them, not the dpad/face buttons/overdrive/start/select buttons.)
  
issues unresolved/ to fix:

program doesn't work when keyboard is plugged into a USB hub

# Installation instructions:

Requires

-Windows with .NET 4.5 Framework
-LibUSB filter driver, e.g. wizard from LibUsbDotNet
-A virtual MIDI device, e.g. loopMIDI

If you want to compile it yourself you’ll need LibUsbDotNet and midi-dot-net - the packages on NuGet will do.
Usage

Install the LibUSB filter driver on your keyboard, keep in mind this is per USB port.

    PS3: “vid: 12ba pid:2330”
    Wii: “vid: 1bad pid:3330”

With a virtual MIDI device program running, run RB3KB-USB2MIDI, select your virtual device and click “Doing Nothing” to run.



# RB3-USB
Rock Band 3 PS3/Wii Controller Tools
