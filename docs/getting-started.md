
# Getting started the IL2C

## How works the IL2C

The IL2C will translate your C# (or another .NET based) code to the C language source code.
It bites the .NET assembly (*.dll or *.exe) and will produce multiple C language source code files. It illustrates the graph:

![The IL2C building structure](../images/overview.png)

First half stage, we can automate building process with the NuGet package nemed [IL2C.Build](https://www.nuget.org/packages/IL2C.Build).

Then we'll get the translated C language source code files, so we need compile and get the target binary at the second half stage manually.

This document introduces generally usage for the IL2C. Try it!

## 1. Step by step for first trying

We know about better first step, it's the "Hello world." [(The completed projects contain the IL2C repository. You can refer it.)](https://github.com/kekyo/IL2C/tree/master/samples/GettingStartedIL2C)

Note: Chapter 1 is absolutely basic topics for how to enable IL2C from scratch (and I recommend you know about it). But if you want to only simple way, go to chapter 2 (2-0).

### 1-1. Create new C# library project with "net46" or "netcoreapp3.1" platform using Visual Studio 2019.

You can choice ether old (.NET Framework style) project or new (.NET Core/Standard style) project. For example, this project named "GettingStartedIL2C" on .NET Core 3.1:

![.NET Core console app](../images/tutorial111.png)
![.NET Core console app](../images/tutorial112.png)

### 1-2. Add the [IL2C.Build NuGet package](https://www.nuget.org/packages/IL2C.Build) using the "Package Manager Console" or "NuGet package manager dialog."

![The IL2C.Build NuGet package](../images/tutorial12.png)

Shows example csproj file:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp3.1</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="IL2C.Build" Version="0.4.70" />
  </ItemGroup>
</Project>
```

### 1-3. Write code fragment:

```csharp
using System;

namespace GettingStartedIL2C
{
    public static class Program
    {
        public static void Main() =>
            Console.WriteLine("Hello world with IL2C!");
    }
}
```

### 1-4. Build project and run.

![The hello world result on the pure .NET runtime.](../images/tutorial13.png)

You'll see print it inside the console window and you'll definitely say: "Is not that natural? It's running on the pure .NET runtime." Yes it is. But you already have the translated C source code files. Check it up:

```
[The project directory]
|
|   GettingStartedIL2C.csproj
|   Program.cs
\---bin
    \---Debug
        \---netcoreapp3.1
            |   GettingStartedIL2C.deps.json
            |   GettingStartedIL2C.dll           // It's standard .NET executable assembly (we saw results)
            |   GettingStartedIL2C.exe           // .NET Core 3.1 bootstrapper
            |   GettingStartedIL2C.pdb
            |   GettingStartedIL2C.runtimeconfig.dev.json
            |   GettingStartedIL2C.runtimeconfig.json
            |   IL2C.Interop.dll                 // (doesn't use it now)
            \---IL2C                             // The IL2C translated C language source codes
                +---include
                |   |   GettingStartedIL2C.h
                |   |
                |   \---GettingStartedIL2C
                |       \---GettingStartedIL2C
                |               Program.h
                \---src
                    |   GettingStartedIL2C_internal.c
                    |   GettingStartedIL2C_internal.h
                    |
                    \---GettingStartedIL2C
                        \---GettingStartedIL2C
                                Program.c
```

You can find the "IL2C" subdirectory descendant bin directory. It's default output directory for IL2C. These files are the C source codes and headers, indicate extensions "*.c" or "*.h".

The "src" directory contains translated method body code, and the "include" directory contains public/internal types and methods definitions. We can build native binary both using these files and the IL2C runtime library.

"IL2C.Runtime" directory at the IL2C repository root contains runtime library. This tutorial compiles with Visual C++ (But the IL2C is aiming for targeting multiple platform. You can also choice the gcc.)

### 1-5. Add Visual C++ console application into your solution.

This is the second half stage. For example, this project named "GettingStartedIL2CMain".

![Add the Visual C++ project](../images/tutorial141.png)
![Add the Visual C++ project](../images/tutorial142.png)

(The Visual C++ wizard generates a lot of helper code fragments. We can remove it, but I don't it in this tutorial because I'll focus for the IL2C usage.)

Open project file (GettingStartedIL2CMain.vcxproj) and edit directly below (because it's easy way. Edit after exit Visual Studio):

1. Add "AdditionalIncludeDirectories" and "PrecompiledHeader" property each descendant ItemDefinitionGroup/ClCompile elements. We have to do it because the VC++ compiler requires referring both IL2C runtime header files and translated header files. The "PrecompiledHeader" property requires because the translated files target to the "C language" and the feature can use only C++ language. (It's Visual C++ limitation.)

```xml
<ItemDefinitionGroup Condition="..."> <!-- each conditions --> 
  <ClCompile>
    <!-- ... -->
    <!-- Added below -->
    <AdditionalIncludeDirectories>$(ProjectDir)../../../IL2C.Runtime/include;$(ProjectDir)../../../IL2C.Runtime/src;$(ProjectDir)../GettingStartedIL2C/bin/$(Configuration)/netcoreapp3.1/IL2C/include;$(ProjectDir)../GettingStartedIL2C/bin/$(Configuration)/netcoreapp3.1/IL2C/src;%(AdditionalIncludeDirectories)</AdditionalIncludeDirectories>
    <PrecompiledHeader>NotUsing</PrecompiledHeader>
  </ClCompile>
  <!-- ... -->
</ItemDefinitionGroup>
```

You have to adjust valid directory paths for your environments. You see trivial path for `$(Configuration)`, it expands to flexible searching both target configuration (ex: Debug). `netcoreapp3.1` is target framework moniker, you have to replace it your environment.

2. Add referring the IL2C runtime and translated files nearly exist "ClCompile" ItemGroup element:

```xml
<!-- Added this ItemGroup below -->
<ItemGroup>
  <ClInclude Include="$(ProjectDir)../../../IL2C.Runtime/include/**/*.h" />
  <ClInclude Include="$(ProjectDir)../../../IL2C.Runtime/src/**/*.h" />
  <ClInclude Include="$(ProjectDir)../GettingStartedIL2C/bin/$(Configuration)/netcoreapp3.1/IL2C/include/**/*.h" />
  <ClInclude Include="$(ProjectDir)../GettingStartedIL2C/bin/$(Configuration)/netcoreapp3.1/IL2C/src/**/*.h" />
</ItemGroup>

<ItemGroup>
  <!-- Changed extension .cpp to .c -->
  <ClCompile Include="GettingStartedIL2CMain.c" />
  <!-- Added below -->
  <ClCompile Include="$(ProjectDir)../../../IL2C.Runtime/src/**/*.c" />
  <ClCompile Include="$(ProjectDir)../GettingStartedIL2C/bin/$(Configuration)/netcoreapp3.1/IL2C/src/**/*.c" />
</ItemGroup>
```

You have to adjust valid paths for your environments too.

3. Rename a file "GettingStartedIL2CMain.cpp" to "GettingStartedIL2CMain.c", and write (modify) C main function body:

```c++
// Referrer translated code.
#include <GettingStartedIL2C.h>

int main()
{
    // Initialize the IL2C runtime.
    il2c_initialize();

    // Invoke (translated) the C# code.
    GettingStartedIL2C_Program_Main();

    // Shutdown the IL2C runtime.
    il2c_shutdown();

    return 0;
}
```

### 1-5. Finished Hello world

Run the GettingStartedIL2CMain project and get result.

* Note: You need to set or choice startup project on GettingStartedIL2CMain (C project) instead C# project.

![Finished Hello world](../images/tutorial151.png)

For additional things when you will be interesting about:

```csharp
using System;
using System.Runtime.InteropServices;

namespace GettingStartedIL2C
{
    public static class Program
    {
        [DllImport("user32.dll")]
        public static extern int MessageBoxW(
           IntPtr hWnd, string text, string caption, int options);
        
        public static void Main() =>
            MessageBoxW(IntPtr.Zero, "Hello world with IL2C!", "Getting started IL2C", 1);
    }
}
```

![Finished Hello world](../images/tutorial152.png)

### 1-6. Visual C++ related hints

If your build random failing, it maybe causes the Visual Studio can't calculate project dependencies. You need to set dependance manually at the "Project dependency" dialog.

![Project dependency dialog](../images/tutorial16.png)

And check it up for success building the entire solution at the "Configuration Manager" dialog.

![Configuration manager dialog](../images/tutorial17.png)

## 2-0. [Prebuild] runtime library using the scripts

I showed up you how to build both app code and runtime manually. Now, you understand it and can use runtime library building scripts named "build-runtime".
It'll generate libs (libil2c*.lib/libil2c*.a) into "IL2C.Runtime/lib" directory. Please build it before doing next step.

1. Run "init-tools.bat" or "init-tools.sh" only first time. It'll setup developing tools. (It'll download mingw toolchain from GitHub if you use Windows environment.) We can give a argument for configuration name both "Debug" and "Release", will implicit apply "Debug" when you don't give it. (The names are important case sensitive.)
2. Run "buil-runtime.bat" or "build-runtime.sh".

## 2. Trying sample for the polish notation calculator

If you understand how to use the IL2C from scratch, you go to the next step. What are we thinking about using the IL2C? A topic is we can apply for OOP design with C# language different the C language.

The next sample code is "The Polish notation calculator." It's the console application, input the PN expression and show result.

![Configuration manager dialog](../images/tutorial21.png)

It has modern design using OOP technics. And the solution contains unit test using the NUnit3. [(The completed projects contain the IL2C repository. You can refer it.)](https://github.com/kekyo/IL2C/tree/master/samples/Calculator)

I introduce the project highlights:

### 2-1. Using OOP features

It contains class-base inheritance technics for the AST nodes (Abstract syntax tree).

![PN calculator AST nodes](../images/tutorial22.png)

For example:

```csharp
// Base class
public abstract class AbstractNode
{
    public readonly int NextIndex;

    protected AbstractNode(int nextIndex)
    {
        this.NextIndex = nextIndex;
    }
}

// Derived class
public class OperatorNode : AbstractNode
{
    public readonly char Operator;

    public OperatorNode(char oper, int nextIndex) : base(nextIndex)
    {
        this.Operator = oper;
    }
}

// Derived class
public abstract class ReducibleNode : AbstractNode
{
    protected ReducibleNode(int nextIndex) : base(nextIndex)
    {
    }

    public abstract int Reduce();
}
```

If you wanna check these node types, please refer [the repository.](https://github.com/kekyo/IL2C/tree/master/samples/Calculator) Everyday we use the OOP technics both the C# and another languages on the .NET. For class inheritance, polymorphism, interface separation, first class function (delegate) and related many others. All technics can use on the IL2C. (Currently limitations are dynamic related features same as the AOP)

### 2-2. The library

First tutorial the Hello world is combined all source codes into the Visual C++ project. But the normally case we separate the libraries.

The IL2C is capable combined into a library. It's two ways:

* Construct managed library and will combine at linking stage into one native project.
  * We gonna apply IL2C.Build package each managed library projects.
* Construct native library from each managed library and linking it.
  * We gonna apply IL2C.Build package each managed library projects, and compile to native binary each projects.

It illustrates the graph:

![The building schemes](../images/tutorial23.png)

The Polish notation calculator has only one library (IL2C.Runtime) and combined at linking into one native project.

### 2-3. The unit tests

You can test with NUnit or likely testing framework. The test technics isn't different for standard knowleges. You can use the dependency injection technics (using abstract types) if you have a platform depended accessor.

## 3. You can witness working the C# code at non-OSes environment

The IL2C maybe shows boring these samples, because it's running on the Win32 environment. The .NET Core already has native binary compilation ability. Is it different?

### 3-1. The IL2C's native binary is VERY TINY footprint.

The runtime usage, there're footprint measured result using ProcessExplorer. Left values are private bytes, it's about smaller than 20 multiple. And right values are working set size, it's about smaller than 5 multiple.

![Measured results using ProcessExplorer](../images/tutorial24.png)

### 3-2. The IL2C's runtime ports easy to another platform include the embedded systems.

The environment doesn't have any operating systems, called "Firmware level programming" or "Bare metal programming".

The third sample targets for the "UEFI." The UEFI is same as the BIOS for PC's. Of course, it's non-OS environment. You can see the UEFI version for the Polish notation calculator.

![The UEFI project](../images/tutorial31.png)

"IL2C.Runtime.UEFI" project is the Visual C++ library project for UEFI (VC++ can build it). And "Calculator.UEFI" project is combined both translated source code and runtime library in it. Then, the project outputs "bootx64.efi" file.

It's truly UEFI application binary file. You can store into the USB flash stick with strictly directory structure:

```
E:.
\---EFI
    \---BOOT
            bootx64.efi
```

[(Prebuilt UEFI x64 binary is here: Calculator.UEFI.x64_sample2.zip)](https://github.com/kekyo/IL2C/releases/download/0.4.50/Calculator.UEFI.x64_sample2.zip)

Then, this USB flash stick insert into target (UEFI enabled) PC, reset and invoke the boot device prompt.

![UEFI boot device prompt](../images/tutorial32.jpg)

![Polish notation calculator bootup on the UEFI](../images/tutorial33.jpg)

## 4. Another way

Another way, I tested the M5Stack IoT device with the numerical keypad extension:

![Calculator.M5Stack](../images/Calculator.M5Stack.jpg)

TODO: write details
