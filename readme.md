# PowerTool

A simple tool for Windows that shows a Dialog Box with a radio button for all power profiles on the system.  If the user clicks OK, powercfg /setactive {guid} is invoked to set the profile as active.

This is useful on machines that support Connected Standby, as when Connected Standby is enabled, a user is not able to change the power profile through the GUI.  If Hyper-V is enabled, the processor throttling settings in a power profile will be honored, and Connected Standby Sleep will work properly.  The GUI to change these settings is not available - a workaround is to disable Connected Standby, create power profiles to your liking, then re-enable Connected Standby.

**Connected Standby machines don't have S3 sleep when Connected Standby is disabled**, so using this tool with Hyper-V and Connected Standby enabled is a workaround to allow setting processor speed while retaining sleep capability.

## Compilation Instructions
```
csc powertool.cs
```

No dependencies outside of standard .net and Windows Forms.  Obviously, it only works on Windows, any version Vista or later.

## Execution Instructions
Run the executable.

## Technical Information
On Windows Vista and later, the power profiles are stored in HKLM\System\CurrentControlSet\Control\Power\User\PowerSchemes".  

A String Value in this key, ActivePowerSchemes, contains the GUID of the active scheme.  

Each scheme gets a subkey named for it's GUID under PowerSchemes.

The Scheme Key contains two String Values, Description, and FriendlyName.

The settings are stored in keys under here.  To map the GUIDS, you can run `powercfg /aliases`, or check this blog post: 

https://blogs.technet.microsoft.com/richardsmith/2007/11/29/powercfg-useful-if-you-know-the-guids/

Within each settings key, there are two DWORD values, ACSettingIndex, and DCSettingIndex, containing the values of the setting.

The settings can also be configured from powercfg.  For example, to configure the maximum CPU speed percentage on the built-in High Performance Profile:

```
powercfg -setacvalueindex 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c 54533251-82be-4824-96c1-47b60b740d00 893dee8e-2bef-41e0-89c6-b55d0929964c 100

powercfg -setdcvalueindex 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c 54533251-82be-4824-96c1-47b60b740d00 893dee8e-2bef-41e0-89c6-b55d0929964c 100
```

The GUID of the built-in high performance profile is 
`8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c`, running `powercfg /aliases` shows us:
```
54533251-82be-4824-96c1-47b60b740d00  SUB_PROCESSOR
bc5038f7-23e0-4960-96da-33abaf5935ec    PROCTHROTTLEMAX
893dee8e-2bef-41e0-89c6-b55d0929964c    PROCTHROTTLEMIN
94d3a615-a899-4ac5-ae2b-e4d8f634367f    SYSCOOLPOL
```

## Enable/Disable Connected Standby
HKLM\System\CurrentControlSet\Control\Power\CsEnabled

- 1 = Enabled
- 0 = Disabled

## License
This work has been released into the public domain by the copyright holder. This applies worldwide.

In case this is not legally possible:

The copyright holder grants any entity the right to use this work for any purpose, without any conditions, unless such conditions are required by law.
