`default_nettype none
module clock_divider(
   input Clock,
   input Reset,
   output result
   );
   parameter div = 10;

   reg [31:0] count;
   assign result = ~|count;

   always @(posedge Clock, negedge Reset)
      if (~Reset)
         count <= div - 1;
      else
         count <= ~|count ? div - 1 : count - 1;

endmodule
