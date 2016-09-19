`default_nettype none
module lpm_rom_256_16(
   input Clock,
   input [PC_N-1:0] address,
   output reg [15:0] q
);
`include "CPU_INTERNAL.v"

   reg [15:0] rom[0:(2**PC_N-1)];

   always @(*)
      q <= rom[address];

   initial
      $readmemb("ROM.list", rom);

endmodule
