`default_nettype none
module lpm_ram_256_8(
   input Clock,
   input [7:0] address,
   input [7:0] data,
   input wren, // 1 is write; 0 is read
   output [7:0] q
);

   reg [7:0] ram[0:255];

   assign q = ram[address];

   always @(posedge Clock)
      if (wren)
         ram[address] <= data;

endmodule
