`default_nettype none
`timescale 10ns/1ps
module Hardware_tb;
   parameter T = 4; // edge: k * T
   parameter N = 32;

   reg Clock, Reset;
   wire TX, RX;
   wire [7:0] io_ena;
   wire [7:0] io_0, io_1, io_2, io_3;
   wire [7:0] io_4, io_5, io_6, io_7;

   assign RX = TX;

   Hardware mdl(
      .CLK(Clock),
      .RST(Reset),
      // .H(H), .V(V),
      // .SD(SD), .SEG(SEG),
      // .LD(LD),
      // .SB(SB),
      // .Buzz(Buzz),
      .TX(TX),
      .RX(RX),
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
         #0 Clock = 1;
         forever
            #(T/2) Clock = ~Clock;
      end

   initial
      begin
         #0 Reset = 0;
         #(T/2) Reset = 1;
         #(1000*T) $finish;
      end

endmodule
