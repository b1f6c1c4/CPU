`default_nettype none
module buffer(
   input [N-1:0] in,
   inout [N-1:0] out,
   input en
);
   parameter N = 8;

   assign out = en ? in : 1'bz;

endmodule
