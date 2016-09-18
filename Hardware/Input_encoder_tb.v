`default_nettype none
`timescale 10ns/1ps
module Input_encoder_tb;
`include "INPUT_INTERNAL.v"
`include "INPUT_INTERFACE.v"

   reg Clock;
   reg Reset;
   reg [15:0] key;
   wire [IC_N-1:0] cmd;

   Input_encoder mdl(Clock, Reset, key, cmd);

   initial
      begin
         Clock = 1;
         forever
            #2 Clock = ~Clock;
      end

   initial
      begin
         Reset = 0;
         #2 Reset = 1;
      end

   initial
      begin
         #1 key = 16'b0000000000000000;
         #4 key = 16'b0100000000000000;
         #4 key = 16'b0100000000000000;
         #4 key = 16'b0100010000000000;
         #4 key = 16'b0100010000000000;
         #4 key = 16'b0100010001000000;
         #4 key = 16'b0000010001000100;
         #4 key = 16'b0000010001000000;
         #4 key = 16'b0000010001010000;
         #4 key = 16'b0000000000010000;
         #4 key = 16'b0000001000000000;
         #4 key = 16'b0000001000000000;
         #4 key = 16'b0000000000000000;
         #4 $finish;
      end

   always @(posedge Clock)
      $display("cmd=%h", cmd);

endmodule
