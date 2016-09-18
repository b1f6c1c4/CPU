`default_nettype wire
`timescale 10ns/1ps
module Input_keyboard_tb;
   parameter T = 4; // edge: k * T
   parameter N = 32;

   reg Clock;
   reg Reset;
   wire [3:0] H;
   wire [3:0] V;
   wire [15:0] res;

   Input_keyboard mdl(Clock, Reset, H, V, res);

   reg [15:0] connect, connect_new, connect_xor;

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

   integer i, j;
   initial
      begin
         #0 connect = 16'h0;
         #(T+1);
         for (i = 0; i < 16; i = i + 1)
            for (j = i; j < 16; j = j + 1)
               begin
                  #0 connect_new = 16'b0;
                  #0 connect_new[i] = 1;
                  #0 connect_new[j] = 1;
                  #0 connect_xor = connect_new ^ connect;
                  repeat (8)
                     #T connect = connect ^ connect_xor;
                  #T connect = connect_new;
                  #(N*T-9*T);
               end
         #0 connect = 16'b0;
         #(N*T);
         #(T) $finish;
      end

   initial
      begin
         #(T);
         #(N*T+T-1);
         forever
            begin
               #0 $display("res: %b", res);
               #(N*T);
            end
      end

endmodule
