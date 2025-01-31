# Windows 7 Service Pack 2

> [!WARNING]
> This project is far from finished, meaning that bugs are to be expected. If you encounter any bugs, please report them in the issue tracker or in the Discord server. Thank you for your understanding.

Windows 7 Service Pack 2 is a package consisting of updates, tweaks, backported apps and overall enhancements with the goal of providing an effortless way to have a fully updated Windows 7 ISO and enhancing usability on semi-modern machines.

## TODOs

- [x] - All updates until 2020, with the addition of Windows Embedded Standard 7 updates
- [x] - Snipping Tool from Windows 10 version 1507 -> Credits: vxiduu
- [ ] - Paint from Windows 10 version 1703
- [x] - Registry Editor from Windows 10 version 1703 -> Credits: [Aurorarion](https://github.com/AurorarionWin/)
- [ ] - In-place service pack installer
- [x] - Native USB 3.0 and NVMe support -> Credits: Unknown user from MyDigitalLife forums
- [x] - TPM 2.0 support
- [X] - UEFI support in the ISO releases, with EFI GOP support too -> Credits: [Typical/int10h](https://github.com/Typical0), [AveragePC](https://github.com/FaultyFaulty-glitch/)
- [x] - Inclusion of VxKex -> Credits: vxiduu
- [x] - Windows 8 PE in the ISO, with a theme that resembles Windows 7 -> Credits: [ImSwordQueen](https://github.com/ImSwordQueen/) (Windows 7 theme for Windows 8.x)
- [x] - Windows 10 setup engine in the ISO release -> Credits: [Aurorarion](https://github.com/AurorarionWin/) (helping out on the 10 setup engine port)
- [x] - Builtin Webp and FLAC codecs
- [x] - Task Manager from Windows 8 build 7793 -> Credits: [Jevil7452](https://github.com/Jevil7452)
- [ ] - Upscaled graphics in some places
- [x] - Windows Vista/7 PE's boot screen on the ISO -> Credits: Microsoft Corporation and [Tech Stuff](https://github.com/teknixstuff) (Boot8Plus)
- [ ] - Removal of the annoying "Unsupported hardware" prompt 
- [ ] - Better DPI support in aero.msstyles -> Credits: [Vaporvance](https://www.deviantart.com/vaporvance) (high DPI classes from Aero10 that will be ported to Windows 7)
- [ ] - Integration of Microsoft Visual C++ Redistributable AIO -> Credits: Microsoft Corporation (Visual C++) and [abbodi1406](https://github.com/abbodi1406/) (VC++ AIO repack)

## Installation

> [!CAUTION]
> Failure to follow instructions properly will result in a bricked system! We do not take responsibility for this unless this is a legitimate bug in the installer!

> [!IMPORTANT]
> RAID/Intel RST only works on specific systems!

> [!WARNING]
> Both versions of the Windows 7 Service Pack 2 are x64 only! Don't file issues over the lack of 32-bit hardware support as we do not plan to support 32-bit hardware. Consult the FAQ for more information.

### ISO installation

> [!IMPORTANT]
> The ISO releases are for clean installs only. For in-place installs, please use the installer once we release a working version of it.

1. Pick the UEFI or Legacy Boot version, depending on what your system uses
2. Flash it onto a DVD or a USB flash drive
3. Boot from the Windows 7 install media on the machine you wish to install it in
4. Proceed as usual
5. Finish the out of box experience

### In-place intallation

- Download the Windows 7 SP2 installer under the releases of this repository
- Follow the steps in the installer as usual

> [!WARNING]
> If the installer is stuck at a certain percentage (during the Upgrading Windows phase), don't turn off your computer! Doing so will brick your system!

## FAQ

### Q: My system is 32-bit in hardware. Why won't the installer and the ISO run?

A: Supporting both 64-bit and 32-bit Windows would be very time consuming, so we chose to support only 64-bit systems. Please don't file any issues regarding the lack of 32-bit support as we have no interests of making any releases targeting 32-bit hardware.

### Q: Why don't you include ESU updates?

A: ESU updates will be rolled out by Microsoft until 2026, and unfortunately adding ESU updates will be time consuming as it requires us to roll out new releases every time a batch of ESU updates gets rolled out.

### Q: Why VxKex instead of the dotexe1337 Windows 7 Extended Kernel?

A: VxKex is safer than dotexe's Windows 7 Extended Kernel as it relies on external DLLs.

### Q: Does this include custom integrated GPU dri-

A: **NO**

### Q: Why aren't my drives appearing in the Windows Setup screen?

A: If you are using an RAID/RST configuration, it may well likely be that the drivers installed do not work.

## Other credits

- K4sum1: Inspiration for creating Windows 7 SP2
- GMM2003: "Under construction" wallpaper used in Public Beta builds
- Our contributors
- Anyone who reports bugs constantly
