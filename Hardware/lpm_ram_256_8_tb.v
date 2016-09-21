`default_nettype none
`timescale 10ns/1ps
module lpm_ram_256_8_tb;

   reg Clock;
   reg [7:0] address, data;
   reg wren;
   wire [7:0] q;

   lpm_ram_256_8 mdl(
      .clock(Clock),
      .address(address),
      .data(data),
      .wren(wren),
      .q(q));

   initial
      begin
         Clock = 1'b1;
         forever
            #2 Clock = ~Clock;
      end

   initial
      begin
         wren = 1'b0;
         address = 8'h0;
         data = 8'h55;
         #1;
         #4 wren = 1'b1;
         #4 address = 8'h1;
         data = 8'h67;
         #4 address = 8'h2;
         data = 8'hac;
         #4 wren = 1'b0;
         address = 8'h0;
         #4 address = 8'h0;
         #4 address = 8'h1;
         #4 address = 8'h2;
         #4 $finish;
      end

endmodule
