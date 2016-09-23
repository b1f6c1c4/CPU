`default_nettype none
`timescale 10ns/1ps
module Hardware_tb;
   parameter T = 4; // edge: k * T
   parameter N = 32;

   reg Clock, Reset;
   wire [3:0] SD;
   wire [7:0] SEG;
   wire [7:0] LD;
   wire [3:0] H;
   wire [3:0] V;
   wire Buzz;
   wire [7:0] io_ena;
   wire [7:0] io_0, io_1, io_2, io_3;
   wire [7:0] io_4, io_5, io_6, io_7;

   Hardware mdl(
      .CLK(Clock),
      .RST(Reset),
      .H(H), .V(V),
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

   reg [15:0] connect;

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
      end

   // 0  4  8  12
   // 1  5  9  13
   // 2  6  10 14
   // 3  7  11 15
   pulldown p0(H[0]);
   pulldown p1(H[1]);
   pulldown p2(H[2]);
   pulldown p3(H[3]);
   tranif1 t0(H[0], V[0], connect[0]);
   tranif1 t1(H[1], V[0], connect[1]);
   tranif1 t2(H[2], V[0], connect[2]);
   tranif1 t3(H[3], V[0], connect[3]);
   tranif1 t4(H[0], V[1], connect[4]);
   tranif1 t5(H[1], V[1], connect[5]);
   tranif1 t6(H[2], V[1], connect[6]);
   tranif1 t7(H[3], V[1], connect[7]);
   tranif1 t8(H[0], V[2], connect[8]);
   tranif1 t9(H[1], V[2], connect[9]);
   tranif1 ta(H[2], V[2], connect[10]);
   tranif1 tb(H[3], V[2], connect[11]);
   tranif1 tc(H[0], V[3], connect[12]);
   tranif1 td(H[1], V[3], connect[13]);
   tranif1 te(H[2], V[3], connect[14]);
   tranif1 tf(H[3], V[3], connect[15]);

   initial
      begin
         connect = 16'h0;
         #(4*T+1) connect = 16'h8;
         #(N*T) connect = 16'h0;
         #(4*100) $finish;
      end

endmodule
