grammar Asm;

/*
 * Parser Rules
 */

prog
    : (line? EOL) *
    ;

line
    : Comment
    | label Comment?
    | instruction Comment?
    ;

label
    : Name ':'
    ;

instruction
    : Debug = '#'? (typeR | typeI | typeJ)
    ;

typeR
    : TypeR Rd=Register ',' Rs=Register ',' Rt=Register
    ;

typeI
    : TypeI Rt=Register ',' Rs=Register ',' number
    | TypeIJ Rs=Register ',' Rt=Register ',' obj
    ;

typeJ
    : TypeJ obj
    ;

obj
    : number
    | Name
    ;

number
	: Decimal | Binary | Hexadecimal
    ;

/*
 * Lexer Rules
 */

TypeR
    : 'AND' | 'OR' | 'ADD' | 'SUB' | 'SLT' | 'SUBC' | 'ADDC'
    ;

TypeI
    : 'ANDI' | 'ORI' | 'ADDI' | 'LW' | 'SW'
    ;

TypeIJ
    : 'BEQ' | 'BNE'
    ;

TypeJ
    : 'JMP'
    ;

Decimal
    : [0-9]+
    ;

Binary
    : '0b' [01]+
    ;

Hexadecimal
    : '0x' [0-9a-fA-F]+
    ;

Number
    : Decimal | Binary | Hexadecimal
    ;

Register
    : 'R0' | 'R1' | 'R2' | 'R3'
    ;

Name
    : [a-zA-Z] [a-zA-Z0-9\-_]*
    ;

Comment
    : ';' ~ [\r\n]* -> skip
    ;

EOL
    : '\r'? '\n'
    ;

WS
    : [ \t] -> skip
    ;
