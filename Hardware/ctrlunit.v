`default_nettype none
module ctrlunit(
   input [3:0] OP,
   input ZERO,
   output reg JUMP,
   output reg BRANCH,
   output reg [2:0] ALUC,
   output reg ALUSRCB,
   output reg WRITEMEM,
   output reg WRITEREG,
   output reg MEMTOREG,
   output reg REGDES,
   output reg WRFLAG
);
`include "CPU_INTERNAL.v"
`include "ALU_INTERFACE.v"

   always @(*)
      case (OP)
         OP_JMP : JUMP <= 1'b1;
         default: JUMP <= 1'b0;
      endcase

   always @(*)
      case (OP)
         OP_BEQ: BRANCH <= ZERO;
         OP_BNE: BRANCH <= ~ZERO;
         default: BRANCH <= 1'b0;
      endcase

   always @(*)
      case (OP)
         OP_SLT: ALUC <= AC_LS;
         OP_OR, OP_ORI: ALUC <= AC_OR;
         OP_ADD, OP_ADDI: ALUC <= AC_ADX;
         OP_SUB, OP_BNE, OP_BEQ: ALUC <= AC_SBX;
         OP_AND, OP_ANDI: ALUC <= AC_AN;
         OP_ADDC: ALUC <= AC_AD;
         OP_SUBC: ALUC <= AC_SB;
         default: ALUC <= 3'bx;
      endcase

   always @(*)
      case (OP)
         OP_ORI, OP_ADDI, OP_ANDI, OP_LW, OP_SW:
            ALUSRCB <= 1'b1;
         default: ALUSRCB <= 1'b0;
      endcase

   always @(*)
      case (OP)
         OP_SW: WRITEMEM <= 1'b1;
         default: WRITEMEM <= 1'b0;
      endcase

   always @(*)
      case (OP)
         OP_AND, OP_OR, OP_SUB, OP_ADD, OP_ADDC, OP_SUBC, OP_SLT, OP_LW, OP_ADDI, OP_ANDI, OP_ORI:
            WRITEREG <= 1'b1;
         default: WRITEREG <= 1'b0;
      endcase

   always @(*)
      case (OP)
         OP_LW: MEMTOREG <= 1'b1;
         default: MEMTOREG <= 1'b0;
      endcase

   always @(*)
      case (OP)
         OP_AND, OP_OR, OP_ADD, OP_SUB, OP_ADDC, OP_SUBC, OP_SLT:
            REGDES <= 1'b1;
         default: REGDES <= 1'b0;
      endcase

   always @(*)
      case (OP)
         OP_ADD, OP_SUB, OP_ADDI, OP_ADDC, OP_SUBC:
            WRFLAG <= 1'b1;
         default: WRFLAG <= 1'b0;
      endcase

endmodule
