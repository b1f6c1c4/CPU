`default_nettype none
module lpm_ram_256_8(
   input Clock,
   input Reset,
   input [7:0] address,
   input [7:0] data,
   input wren, // 1 is write; 0 is read
   output reg [7:0] q
);

endmodule
