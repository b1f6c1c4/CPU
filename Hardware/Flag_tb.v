`default_nettype none
`timescale 10ns/1ps
module Flag_tb;

   reg Clock, Reset;
   reg [7:0] Flagin;
   wire [7:0] Flagout;

   Flag mdl(Clock, Reset, Flagin, Flagout);

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
         Flagin = 8'h55;
         #1;
         #4 Flagin = 8'h33;
         #4 Flagin = Flagout;
         #4 $finish;
      end

endmodule
