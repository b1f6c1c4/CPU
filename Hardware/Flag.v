`default_nettype none
module Flag(
   input Clock,
   input Reset,
   input [7:0] Flagin,
   output reg [7:0] Flagout
);

   always @(posedge Clock, negedge Reset)
      if (~Reset)
         Flagout <= 8'b0;
      else
         Flagout <= Flagin;

endmodule
