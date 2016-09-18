`default_nettype none
// Asynchronous reset, synchronous release
module rst_recover(
   input Clock,
   input RST,
   output reg Reset
   );

   reg mid;
   always @(posedge Clock or negedge RST)
      if (~RST)
         begin
            mid <= 1'b0;
            Reset <= 1'b0;
         end
      else
         begin
            mid <= 1'b1;
            Reset <= mid;
         end

endmodule
