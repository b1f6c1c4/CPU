`default_nettype none
`timescale 10ns/1ps
module reg4_8_tb;

   reg Clock, Reset;
   reg [1:0] N1, N2, ND;
   reg [7:0] DI;
   reg REG_WE;
   wire [7:0] Q1, Q2;

   reg4_8 mdl(Clock, Reset, N1, Q1, N2, Q2, ND, DI, REG_WE);

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
         N1 = 2'd0;
         N2 = 2'd1;
         ND = 2'd0;
         DI = 8'h55;
         REG_WE = 1'b0;
         #1;
         #4 REG_WE = 1'b1;
         #4 REG_WE = 1'b0;
         DI = 8'haa;
         ND = 2'd1;
         #4 REG_WE = 1'b1;
         #4 REG_WE = 1'b0;
         #4 $finish;
      end

endmodule
