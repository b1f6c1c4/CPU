`default_nettype none
`timescale 10ns/1ps
module instrconunit_tb;

   reg Clock, Reset;
   reg BRANCH;
   reg JUMP;
   reg [11:0] imm;
   wire [11:0] PC;

   instrconunit mdl(Clock, Reset, BRANCH, JUMP, imm, PC);

   initial
      begin
         Clock = 1'b1;
         forever
            #2 Clock = ~Clock;
      end

   initial
      begin
         Reset = 1'b0;
         #2 Reset = 1'b1;
      end

   initial
      begin
         BRANCH = 0;
         JUMP = 0;
         #1;
         #4 JUMP = 1;
         imm = 12'h5aa;
         #4 imm = 12'h312;
         #4 BRANCH = 1;
         JUMP = 0;
         imm = 12'h010;
         #4 imm = 12'hfff;
         #4 $finish;
      end

endmodule
