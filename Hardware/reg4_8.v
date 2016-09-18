`default_nettype none
module reg4_8(
   input Clock,
   input Reset,
   // read channel 1
   input [1:0] N1, // reg id
   output reg [7:0] Q1, // data
   // read channel 2
   input [1:0] N2, // reg id
   output reg [7:0] Q2, // data
   // write channel
   input [1:0] ND, // reg id
   input [7:0] DI, // data
   input REG_WE // enable
);

endmodule
