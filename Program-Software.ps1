param([string] $File)

./Assembler/Assembler/bin/Release/Assembler.exe -o ./prog.hex $File

quartus_stp -t ./Script/prog.tcl

rm ./prog.hex
