`default_nettype none
`timescale 10ns/1ps
module lpm_rom_256_16_tb;

   reg Clock;
   reg [7:0] address;
   wire [15:0] q;

   lpm_rom_256_16 mdl(
      .clock(Clock),
      .address(address),
      .q(q));

   initial
      begin
         Clock = 1'b1;
         forever
            #2 Clock = ~Clock;
      end

   initial
      begin
         address = 8'h0;
         #1;
         #4 address = 8'h1;
         #4 address = 8'h2;
         #4 $finish;
      end

endmodule
