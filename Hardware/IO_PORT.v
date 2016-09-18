`default_nettype none
module IO_PORT(
   input [7:0] addr,
   input RE,
   input WE,
   input [7:0] Din,
   output [7:0] Dout,
   output io_read,
   output io_write,
   inout [7:0] IO0,
   inout [7:0] IO1,
   inout [7:0] IO2,
   inout [7:0] IO3,
   inout [7:0] IO4,
   inout [7:0] IO5,
   inout [7:0] IO6,
   inout [7:0] IO7
);

endmodule
