using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;

namespace Assembler
{
    partial class AsmEParser
    {
        public sealed partial class MacroContext : IFlattenable<IExecutableInstruction>
        {
            /* MEM[0xff] = sp
             * MEM[0xfe] = bp
             */

            private const string Init = @"
ANDI R1, R1, 0x00
ADDI R2, R1, 0xfe
SW   R2, R1, 0xff   ; sp = 0xfe
SW   R2, R1, 0xfe   ; bp = 0xfe
";

            private const string Call = @"
LPCH R2
LPCL R0
ANDI R1, R1, 0x00   ; +0 R0 points here
BNE  R0, R1, 0x01   ; +1
ADDI R2, R2, 0x01   ; +2
ADDI R0, R0, 11     ; +3
ADDC R2, R2, R1     ; +4
LW   R3, R1, 0xff   ; +5
SW   R0, R3, 0xfe   ; +6 MEM[sp-2] = PC_L
SW   R2, R3, 0xff   ; +7 MEM[sp-1] = PC_H
ADDI R3, R3, 0xfe   ; +8
SW   R3, R1, 0xff   ; +9 sp -= 2
JMP  {0}            ; +10
;                   ; +11
";
            private const string Ret = @"
ANDI R1, R1, 0x00
LW   R3, R1, 0xfe   ; R3 = bp
LW   R2, R3, 0x00   ; R2 = MEM[bp] = bp'
SW   R2, R1, 0xfe   ; bp = bp'
ADDI R3, R3, 0x03
SW   R3, R1, 0xff   ; POP bp, PC_L, PC_H
LW   R2, R3, 0xfe   ; R2 = PC_L
LW   R3, R3, 0xff   ; R3 = PC_H
SPC  R3, R2         ; PC = {R3,R2}
";

            private const string Push = @"
ANDI R1, R1, 0x00
LW   R1, R1, 0xff   ; R1 = sp
SW   {0}, R1, 0xff  ; MEM[sp-1] = {0}
ADDI {0}, R1, 0xff
ANDI R1, R1, 0x00
SW   {0}, R1, 0xff  ; sp--
LW   {0}, {0}, 0x00 ; {0} = MEM[sp]
";

            private const string PushBp = @"
ANDI R1, R1, 0x00
LW   R1, R1, 0xff   ; R1 = sp
SW   R0, R1, 0xfe   ; MEM[sp-2] = R0
ANDI R1, R1, 0x00
LW   R0, R1, 0xfe   ; R0 = bp
LW   R1, R1, 0xff   ; R1 = sp
SW   R0, R1, 0xff   ; MEM[sp-1] = bp
ADDI R0, R1, 0xff
ANDI R1, R1, 0x00
SW   R0, R1, 0xff   ; sp--
SW   R0, R1, 0xfe   ; bp = sp
LW   R0, R0, 0xff   ; R0 = MEM[sp-1]
";

            private const string Pop = @"
ANDI R1, R1, 0x00
LW   R1, R1, 0xff
ADDI {0}, R1, 0x01
ANDI R1, R1, 0x00
SW   {0}, R1, 0xff  ; sp++
LW   {0}, {0}, 0xff  ; {0} = MEM[sp-1]
";

            private const string Halt = @"
BEQ R1, R1, 0xff
";

            private const string AddPc = @"
LPCH R2
LPCL R0
ANDI R1, R1, 0x00   ; +0 R0 points here
BNE  R0, R1, 0x01   ; +1
ADDI R2, R2, 0x01   ; +2
LW   R3, R1, 0xff   ; +3
LW   R3, R3, 0x00   ; +4
ADDI R3, R3, 9      ; +5
ADD  R0, R0, R3     ; +6
ADDC R2, R2, R1     ; +7
SPC  R2, R0         ; +8
;                   ;+9
";

            public IReadOnlyList<IExecutableInstruction> Flatten(bool debug)
            {
                if (Debug != null &&
                    !debug)
                    return new List<IExecutableInstruction>();

                switch (Op.Text.ToUpper())
                {
                    case "INIT":
                        return Parse(Init, debug);
                    case "CALL":
                        return Parse(string.Format(Call, obj().GetText()), debug);
                    case "RET":
                        return Parse(Ret, debug);
                    case "ADDPC":
                        return Parse(AddPc, debug);
                    case "HALT":
                        return Parse(Halt, debug);
                    case "PUSH":
                        if (Rx.Text.ToUpper() != "BP" &&
                            SemanticHelper.RegisterNumber(Rx) == 1)
                            throw new ApplicationException("Cannot PUSH R1!");
                        return Parse(Rx.Text.ToUpper() == "BP" ? PushBp : string.Format(Push, Rx.Text), debug);
                    case "POP":
                        if (SemanticHelper.RegisterNumber(Rx) == 1)
                            throw new ApplicationException("Cannot POP R1!");
                        return Parse(string.Format(Pop, Rx.Text), debug);
                    default:
                        throw new InvalidOperationException();
                }
            }

            public string Prettify()
            {
                string str;
                switch (Op.Text.ToUpper())
                {
                    case "INIT":
                    case "ADDPC":
                    case "RET":
                    case "HALT":
                        str = $"{Op.Text.ToUpper()}";
                        break;
                    case "CALL":
                        str = $"{Op.Text.ToUpper().PadRight(4)} {obj().GetText()}";
                        break;
                    case "PUSH":
                        str =
                            $"{Op.Text.ToUpper().PadRight(4)} {(Rx.Text.ToUpper() == "BP" ? "BP" : $"R{SemanticHelper.RegisterNumber(Rx)}")}";
                        break;
                    case "POP":
                        str = $"{Op.Text.ToUpper().PadRight(4)} R{SemanticHelper.RegisterNumber(Rx)}";
                        break;
                    default:
                        throw new InvalidOperationException();
                }
                return (Debug != null ? "#   " : "    ") + str;
            }

            private static IReadOnlyList<IExecutableInstruction> Parse(string str, bool debug)
            {
                var lexer = new AsmELexer(new AntlrInputStream(str));
                var parser = new AsmEParser(new CommonTokenStream(lexer));
                var prog = parser.prog();
                return
                    prog.line()
                        .SelectMany(
                                    l =>
                                    {
                                        if (l.instruction() != null)
                                            return new[] { l.instruction() as IExecutableInstruction };
                                        if (l.macro() != null)
                                            return l.macro().Flatten(debug);
                                        return Enumerable.Empty<IExecutableInstruction>();
                                    }).ToList();
            }
        }
    }
}
