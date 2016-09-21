`default_nettype none
module latch_buffer(
   input Clock,
   input Reset,
   input en,
   input [N-1:0] in,
   output reg [N-1:0] out
);
   parameter N = 8;

   always @(posedge Clock, negedge Reset)
      if (~Reset)
         out <= 1'b0;
      else if (en)
         out <= in;

endmodule
