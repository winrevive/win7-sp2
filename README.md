# Windows 7 Service Pack 2
> [!WARNING]
> This project is far from finished, meaning that bugs are to be expected. If you encounter any bugs, please report them in the issue tracker or in the Discord server. Thank you for your understanding. 

Windows 7 Service Pack 2 is a package consisting of updates, tweaks, backported apps and overall enhancements with the goal of providing an effortless way to have a fully updated Windows 7 ISO and enhancing usability on semi-modern machines.
## Planned features
- All updates until 2020, with the addition of Windows Embedded Standard 7 updates
- Snipping Tool from Windows 10 version 1507
- Paint from Windows 10 version 1703
- Registry Editor from Windows 10 version 1703
- In-place service pack installer based on Dominic Hayes' 8to7 backend
- Generic USB 3.0 and NVMe drivers
- UEFI support in the ISO releases (if you download the UEFI-specific version)
- Inclusion of VxKex
- Windows 8 PE in the ISO, with a theme that resembles Windows 7
- Windows 10 setup engine in the ISO release
- Upscaled graphics in some places
- Windows Vista/7 PE's boot screen on the ISO
- Removal of the annoying "Unsupported hardware" prompt
## Installation
> [!CAUTION]
> Failure to follow instructions properly will result in a bricked system! We do not take responsibility for this unless this is a legitimate bug in the installer!

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
## Credits
### Developers
- Alureon
- h4rl
### Honorable mentions
- Microsoft Corporation: Windows 7 operating system. *Windows 7 Service Pack 2 is by no means affiliated with Microsoft Corporation.*
- K4sum1: Inspiration (this project is inspired from the Windows 7 Updated ISO by K4sum1)
- manatails: UEFISeven
- vxiduu: VxKex and SnippingToolEx
- Aurorarion: Registry Editor and Paint backports
