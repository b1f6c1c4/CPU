param([string] $File)

./Assembler/Assembler/bin/Release/Assembler.exe -o ./prog.hex $File

if ($LastExitCode -ne 0)
{
    rm -Force ./prog.hex
    return
}

quartus_stp -t ./Script/prog.tcl

rm ./prog.hex
