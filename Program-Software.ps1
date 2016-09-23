param([string] $File)

./AssemblerCli.exe -o ./prog.hex $File

if ($LastExitCode -ne 0)
{
    rm -Force ./prog.hex
    return
}

quartus_stp -t ./Script/prog.tcl

rm ./prog.hex
