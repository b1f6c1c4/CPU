`default_nettype none
module instrconunit(
   input Clock,
   input Reset,
   input BRANCH,
   input JUMP,
   input [PC_N-1:0] imm,
   output reg [PC_N-1:0] PC
);
`include "CPU_INTERNAL.v"

   always @(posedge Clock, negedge Reset)
      if (~Reset)
         PC <= 8'b0;
      else if (JUMP)
         PC <= imm;
      else if (BRANCH)
         PC <= PC + imm + 1'b1;
      else
         PC <= PC + 1'b1;

endmodule
