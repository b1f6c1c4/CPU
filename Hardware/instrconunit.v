`default_nettype none
module instrconunit(
   input Clock,
   input Reset,
   input BRANCH,
   input JUMP,
   input [7:0] imm,
   output reg [7:0] PC
);

   always @(posedge Clock, negedge Reset)
      if (~Reset)
         PC <= 8'b0;
      else if (JUMP)
         PC <= imm;
      else if (BRANCH)
         PC <= PC + imm + 8'b1;
      else
         PC <= PC + 8'b1;

endmodule
