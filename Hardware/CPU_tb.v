`default_nettype none
`timescale 10ns/1ps
module CPU_tb;
`include "CPU_INTERNAL.v"

   reg Clock, Reset;
   wire [7:0] io_ena;
   wire [7:0] io_0, io_1, io_6, io_7;
   wire [7:0] io_2, io_3, io_4, io_5;
   wire [PC_N-1:0] pc_pc;
   wire [7:0] fl_out;
   wire [7:0] R0;
   wire [7:0] R1;
   wire [7:0] R2;
   wire [7:0] R3;
   wire [2:0] cu_aluc;
   wire [15:0] ro_q;

   CPU mdl(.Clock(Clock), .Reset(Reset), .io_ena(io_ena), .io_0(io_0), .io_1(io_1), .io_2(io_2), .io_3(io_3), .io_4(io_4), .io_5(io_5), .io_6(io_6), .io_7(io_7), .pc_pc(pc_pc), .fl_out(fl_out), .R0(R0), .R1(R1), .R2(R2), .R3(R3), .cu_aluc(cu_aluc), .ro_q(ro_q));

   initial
      begin
         Clock = 1;
         forever
            #2 Clock = ~Clock;
      end

   initial
      begin
         Reset = 0;
         #1 Reset = 1;
      end

   initial
      begin
         #(4*1000) $finish;
      end

endmodule
