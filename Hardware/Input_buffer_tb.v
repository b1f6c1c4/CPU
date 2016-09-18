`default_nettype none
`timescale 10ns/1ps
module Input_buffer_tb;
`include "INPUT_INTERNAL.v"
`include "INPUT_INTERFACE.v"

   reg Clock;
   reg Reset;
   reg [IC_N-1:0] cmd;
   wire [15:0] SRC;
   wire [15:0] DST;
   wire [IC_N-1:0] ALU_OP;
   wire finish;

   Input_buffer mdl(Clock, Reset, cmd, SRC, DST, ALU_OP, finish);

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
         #1 cmd = IC_NONE;
         #4 cmd = IC_NUM5;
         #4 cmd = IC_NUM6;
         #4 cmd = IC_OPAD;
         #4 cmd = IC_NUM3;
         #4 cmd = IC_NUM7;
         #4 cmd = IC_CTOK;
         #4 cmd = IC_OPSB;
         #4 cmd = IC_NUM4;
         #4 cmd = IC_NUM9;
         #4 cmd = IC_CTOK;
         #4 cmd = IC_CTOK;
         #4 cmd = IC_NUM4;
         #4 cmd = IC_CTOK;
         #4 cmd = IC_OPSB;
         #4 cmd = IC_CTOK;
         #4 cmd = IC_NUM9;
         #4 cmd = IC_CTOK;
         #4 $finish;
      end

   always @(posedge Clock)
      $display("SRC=%d DST=%d ALU_OP=%h finish=%b", SRC, DST, ALU_OP, finish);

endmodule
