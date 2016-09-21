`default_nettype none
`timescale 10ns/1ps
module Hardware_tb;

   reg Clock, Reset;
   wire [3:0] SD;
   wire [7:0] SEG;
   wire [7:0] LD;
   wire Buzz;
   wire [7:0] io_ena;
   wire [7:0] io_0, io_1, io_6, io_7;
   wire [7:0] io_2, io_3, io_4, io_5;

   Hardware mdl(
      .CLK(Clock),
      .RST(Reset),
      // .H(H), .V(V),
      .SD(SD), .SEG(SEG),
      .LD(LD),
      // .SB(SB),
      .Buzz(Buzz),
      .io_ena(io_ena),
      .io_0(io_0),
      .io_1(io_1),
      .io_2(io_2),
      .io_3(io_3),
      .io_4(io_4),
      .io_5(io_5),
      .io_6(io_6),
      .io_7(io_7));

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
         #(4*100) $finish;
      end

endmodule
