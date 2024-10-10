# Windows 7 Service Pack 2
Windows 7 Service Pack 2 is a package consisting of updates, tweaks, backported apps and overall enhancements with the goal of providing an effortless updated Windows 7 installation and making use for
## Planned additions
- All updates until 2020, with the addition of Windows Embedded 7 updates
- Snipping tool from Windows 10 version 1507
- Paint from Windows 10 version 1703
- Registry Editor from Windows 10 version 1703
- In-place service pack installer based on Dominic Hayes' 8to7 backend
- Generic USB 3.0 and NVMe included by default in the ISO version
- UEFI support in the ISO version (if you select the version with UEFI)
- VxKex, for modern software support
- Windows 8 PE in the ISO, with a theme that resembles Windows 7
- Windows 10 setup engine in the ISO version
- Upscaled graphics in some places
- Replica of Windows 7's bootscreen on the PE in the ISO
- Removal of the annoyinhg "Unsupported hardware" prompt
## Installation (ISO version)
1. Pick the UEFI or Legacy Boot version, depending on what your system uses
2. Burn it onto a DVD or a USB flash drive
3. Boot from the Windows 7 install media on the machine you wish to install it in
4. Proceed as usual
5. Finish the out of box experience
## Installation (installer)
- Download the Windows 7 SP2 installer under the releases of this repository
- Follow the steps in the installer as usual
> [!WARNING]
> If the installer is stuck at a certain percentage, don't turn off your computer! Doing so will brick your system!
## FAQ
### Q: Why don't you include ESU updates?
A: ESU updates will be rolled out by Microsoft until 2026, and unfortunately adding ESU updates will be time consuming as ti requires us to make new releases every time a batch of ESU updates gets rolled out.
### Q: Why VxKex instead of the dotexe1337 Windows 7 Extended Kernel?
Q: VxKex is safer than dotexe's extended kernel as it relies on external DLLs.
### Q: Does this include custom integrated GPU dri-
A: **NO**
## Credits
- Microsoft Corporation: Windows 7 operating system
- K4sum1: Base ISO used for the ISO release
- Dominic Hayes: 8to7 backend, which the installer is based off of
- manatails: UEFISeven
- vxiduu: VxKex and backport of the snipping tool
- Aurorarion: Registry Editor and Paint backports
