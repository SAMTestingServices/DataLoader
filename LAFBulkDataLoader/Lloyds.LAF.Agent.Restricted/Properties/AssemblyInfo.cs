﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Lloyds.LAF.Agent.Restricted")]
[assembly: AssemblyDescription("Lloyds LAF Agent Restricted")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Lloyds of London")]
[assembly: AssemblyProduct("Lloyds LAF")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("5a75c4c4-637c-45ff-a4cc-190d340436c9")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("2.0.0.0")]
[assembly: AssemblyFileVersion("2.0.0.0")]
[assembly: InternalsVisibleTo("Lloyds.LAF.NET35.Tests")]

// STRONGLY NAMED VERSION OF THE DLL IS GENERATED BY THE FOLLOWING CODE IN THE .CSPROJ FILE.
  //<Target Name="AfterBuild">
  //  <Message Importance="high" Text="Copying required assemblies to output" />
  //  <Exec Command="copy $(TargetDir)Lloyds.LAF.Audit.dll $(ProjectDir)Output\" />
  //  <Exec Command="copy $(TargetDir)Lloyds.LAF.WebServices.Contracts.dll $(ProjectDir)Output\" />
  //  <Message Importance="high" Text="Starting merge of all required assemblies into $(TargetName).Strong.dll" />
  //  <Exec Command="&quot;$(SolutionDir)packages\ilmerge.2.12.0803\ILMerge.exe &quot; /keyfile:&quot;$(SolutionDir)Lloyds.LAF.snk&quot; /targetplatform:v2 /internalize /out:&quot;$(ProjectDir)Output\$(TargetName).Strong.dll&quot; &quot;$(TargetDir)$(TargetFileName)&quot; &quot;$(TargetDir)Dapper.dll&quot; &quot;$(TargetDir)Lloyds.LAF.Audit.dll&quot; &quot;$(TargetDir)log4net.dll&quot;" ContinueOnError="false" />
  //</Target>