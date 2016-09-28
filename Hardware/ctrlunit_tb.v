`default_nettype none
`timescale 10ns/1ps
module ctrlunit_tb;
`include "ALU_INTERFACE.v"

   reg [3:0] OP;
   reg ZERO;
   wire JUMP;
   wire BRANCH;
   wire [2:0] ALUC;
   wire ALUSRCB;
   wire WRITEMEM;
   wire WRITEREG;
   wire MEMTOREG;
   wire REGDES;
   wire WRFLAG;

   ctrlunit mdl(OP, ZERO, JUMP, BRANCH, ALUC, ALUSRCB, WRITEMEM, WRITEREG, MEMTOREG, REGDES, WRFLAG);

   initial
      begin
         for (OP = 4'b0; OP < 4'hf; OP = OP + 4'b1)
            begin
               ZERO = 0;
               #4;
               ZERO = 1;
               #4;
            end

         $finish;
      end
endmodule
