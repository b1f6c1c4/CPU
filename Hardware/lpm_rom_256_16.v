`default_nettype none
module lpm_rom_256_16(
   input Clock,
   input [7:0] address,
   output reg [15:0] q
);

   reg [15:0] rom[0:255];

   always @(posedge Clock)
      q <= rom[address];

   initial
      $readmemb("ROM.list", rom);

endmodule
